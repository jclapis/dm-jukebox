using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DMJukebox.Discord.Gateway
{
    internal class GatewayConnection
    {
        private const string DeviceName = "DM Jukebox";

        private static readonly string OperatingSystem;

        private readonly ClientWebSocket Socket;

        private readonly byte[] ReceiveBuffer;

        private readonly byte[] SendBuffer;

        private readonly ArraySegment<byte> ReceiveBufferSegment;

        private readonly object CloseLock;

        private readonly object WriteLock;

        private bool _IsClosing;

        private bool IsClosing
        {
            get
            {
                lock (CloseLock)
                {
                    return _IsClosing;
                }
            }

            set
            {
                lock (CloseLock)
                {
                    _IsClosing = value;
                }
            }
        }

        private readonly CancellationToken CancelToken;

        private Task HeartbeatLoopTask;

        private Task ReceiveLoopTask;

        private int HeartbeatInterval;
        
        private int LatestSequenceNumber;

        private GatewayConnectionStep ConnectionStep;

        private readonly AutoResetEvent VoiceStateUpdateWaiter;

        private readonly AutoResetEvent VoiceServerUpdateWaiter;

        private readonly AutoResetEvent ConnectWaiter;

        private string SessionID;

        private string BotUserID;

        private string VoiceSessionToken;

        private Uri VoiceServerEndpoint;

        private string VoiceSessionID;

        public string AuthenticationToken { get; set; }

        public string GuildID { get; set; }

        public string ChannelID { get; set; }

        static GatewayConnection()
        {
            OperatingSystem = "Unknown";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                OperatingSystem = "windows";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                OperatingSystem = "linux";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                OperatingSystem = "osx";
            }
        }

        public GatewayConnection(CancellationToken CancelToken)
        {
            this.CancelToken = CancelToken;
            CloseLock = new object();
            WriteLock = new object();
            SendBuffer = new byte[65536];
            ReceiveBuffer = new byte[65536];
            ReceiveBufferSegment = new ArraySegment<byte>(ReceiveBuffer);
            Socket = new ClientWebSocket();
            VoiceStateUpdateWaiter = new AutoResetEvent(false);
            VoiceServerUpdateWaiter = new AutoResetEvent(false);
            ConnectWaiter = new AutoResetEvent(false);
        }

        public async Task Connect(Uri WebSocketUri)
        {
            try
            {
                ConnectionStep = GatewayConnectionStep.Disconnected;
                await Socket.ConnectAsync(WebSocketUri, CancellationToken.None);
                ReceiveLoopTask = ReceiveWebsocketMessageLoop();
                ConnectWaiter.WaitOne();
            }
            catch (Exception ex)
            {
                string message = $"Error connecting to discord: {ex.GetDetails()}";
                System.Diagnostics.Debug.WriteLine(message);
            }
        }

        private void HeartbeatLoop()
        {
            while (!IsClosing)
            {
                try
                {
                    Payload heartbeatMessage = new Payload
                    {
                        OpCode = OpCode.Heartbeat,
                        Data = LatestSequenceNumber
                    };
                    SendWebsocketMessage(heartbeatMessage);
                    Task.Delay(HeartbeatInterval, CancelToken).Wait();
                    System.Diagnostics.Debug.WriteLine($"Sent heartbeat with seq {LatestSequenceNumber}");
                }
                catch (TaskCanceledException)
                {

                }
            }
        }

        private void SendWebsocketMessage(Payload Message)
        {
            string serializedMessage = JsonConvert.SerializeObject(Message);
            int bytesWritten = Encoding.UTF8.GetBytes(serializedMessage, 0, serializedMessage.Length, SendBuffer, 0);
            ArraySegment<byte> messageSegment = new ArraySegment<byte>(SendBuffer, 0, bytesWritten);
            lock (WriteLock)
            {
                Socket.SendAsync(messageSegment, WebSocketMessageType.Text, true, CancelToken).Wait();
            }
        }

        private async Task ReceiveWebsocketMessageLoop()
        {
            while (!IsClosing)
            {
                WebSocketReceiveResult result = await Socket.ReceiveAsync(ReceiveBufferSegment, CancelToken);
                if (!result.EndOfMessage)
                {
                    throw new Exception("Too many bytes received (limit is 64k for now)");
                }

                switch (result.MessageType)
                {
                    case WebSocketMessageType.Close:
                        await Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancelToken);
                        return;

                    case WebSocketMessageType.Text:
                        string message = Encoding.UTF8.GetString(ReceiveBuffer, 0, result.Count);
                        System.Diagnostics.Debug.WriteLine($"Received a message from the Discord Server: {message}");
                        Payload payload = JsonConvert.DeserializeObject<Payload>(message);
                        ParsePayloadData(payload);
                        break;

                    case WebSocketMessageType.Binary:
                        throw new Exception("Got a binary websocket message?");
                }
            }
        }

        private void ParsePayloadData(Payload Payload)
        {
            if (Payload.SequenceNumber != null)
            {
                LatestSequenceNumber = Payload.SequenceNumber.Value;
            }

            switch (Payload.OpCode)
            {
                case OpCode.Hello:
                    if (ConnectionStep == GatewayConnectionStep.WaitingForHello)
                    {
                        HelloData helloData = ((JObject)Payload.Data).ToObject<HelloData>();
                        HandleHello(helloData);
                    }
                    break;

                case OpCode.Dispatch:
                    switch (Payload.EventName)
                    {
                        case "READY":
                            if (ConnectionStep == GatewayConnectionStep.WaitingForReady)
                            {
                                ReadyEventData readyData = ((JObject)Payload.Data).ToObject<ReadyEventData>();
                                HandleReady(readyData);
                            }
                            break;

                        case "GUILD_CREATE":
                            System.Diagnostics.Debug.WriteLine("Got a guild create message, ignoring it.");
                            break;

                        case "VOICE_STATE_UPDATE":
                            if (ConnectionStep == GatewayConnectionStep.WaitingForVoiceServerInfo)
                            {
                                VoiceStateUpdateEventData voiceStateData = ((JObject)Payload.Data).ToObject<VoiceStateUpdateEventData>();
                                if (voiceStateData.UserID.Equals(BotUserID))
                                {
                                    VoiceSessionID = voiceStateData.SessionID;
                                    VoiceStateUpdateWaiter.Set();
                                }
                            }
                            break;

                        case "VOICE_SERVER_UPDATE":
                            if (ConnectionStep == GatewayConnectionStep.WaitingForVoiceServerInfo)
                            {
                                VoiceServerUpdateEventData voiceServerData = ((JObject)Payload.Data).ToObject<VoiceServerUpdateEventData>();
                                VoiceSessionToken = voiceServerData.VoiceConnectionToken;
                                VoiceServerEndpoint = new Uri($"wss://{voiceServerData.VoiceServerHostname}");
                                VoiceServerUpdateWaiter.Set();
                            }
                            break;
                    }
                    break;

                case OpCode.HeartbeatAck:
                    System.Diagnostics.Debug.WriteLine("Received a Heartbeat ack.");
                    break;
            }
        }

        private void HandleHello(HelloData Data)
        {
            HeartbeatInterval = Data.HeartbeatInterval;
            IdentifyData data = new IdentifyData
            {
                Token = AuthenticationToken,
                IsCompressionSupported = false,
                LargeThreshold = 50,
                Properties = new IdentifyDataProperties
                {
                    OperatingSystem = OperatingSystem,
                    Device = DeviceName
                },
                ShardInfo = new int[] { 0, 1 }
            };

            Payload message = new Payload
            {
                OpCode = OpCode.Identify,
                Data = JObject.FromObject(data)
            };
            ConnectionStep = GatewayConnectionStep.WaitingForReady;
            SendWebsocketMessage(message);
        }

        private void HandleReady(ReadyEventData Data)
        {
            SessionID = Data.SessionID;
            BotUserID = Data.UserInfo.ID;
            HeartbeatLoopTask = Task.Run((Action)HeartbeatLoop);
            ConnectToVoiceChannel();
        }

        private void ConnectToVoiceChannel()
        {
            VoiceStateUpdateData updateData = new VoiceStateUpdateData
            {
                GuildID = GuildID,
                ChannelID = ChannelID,
                IsDeafened = true,
                IsMuted = false
            };
            Payload message = new Payload
            {
                OpCode = OpCode.VoiceStateUpdate,
                Data = updateData
            };

            ConnectionStep = GatewayConnectionStep.WaitingForVoiceServerInfo;
            VoiceStateUpdateWaiter.WaitOne();
            VoiceServerUpdateWaiter.WaitOne();
            ConnectionStep = GatewayConnectionStep.Connected;
            ConnectWaiter.Set();

        }
    }
}
