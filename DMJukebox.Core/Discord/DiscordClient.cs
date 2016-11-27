using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;

namespace DMJukebox.Discord
{
    internal class DiscordClient
    {
        private const string DeviceName = "DM Jukebox";

        private static readonly string OperatingSystem;

        private static readonly Uri DiscordApiUri;

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

        public string Token { get; set; }

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
        }

        public async Task Connect()
        {
            try
            {
                Uri gatewayRequestUri = new Uri(DiscordApiUri, "gateway");
                HttpClient client = new HttpClient();
                string responseBody = await client.GetStringAsync(gatewayRequestUri);
                GetGatewayResponse response = JsonConvert.DeserializeObject<GetGatewayResponse>(responseBody);

                Uri websocketUri = new Uri($"{response.Url}/?encoding=json&v=5");
                await Socket.ConnectAsync(websocketUri, CancellationToken.None);
                ReceiveLoopTask = Task.Run(ReceiveWebsocketMessageLoop);
            }
            catch(Exception ex)
            {
                string message = $"Error connecting to discord: {ex.GetDetails()}";
                System.Diagnostics.Debug.WriteLine(message);
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
                    HelloData helloData = ((JObject)(Payload.Data)).ToObject<HelloData>();
                    HandleHello(helloData);
                    break;

                case OpCode.Dispatch:
                    switch(Payload.EventName)
                    {
                        case "READY":
                            ReadyData readyData = ((JObject)(Payload.Data)).ToObject<ReadyData>();
                            HandleReady(readyData);
                            break;

                        case "GUILD_CREATE":
                            System.Diagnostics.Debug.WriteLine("Got a guild create message, ignoring it.");
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
                Token = Token,
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
            SendWebsocketMessage(message);
        }

        private void HandleReady(ReadyData Data)
        {
            SessionID = Data.SessionID;
            HeartbeatLoopTask = Task.Run((Action)HeartbeatLoop);
        }

        private void SendWebsocketMessage(Payload Message)
        {
            string serializedMessage = JsonConvert.SerializeObject(Message);
            int bytesWritten = Encoding.UTF8.GetBytes(serializedMessage, 0, serializedMessage.Length, SendBuffer, 0);
            ArraySegment<byte> messageSegment = new ArraySegment<byte>(SendBuffer, 0, bytesWritten);
            lock(WriteLock)
            {
                Socket.SendAsync(messageSegment, WebSocketMessageType.Text, true, CancelSource.Token).Wait();
            }
        }

        private void HeartbeatLoop()
        {
            while(!IsClosing)
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
                catch(TaskCanceledException)
                {

                }
            }
        }

    }
}
