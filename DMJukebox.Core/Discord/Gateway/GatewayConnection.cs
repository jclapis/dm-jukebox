/* ========================================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * ====================================================================== */

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
    /// <summary>
    /// This class establishes and maintains a connection to the
    /// Discord gateway. 
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/gateway.
    /// </remarks>
    internal class GatewayConnection : IDisposable
    {
        /// <summary>
        /// Discord requests a name for the client as part
        /// of the handshake, so we may as well give it the
        /// name of the application here.
        /// </summary>
        private const string DeviceName = "DM Jukebox";

        /// <summary>
        /// Discord also asks for the name of the operating
        /// system during the initial handshake, so this 
        /// is it.
        /// </summary>
        private static readonly string OperatingSystem;

        /// <summary>
        /// This is the websocket used to talk to the gateway
        /// </summary>
        private readonly ClientWebSocket Socket;

        /// <summary>
        /// This is a reusable buffer for receiving messages
        /// from the gateway
        /// </summary>
        private readonly byte[] ReceiveBuffer;

        /// <summary>
        /// This is a reusable buffer for sending messages
        /// to the gateway
        /// </summary>
        private readonly byte[] SendBuffer;

        /// <summary>
        /// The built-in <see cref="ClientWebSocket"/>
        /// implementation requires buffers to be wrapped with
        /// these array segments, so this is what it uses to
        /// receive messages.
        /// </summary>
        private readonly ArraySegment<byte> ReceiveBufferSegment;

        /// <summary>
        /// This is a synchronization object for closing the
        /// receive loop in a thread-safe manner.
        /// </summary>
        private readonly object CloseLock;

        /// <summary>
        /// This is a synchronization object for writing to
        /// the gateway in a thread-safe manner.
        /// </summary>
        private readonly object WriteLock;

        /// <summary>
        /// This is just the backing field for the
        /// <see cref="IsClosing"/> property.
        /// </summary>
        private bool _IsClosing;

        /// <summary>
        /// This is a thread-safe flag that the receive loop
        /// uses to determine if/when it needs to close.
        /// </summary>
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

        /// <summary>
        /// This is the source of the cancel token
        /// </summary>
        private readonly CancellationTokenSource CancelSource;

        /// <summary>
        /// This is a token used to interrupt and cancel
        /// asynchronous operations.
        /// </summary>
        private readonly CancellationToken CancelToken;

        /// <summary>
        /// This is the task that sends heartbeats to the
        /// gateway.
        /// </summary>
        private Task HeartbeatLoopTask;

        /// <summary>
        /// This is the task that receives messages from
        /// the gateway.
        /// </summary>
        private Task ReceiveLoopTask;

        /// <summary>
        /// This is the interval that we should wait between
        /// sending heartbeat messages to the server.
        /// </summary>
        private int HeartbeatInterval;

        /// <summary>
        /// This is the latest "sequence number" we received
        /// from the gateway's messages. It comes from 
        /// <see cref="Payload.SequenceNumber"/>. When we send
        /// some messages back to the server, we need to use
        /// this value to tell it what the most recent message
        /// we received was.
        /// </summary>
        private int LatestSequenceNumber;

        /// <summary>
        /// The current step in the gateway handshake process
        /// </summary>
        private GatewayConnectionStep ConnectionStep;

        /// <summary>
        /// This is a synchronization object used during the
        /// handshake process. It tells the handshake thread
        /// when the receive thread has received a
        /// <see cref="VoiceStateUpdateData"/> message.
        /// </summary>
        private readonly AutoResetEvent VoiceStateUpdateWaiter;

        /// <summary>
        /// This is a synchronization object used during the
        /// handshake process. It tells the handshake thread
        /// when the receive thread has received a
        /// <see cref="VoiceServerUpdateEventData"/> message.
        /// </summary>
        private readonly AutoResetEvent VoiceServerUpdateWaiter;

        /// <summary>
        /// This is a synchronization object used to tell the
        /// thread that initially requested the gateway
        /// connection when the connection has been established.
        /// </summary>
        private readonly AutoResetEvent ConnectWaiter;

        /// <summary>
        /// This is the unique ID for the current session,
        /// provided by the gateway.
        /// </summary>
        private string SessionID;

        /// <summary>
        /// This is the unique ID for the Discord bot's user
        /// account.
        /// </summary>
        public string BotUserID { get; private set; }

        /// <summary>
        /// This is the special authentication token the gateway
        /// provided for the current voice session.
        /// </summary>
        public string VoiceSessionToken { get; private set; }

        /// <summary>
        /// This is the hostname of the voice server to connect
        /// to in order to send audio data.
        /// </summary>
        public string VoiceServerHostname { get; private set; }

        /// <summary>
        /// This is the unique ID of the voice session provided
        /// by the gateway, used to connect to the voice server.
        /// </summary>
        public string VoiceSessionID { get; private set; }

        /// <summary>
        /// This is the authentication token for the Discord
        /// bot account. It's like a password for bots.
        /// </summary>
        public string AuthenticationToken { get; set; }

        /// <summary>
        /// This is the ID of the guild / server that owns
        /// the target Discord voice channel.
        /// </summary>
        public string GuildID { get; set; }

        /// <summary>
        /// This is the ID of the voice channel that you want
        /// to play audio over.
        /// </summary>
        public string ChannelID { get; set; }

        /// <summary>
        /// The static constructor just figures out what OS is
        /// currently running, since Discord needs that during the
        /// handshake.
        /// </summary>
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

        /// <summary>
        /// Creates a new GatewayConnection instance.
        /// </summary>
        /// <param name="CancelToken">A token for cancelling
        /// asynchronous operations</param>
        public GatewayConnection()
        {
            CancelSource = new CancellationTokenSource();
            CancelToken = CancelSource.Token;
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

        /// <summary>
        /// Connects to the Discord gateway.
        /// </summary>
        /// <param name="WebSocketUri">The URL of the gateway's websocket
        /// server to connect to</param>
        /// <returns>The task running the method</returns>
        public async Task Connect(Uri WebSocketUri)
        {
            try
            {
                await Socket.ConnectAsync(WebSocketUri, CancellationToken.None);
                ConnectionStep = GatewayConnectionStep.WaitingForHello;
                ReceiveLoopTask = Task.Run(ReceiveWebsocketMessageLoop);
                ConnectWaiter.WaitOne();
            }
            catch (Exception ex)
            {
                string message = $"Error connecting to discord: {ex.GetDetails()}";
                System.Diagnostics.Debug.WriteLine(message);
            }
        }

        /// <summary>
        /// This is the body of the <see cref="HeartbeatLoopTask"/>, which sends
        /// heartbeat messages to the gateway at regular intervals.
        /// </summary>
        private void HeartbeatLoop()
        {
            while (!IsClosing)
            {
                Payload heartbeatMessage = new Payload
                {
                    OpCode = OpCode.Heartbeat,
                    Data = LatestSequenceNumber
                };
                SendWebsocketMessage(heartbeatMessage);
                System.Diagnostics.Debug.WriteLine($"Sent heartbeat with seq {LatestSequenceNumber}");
                try
                {
                    Task.Delay(HeartbeatInterval, CancelToken).Wait();
                }
                catch (AggregateException)
                {
                    return;
                }
                catch (TaskCanceledException)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Writes a message to the gateway. This function is thread-safe.
        /// </summary>
        /// <param name="Message">The message to write</param>
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

        /// <summary>
        /// This is the body of the <see cref="ReceiveLoopTask"/>, which receives messages
        /// from the gateway and processes them.
        /// </summary>
        /// <returns>The task running the method</returns>
        private async Task ReceiveWebsocketMessageLoop()
        {
            while (!IsClosing)
            {
                WebSocketReceiveResult result = null;
                int totalBytesReceived = 0;
                try
                {
                    // Read bytes from the websocket over and over until the whole message has been processed
                    result = await Socket.ReceiveAsync(ReceiveBufferSegment, CancelToken);
                    int receiveOffset = 0;
                    totalBytesReceived = result.Count;
                    while (!result.EndOfMessage)
                    {
                        int bytesReceived = result.Count;
                        receiveOffset += bytesReceived;
                        if (receiveOffset >= ReceiveBuffer.Length)
                        {
                            throw new Exception("Too many bytes received (limit is 64k for now)");
                        }
                        ArraySegment<byte> segment = new ArraySegment<byte>(ReceiveBuffer, receiveOffset, ReceiveBuffer.Length - receiveOffset);
                        result = await Socket.ReceiveAsync(segment, CancelToken);
                        totalBytesReceived += result.Count;
                    }
                }
                catch (AggregateException)
                {
                    return;
                }
                catch (TaskCanceledException)
                {
                    return;
                }

                switch (result.MessageType)
                {
                    case WebSocketMessageType.Close:
                        await Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancelToken);
                        return;

                    case WebSocketMessageType.Text:
                        string message = Encoding.UTF8.GetString(ReceiveBuffer, 0, totalBytesReceived);
                        System.Diagnostics.Debug.WriteLine($"Received a message from the Discord Gateway: {message}");
                        Payload payload = JsonConvert.DeserializeObject<Payload>(message);

                        // Don't await the processing, let it run in a separate thread so we can keep receiving messages
                        Task task = Task.Run(() => HandleNewMessage(payload));
                        break;

                    case WebSocketMessageType.Binary:
                        throw new Exception("Got a binary websocket message?");
                }
            }
        }

        /// <summary>
        /// Handles a new message from the gateway.
        /// </summary>
        /// <param name="Payload">The new message to handle</param>
        private void HandleNewMessage(Payload Payload)
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
                        // We only care about this while waiting for one as part of the
                        // connection process.
                        case "READY":
                            if (ConnectionStep == GatewayConnectionStep.WaitingForReady)
                            {
                                ReadyEventData readyData = ((JObject)Payload.Data).ToObject<ReadyEventData>();
                                HandleReady(readyData);
                            }
                            break;

                        // This gets sent as part of the handshake but we can ignore it.
                        case "GUILD_CREATE":
                            break;

                        // We only care about this while waiting for one as part of the
                        // connection process.
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

                        // We only care about this while waiting for one as part of the
                        // connection process.
                        case "VOICE_SERVER_UPDATE":
                            if (ConnectionStep == GatewayConnectionStep.WaitingForVoiceServerInfo)
                            {
                                VoiceServerUpdateEventData voiceServerData = ((JObject)Payload.Data).ToObject<VoiceServerUpdateEventData>();
                                VoiceSessionToken = voiceServerData.VoiceConnectionToken;
                                VoiceServerHostname = voiceServerData.VoiceServerHostname;
                                VoiceServerUpdateWaiter.Set();
                            }
                            break;
                    }
                    break;

                case OpCode.HeartbeatAck:
                    System.Diagnostics.Debug.WriteLine("Received a Gateway Heartbeat ack.");
                    break;
            }
        }

        /// <summary>
        /// Handles a hello message from the server.
        /// </summary>
        /// <param name="Data">The data from the hello message</param>
        private void HandleHello(HelloData Data)
        {
            // This is the first step in the handshake process,
            // so it provides a lot of the stuff we need during
            // later steps.
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

            // Once we have what we need, we can send an Identify
            // message to continue the handshake process.
            Payload message = new Payload
            {
                OpCode = OpCode.Identify,
                Data = JObject.FromObject(data)
            };
            ConnectionStep = GatewayConnectionStep.WaitingForReady;
            SendWebsocketMessage(message);
        }

        /// <summary>
        /// Handles a ready message form the server.
        /// </summary>
        /// <param name="Data">The data from the ready message</param>
        private void HandleReady(ReadyEventData Data)
        {
            // This is the second step in the handshake process,
            // which gives some more info. At this point we can
            // establish a connection to the voice channel.
            SessionID = Data.SessionID;
            BotUserID = Data.UserInfo.ID;
            HeartbeatLoopTask = Task.Run((Action)HeartbeatLoop);
            GetVoiceChannelConnectionInfo();
        }

        /// <summary>
        /// Asks the gateway to provide connection information for
        /// the voice server.
        /// </summary>
        private void GetVoiceChannelConnectionInfo()
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
            SendWebsocketMessage(message);
            VoiceStateUpdateWaiter.WaitOne();
            VoiceServerUpdateWaiter.WaitOne();
            ConnectionStep = GatewayConnectionStep.Connected;
            ConnectWaiter.Set();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Socket.State == WebSocketState.Open)
                    {
                        Socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Shutting down", CancellationToken.None).Wait();
                    }
                    IsClosing = true;
                    CancelSource.Cancel();
                    if (HeartbeatLoopTask != null && HeartbeatLoopTask.Status != TaskStatus.Canceled)
                    {
                        try
                        {
                            HeartbeatLoopTask.Wait();
                        }
                        catch (AggregateException)
                        {

                        }
                        HeartbeatLoopTask = null;
                    }
                    if (ReceiveLoopTask != null && ReceiveLoopTask.Status != TaskStatus.Canceled)
                    {
                        try
                        {
                            ReceiveLoopTask.Wait();
                        }
                        catch (AggregateException)
                        {

                        }
                        ReceiveLoopTask = null;
                    }
                    Socket.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
