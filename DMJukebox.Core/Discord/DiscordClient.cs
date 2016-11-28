using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DMJukebox.Discord
{
    internal class DiscordClient
    {
        private const string DeviceName = "DM Jukebox";

        private static readonly string OperatingSystem;

        private static readonly Uri DiscordApiUri;

        private readonly ClientWebSocket Socket;

        private ClientWebSocket VoiceSocket;

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
                lock(CloseLock)
                {
                    return _IsClosing;
                }
            }

            set
            {
                lock(CloseLock)
                {
                    _IsClosing = value;
                }
            }
        }

        private readonly CancellationTokenSource CancelSource;

        private Task HeartbeatLoopTask;

        private Task ReceiveLoopTask;

        private int HeartbeatInterval;

        private int LatestSequenceNumber;

        private string SessionID;

        private string BotUserID;

        private string VoiceSessionID;

        public string AuthenticationToken { get; set; }

        public string GuildID { get; set; }

        public string ChannelID { get; set; }

        private ClientStep CurrentStep;

        private readonly AutoResetEvent VoiceStateUpdateWaiter;

        private readonly AutoResetEvent VoiceServerUpdateWaiter;

        private string VoiceSessionToken;

        private Uri VoiceServerEndpoint;

        static DiscordClient()
        {
            DiscordApiUri = new Uri("https://discordapp.com/api/");
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

        public DiscordClient()
        {
            CloseLock = new object();
            WriteLock = new object();
            CancelSource = new CancellationTokenSource();
            SendBuffer = new byte[65536];
            ReceiveBuffer = new byte[65536];
            ReceiveBufferSegment = new ArraySegment<byte>(ReceiveBuffer);
            Socket = new ClientWebSocket();
            CurrentStep = ClientStep.Disconnected;
            VoiceStateUpdateWaiter = new AutoResetEvent(false);
            VoiceServerUpdateWaiter = new AutoResetEvent(false);
        }

        public async Task Connect()
        {
            try
            {
                CurrentStep = ClientStep.Disconnected;
                Uri gatewayRequestUri = new Uri(DiscordApiUri, "gateway");
                HttpClient client = new HttpClient();
                string responseBody = await client.GetStringAsync(gatewayRequestUri);
                GetGatewayResponse response = JsonConvert.DeserializeObject<GetGatewayResponse>(responseBody);

                Uri websocketUri = new Uri($"{response.Url}/?encoding=json&v=5");
                CurrentStep = ClientStep.WaitingForHello;
                await Socket.ConnectAsync(websocketUri, CancellationToken.None);
                ReceiveLoopTask = Task.Run(ReceiveWebsocketMessageLoop);
            }
            catch(Exception ex)
            {
                string message = $"Error connecting to discord: {ex.GetDetails()}";
                System.Diagnostics.Debug.WriteLine(message);
            }
        }

        private void SendWebsocketMessage(Payload Message)
        {
            string serializedMessage = JsonConvert.SerializeObject(Message);
            int bytesWritten = Encoding.UTF8.GetBytes(serializedMessage, 0, serializedMessage.Length, SendBuffer, 0);
            ArraySegment<byte> messageSegment = new ArraySegment<byte>(SendBuffer, 0, bytesWritten);
            lock (WriteLock)
            {
                Socket.SendAsync(messageSegment, WebSocketMessageType.Text, true, CancelSource.Token).Wait();
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
                    Task.Delay(HeartbeatInterval, CancelSource.Token).Wait();
                    System.Diagnostics.Debug.WriteLine($"Sent heartbeat with seq {LatestSequenceNumber}");
                }
                catch (TaskCanceledException)
                {

                }
            }
        }

        private async Task ReceiveWebsocketMessageLoop()
        {
            while(!IsClosing)
            {
                WebSocketReceiveResult result = await Socket.ReceiveAsync(ReceiveBufferSegment, CancelSource.Token);
                if (!result.EndOfMessage)
                {
                    throw new Exception("Too many bytes received (limit is 64k for now)");
                }

                switch (result.MessageType)
                {
                    case WebSocketMessageType.Close:
                        await Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancelSource.Token);
                        return;

                    case WebSocketMessageType.Text:
                        string message = Encoding.UTF8.GetString(ReceiveBuffer, 0, result.Count);
                        System.Diagnostics.Debug.WriteLine($"Received a message from the Discord Server: {message}");
                        Payload payload = JsonConvert.DeserializeObject<Payload>(message);
                        await ParsePayloadData(payload);
                        break;

                    case WebSocketMessageType.Binary:
                        throw new Exception("Got a binary websocket message?");
                }
            }
        }
        
        private async Task ParsePayloadData(Payload Payload)
        {
            if (Payload.SequenceNumber != null)
            {
                LatestSequenceNumber = Payload.SequenceNumber.Value;
            }

            switch (Payload.OpCode)
            {
                case OpCode.Hello:
                    if(CurrentStep == ClientStep.WaitingForHello)
                    {
                        HelloData helloData = ((JObject)Payload.Data).ToObject<HelloData>();
                        HandleHello(helloData);
                    }
                    break;

                case OpCode.Dispatch:
                    switch(Payload.EventName)
                    {
                        case "READY":
                            if (CurrentStep == ClientStep.WaitingForReady)
                            {
                                ReadyEventData readyData = ((JObject)Payload.Data).ToObject<ReadyEventData>();
                                await HandleReady(readyData);
                            }
                            break;

                        case "GUILD_CREATE":
                            System.Diagnostics.Debug.WriteLine("Got a guild create message, ignoring it.");
                            break;

                        case "VOICE_STATE_UPDATE":
                            if(CurrentStep == ClientStep.WaitingForVoiceServerInfo)
                            {
                                VoiceStateUpdateEventData voiceStateData = ((JObject)Payload.Data).ToObject<VoiceStateUpdateEventData>();
                                if(voiceStateData.UserID.Equals(BotUserID))
                                {
                                    VoiceSessionID = voiceStateData.SessionID;
                                    VoiceStateUpdateWaiter.Set();
                                }
                            }
                            break;

                        case "VOICE_SERVER_UPDATE":
                            if(CurrentStep == ClientStep.WaitingForVoiceServerInfo)
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
            CurrentStep = ClientStep.WaitingForReady;
            SendWebsocketMessage(message);
        }

        private async Task HandleReady(ReadyEventData Data)
        {
            SessionID = Data.SessionID;
            BotUserID = Data.UserInfo.ID;
            HeartbeatLoopTask = Task.Run((Action)HeartbeatLoop);
            await ConnectToVoiceChannel();
        }

        private async Task ConnectToVoiceChannel()
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

            CurrentStep = ClientStep.WaitingForVoiceServerInfo;
            VoiceStateUpdateWaiter.WaitOne();
            VoiceServerUpdateWaiter.WaitOne();

            VoiceSocket = new ClientWebSocket();
            await VoiceSocket.ConnectAsync(VoiceServerEndpoint, CancelSource.Token);
            VoiceIdentifyData identifyData = new VoiceIdentifyData
            {
                ServerID = GuildID,
                UserID = BotUserID,
                SessionID = VoiceSessionID,
                Token = VoiceSessionToken
            };
            Payload identifyPayload = new Payload
            {
                OpCode = 0,
                Data = identifyData
            };
            string serializedPayload = JsonConvert.SerializeObject(identifyPayload);
            byte[] payloadBytes = Encoding.UTF8.GetBytes(serializedPayload);
            ArraySegment<byte> payloadSegment = new ArraySegment<byte>(payloadBytes);
            CurrentStep = ClientStep.WaitingForVoiceReady;
            await VoiceSocket.SendAsync(payloadSegment, WebSocketMessageType.Text, true, CancelSource.Token);

            byte[] voiceReceiveBuffer = new byte[65536];
            ArraySegment<byte> voiceReceiveSegment = new ArraySegment<byte>(voiceReceiveBuffer);
            while(true)
            {
                WebSocketReceiveResult result = await VoiceSocket.ReceiveAsync(voiceReceiveSegment, CancelSource.Token);
                if(!result.EndOfMessage)
                {
                    throw new Exception("Voice socket receive buffer overloaded while waiting for a Ready message.");
                }
                string responseMessage = Encoding.UTF8.GetString(voiceReceiveBuffer, 0, result.Count);
                System.Diagnostics.Debug.WriteLine($"Received a message from the Discord Voice Server: {responseMessage}");
                Payload payload = JsonConvert.DeserializeObject<Payload>(responseMessage);
                if((int)payload.OpCode != 2)
                {
                    System.Diagnostics.Debug.WriteLine($"Message was OP code {payload.OpCode}, expected 2.");
                    continue;
                }

            }
        }

    }
}
