using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DMJukebox.Discord.Voice
{
    internal class VoiceClient
    {
        private readonly ClientWebSocket Socket;

        private readonly CancellationToken CancelToken;

        private readonly object WriteLock;

        private readonly object CloseLock;

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

        private readonly byte[] ReceiveBuffer;

        private readonly byte[] SendBuffer;

        private readonly ArraySegment<byte> ReceiveBufferSegment;

        private Task ReceiveLoopTask;

        private Task HeartbeatLoopTask;

        private int LatestSequenceNumber;

        private VoiceConnectionStep ConnectionStep;

        public string GuildID { get; set; }

        private uint SynchronizationSource;

        private int HeartbeatInterval;

        private string VoiceServerHostname;

        private VoiceUdpConnection VoiceChannel;

        private AutoResetEvent ConnectWaiter;

        public VoiceClient(CancellationToken CancelToken)
        {
            this.CancelToken = CancelToken;
            ConnectWaiter = new AutoResetEvent(false);
            CloseLock = new object();
            WriteLock = new object();
            Socket = new ClientWebSocket();
            SendBuffer = new byte[65536];
            ReceiveBuffer = new byte[65536];
            ReceiveBufferSegment = new ArraySegment<byte>(ReceiveBuffer);
        }

        public async Task Connect(string BotUserID, string VoiceSessionID, string VoiceSessionToken, string VoiceServerHostname)
        {
            ConnectionStep = VoiceConnectionStep.Disconnected;
            this.VoiceServerHostname = VoiceServerHostname.Substring(0, VoiceServerHostname.IndexOf(':'));
            Uri endpointUri = new Uri($"wss://{this.VoiceServerHostname}");
            await Socket.ConnectAsync(endpointUri, CancelToken);
            ReceiveLoopTask = Task.Run(ReceiveWebsocketMessageLoop);
            VoiceIdentifyData identifyData = new VoiceIdentifyData
            {
                ServerID = GuildID,
                UserID = BotUserID,
                SessionID = VoiceSessionID,
                Token = VoiceSessionToken
            };
            Payload identifyPayload = new Payload
            {
                OpCode = OpCode.Identify,
                Data = identifyData
            };
            ConnectionStep = VoiceConnectionStep.WaitForReady;
            SendWebsocketMessage(identifyPayload);
            ConnectWaiter.WaitOne();
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
                        System.Diagnostics.Debug.WriteLine("Voice connection got a close message.");
                        await Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancelToken);
                        return;

                    case WebSocketMessageType.Text:
                        string message = Encoding.UTF8.GetString(ReceiveBuffer, 0, result.Count);
                        System.Diagnostics.Debug.WriteLine($"Received a message from the Voice Server: {message}");
                        Payload payload = JsonConvert.DeserializeObject<Payload>(message);
                        Task parseTask = Task.Run(() => ParsePayloadData(payload));
                        break;

                    case WebSocketMessageType.Binary:
                        System.Diagnostics.Debug.WriteLine("Voice connection got a binary message?");
                        break;
                        //throw new Exception("Got a binary websocket message?");
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
                case OpCode.Ready:
                    if (ConnectionStep == VoiceConnectionStep.WaitForReady)
                    {
                        ReadyData readyData = ((JObject)Payload.Data).ToObject<ReadyData>();
                        HandleReady(readyData);
                    }
                    break;

                case OpCode.SessionDescription:
                    if(ConnectionStep == VoiceConnectionStep.WaitForSessionDescription)
                    {
                        SessionDescription description = ((JObject)Payload.Data).ToObject<SessionDescription>();
                        VoiceChannel.SecretKey = description.SecretKey;
                        ConnectWaiter.Set();
                    }
                    break;

                case OpCode.Heartbeat:
                    System.Diagnostics.Debug.WriteLine("Received a Voice Heartbeat ack.");
                    break;

                default:
                    System.Diagnostics.Debug.WriteLine($"VoiceClient got a message with code {Payload.OpCode}, ignoring it.");
                    break;
            }
        }

        private void HandleReady(ReadyData Data)
        {
            SynchronizationSource = Data.SynchronizationSource;
            HeartbeatInterval = Data.HeartbeatInterval;
            VoiceChannel = new VoiceUdpConnection(VoiceServerHostname, Data.Port, SynchronizationSource);
            IPEndPoint localEndpoint = VoiceChannel.DiscoverAddress();

            SelectProtocolData data = new SelectProtocolData
            {
                Port = localEndpoint.Port,
                Address = localEndpoint.Address.ToString(),
                Mode = "xsalsa20_poly1305"
            };
            SelectProtocol message = new SelectProtocol
            {
                Protocol = "udp",
                Data = data
            };
            Payload payload = new Payload
            {
                OpCode = OpCode.SelectProtocol,
                Data = message
            };

            HeartbeatLoopTask = Task.Run((Action)HeartbeatLoop);
            ConnectionStep = VoiceConnectionStep.WaitForSessionDescription;
            SendWebsocketMessage(payload);
        }

        public void AddPlaybackData(float[] PlaybackData, int NumberOfSamplesToWrite)
        {
            VoiceChannel.AddPlaybackData(PlaybackData, NumberOfSamplesToWrite);
        }

        public void Start()
        {
            SetSpeaking data = new SetSpeaking
            {
                IsSpeaking = true,
                Delay = 0
            };
            Payload message = new Payload
            {
                SequenceNumber = LatestSequenceNumber,
                OpCode = OpCode.Speaking,
                Data = data
            };
            SendWebsocketMessage(message);
            VoiceChannel.StartSending();
        }

        public void Stop()
        {
            SetSpeaking data = new SetSpeaking
            {
                IsSpeaking = false,
                Delay = 0
            };
            Payload message = new Payload
            {
                SequenceNumber = LatestSequenceNumber,
                OpCode = OpCode.Speaking,
                Data = data
            };
            SendWebsocketMessage(message);
            VoiceChannel.StopSending();
        }

    }
}
