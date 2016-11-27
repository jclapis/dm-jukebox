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

namespace DMJukebox.Discord
{
    internal class DiscordClient
    {
        private static readonly Uri DiscordApiUri;

        private readonly ClientWebSocket Socket;

        private readonly byte[] ReceiveBuffer;

        private readonly ArraySegment<byte> ReceiveBufferSegment;

        private readonly object CloseLock;

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

        private Task HearbeatLoopTask;

        private int HeartbeatInterval;

        static DiscordClient()
        {
            DiscordApiUri = new Uri("https://discordapp.com/api/");
        }

        public DiscordClient()
        {
            CloseLock = new object();
            CancelSource = new CancellationTokenSource();
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
                WebSocketReceiveResult result = await Socket.ReceiveAsync(ReceiveBufferSegment, CancelSource.Token);
                switch (result.MessageType)
                {
                    case WebSocketMessageType.Close:
                        await Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancelSource.Token);
                        return;

                    case WebSocketMessageType.Text:
                        string msg = Encoding.UTF8.GetString(ReceiveBuffer, 0, result.Count);
                        System.Diagnostics.Debug.WriteLine($"Received a message from the Discord Server: {msg}");
                        return;

                    case WebSocketMessageType.Binary:
                        int receivedBytes = result.Count;
                        msg = BitConverter.ToString(ReceiveBuffer, 0, receivedBytes);
                        System.Diagnostics.Debug.WriteLine($"Received a message from the Discord Server: {msg}");
                        return;
                }
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
                        GatewayPayload payload = JsonConvert.DeserializeObject<GatewayPayload>(message);
                        break;

                    case WebSocketMessageType.Binary:
                        throw new Exception("Got a binary websocket message?");
                }
            }
        }
        
        private void ParsePayloadData(GatewayPayload Payload)
        {
            switch(Payload.OpCode)
            {
                case GatewayOpCode.Hello:
                    GatewayHelloData data = Payload.Data.ToObject<GatewayHelloData>();
                    HandleHello(data);
                    break;
            }
        }
        
        private void HandleHello(GatewayHelloData Data)
        {

        }

        private void SendWebsocketMessages()
        {

        }

        private void HeartbeatLoop()
        {
            while(true)
            {
                try
                {
                    //Socket.SendAsync()
                }
            }
        }

    }
}
