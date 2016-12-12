/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

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
    /// <summary>
    /// This class handles the connection to the Discord voice server
    /// (specifically, the websocket used to set the connection up
    /// and maintain it).
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/voice-connections.
    /// </remarks>
    internal class VoiceClient
    {
        /// <summary>
        /// This is the websocket client that connects to the voice
        /// server
        /// </summary>
        private readonly ClientWebSocket Socket;

        /// <summary>
        /// A synchronization object for cancelling long asynchronous
        /// operations
        /// </summary>
        private readonly CancellationToken CancelToken;

        /// <summary>
        /// This is a synchronization object that's used to write
        /// methods in a thread-safe way
        /// </summary>
        private readonly object WriteLock;

        /// <summary>
        /// This is a synchronization object that's used to close
        /// the receive task in a thread-safe way
        /// </summary>
        private readonly object CloseLock;

        /// <summary>
        /// This is just a backing field for the 
        /// <see cref="IsClosing"/> property.
        /// </summary>
        private bool _IsClosing;

        /// <summary>
        /// This flag is used to tell the receieve task when it
        /// needs to shut down.
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
        /// This is a reusable buffer for storing received messages
        /// sent from the server
        /// </summary>
        private readonly byte[] ReceiveBuffer;

        /// <summary>
        /// This is a reusable buffer for storing messages sent
        /// to the server
        /// </summary>
        private readonly byte[] SendBuffer;

        /// <summary>
        /// This is the ArraySegment mapped to the receive buffer,
        /// which the websocket client uses to receive data.
        /// </summary>
        private readonly ArraySegment<byte> ReceiveBufferSegment;

        /// <summary>
        /// This is the task that receives messages from the server.
        /// </summary>
        private Task ReceiveLoopTask;

        /// <summary>
        /// This is the task that sends heartbeat messages to the
        /// server.
        /// </summary>
        private Task HeartbeatLoopTask;

        /// <summary>
        /// This is the current step in the connection process.
        /// </summary>
        private VoiceConnectionStep ConnectionStep;

        /// <summary>
        /// This is the ID of the server / guild that owns the voice
        /// channel you want to connect to.
        /// </summary>
        public string GuildID { get; set; }

        /// <summary>
        /// This is the synchronization source, which is used during
        /// voice data encryption.
        /// </summary>
        private uint SynchronizationSource;

        /// <summary>
        /// This is the interval to wait between sending heartbeat
        /// message to the server
        /// </summary>
        private int HeartbeatInterval;

        /// <summary>
        /// This is the hostname of the Discord voice server.
        /// </summary>
        private string VoiceServerHostname;

        /// <summary>
        /// This class manages the UDP connection for sending
        /// audio data to Discord.
        /// </summary>
        private VoiceUdpConnection VoiceChannel;

        /// <summary>
        /// This is a synchronization object that blocks until
        /// the connection process is complete.
        /// </summary>
        private AutoResetEvent ConnectWaiter;

        /// <summary>
        /// Creates a new VoiceClient instance.
        /// </summary>
        /// <param name="CancelToken">A cancellation token that
        /// can be used to interrupt asynchronous tasks</param>
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

        /// <summary>
        /// Connects to the Discord voice server, opening the
        /// channel so you can begin sending audio data to it.
        /// </summary>
        /// <param name="BotUserID">The ID of the bot account to connect with</param>
        /// <param name="VoiceSessionID">The ID of the voice connection's session</param>
        /// <param name="VoiceSessionToken">The unique authentication token for this
        /// voice session</param>
        /// <param name="VoiceServerHostname">The hostname of the voice server to
        /// connect to</param>
        /// <returns>The task running the method</returns>
        public async Task Connect(string BotUserID, string VoiceSessionID, string VoiceSessionToken, string VoiceServerHostname)
        {
            // A little initialization action
            ConnectionStep = VoiceConnectionStep.Disconnected;
            this.VoiceServerHostname = VoiceServerHostname.Substring(0, VoiceServerHostname.IndexOf(':'));
            Uri endpointUri = new Uri($"wss://{this.VoiceServerHostname}");
            await Socket.ConnectAsync(endpointUri, CancelToken);
            ReceiveLoopTask = Task.Run(ReceiveWebsocketMessageLoop);

            // Send an Identify message to the server, starting the connection process
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

        /// <summary>
        /// This is the body for the <see cref="HeartbeatLoopTask"/>, which sends
        /// heartbeat messages to the server.
        /// </summary>
        private void HeartbeatLoop()
        {
            while (!IsClosing)
            {
                try
                {
                    Payload heartbeatMessage = new Payload
                    {
                        OpCode = OpCode.Heartbeat
                    };
                    SendWebsocketMessage(heartbeatMessage);
                    Task.Delay(HeartbeatInterval, CancelToken).Wait();
                    System.Diagnostics.Debug.WriteLine($"Sent a voice heartbeat");
                }
                catch (TaskCanceledException)
                {

                }
            }
        }

        /// <summary>
        /// Sends a message to the voice server.
        /// </summary>
        /// <param name="Message">The message to send</param>
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
        /// This is the body of the <see cref="ReceiveLoopTask"/>, which receives
        /// messages from the server.
        /// </summary>
        /// <returns>The task running the method</returns>
        private async Task ReceiveWebsocketMessageLoop()
        {
            while (!IsClosing)
            {
                // Read bytes from the websocket over and over until the whole message has been processed
                WebSocketReceiveResult result = await Socket.ReceiveAsync(ReceiveBufferSegment, CancelToken);
                int receiveOffset = 0;
                int totalBytesReceived = result.Count;
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

                switch (result.MessageType)
                {
                    case WebSocketMessageType.Close:
                        System.Diagnostics.Debug.WriteLine("Voice connection got a close message.");
                        await Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancelToken);
                        return;

                    case WebSocketMessageType.Text:
                        string message = Encoding.UTF8.GetString(ReceiveBuffer, 0, totalBytesReceived);
                        System.Diagnostics.Debug.WriteLine($"Received a message from the Voice Server: {message}");
                        Payload payload = JsonConvert.DeserializeObject<Payload>(message);
                        Task parseTask = Task.Run(() => HandleMessage(payload));
                        break;

                    case WebSocketMessageType.Binary:
                        System.Diagnostics.Debug.WriteLine("Voice connection got a binary message?");
                        break;
                        //throw new Exception("Got a binary websocket message?");
                }
            }
        }

        /// <summary>
        /// Handles messages from the server, routing them to the appropriate functionality.
        /// </summary>
        /// <param name="Message">The message from the server</param>
        private void HandleMessage(Payload Message)
        {
            switch (Message.OpCode)
            {
                // If we're waiting for one of these, process it. Otherwise, ignore it.
                case OpCode.Ready:
                    if (ConnectionStep == VoiceConnectionStep.WaitForReady)
                    {
                        ReadyData readyData = ((JObject)Message.Data).ToObject<ReadyData>();
                        HandleReady(readyData);
                    }
                    break;

                // If we're waiting for one of these, process it. Otherwise, ignore it.
                case OpCode.SessionDescription:
                    if(ConnectionStep == VoiceConnectionStep.WaitForSessionDescription)
                    {
                        SessionDescription description = ((JObject)Message.Data).ToObject<SessionDescription>();
                        VoiceChannel.SecretKey = description.SecretKey;
                        ConnectWaiter.Set();
                    }
                    break;

                case OpCode.Heartbeat:
                    System.Diagnostics.Debug.WriteLine("Received a Voice Heartbeat ack.");
                    break;

                default:
                    System.Diagnostics.Debug.WriteLine($"VoiceClient got a message with code {Message.OpCode}, ignoring it.");
                    break;
            }
        }

        /// <summary>
        /// Handles an <see cref="OpCode.Ready"/> message from the server, sent in response
        /// to an <see cref="OpCode.Identify"/> message.
        /// </summary>
        /// <param name="Data">The payload from the Ready message</param>
        private void HandleReady(ReadyData Data)
        {
            SynchronizationSource = Data.SynchronizationSource;
            HeartbeatInterval = Data.HeartbeatInterval;
            VoiceChannel = new VoiceUdpConnection(VoiceServerHostname, Data.Port, SynchronizationSource);
            IPEndPoint localEndpoint = VoiceChannel.DiscoverAddress();

            // Once we get one of these, we can send a SelectProtocol message
            // to set up the UDP connection.
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

        /// <summary>
        /// Adds audio data to be played back over Discord.
        /// </summary>
        /// <param name="PlaybackData">The audio data to play. This must be in
        /// interleaved format, 48 kHz and stereo.</param>
        /// <param name="NumberOfSamplesToWrite">The number of samples per channel
        /// to play from the incoming data</param>
        public void AddPlaybackData(float[] PlaybackData, int NumberOfSamplesToWrite)
        {
            VoiceChannel.AddPlaybackData(PlaybackData, NumberOfSamplesToWrite);
        }

        /// <summary>
        /// Starts playing audio over Discord once it's connected.
        /// </summary>
        public void Start()
        {
            SetSpeaking data = new SetSpeaking
            {
                IsSpeaking = true,
                Delay = 0
            };
            Payload message = new Payload
            {
                OpCode = OpCode.Speaking,
                Data = data
            };
            SendWebsocketMessage(message);
            VoiceChannel.StartSending();
        }

        /// <summary>
        /// Stops playing audio over Discord.
        /// </summary>
        public void Stop()
        {
            SetSpeaking data = new SetSpeaking
            {
                IsSpeaking = false,
                Delay = 0
            };
            Payload message = new Payload
            {
                OpCode = OpCode.Speaking,
                Data = data
            };
            SendWebsocketMessage(message);
            VoiceChannel.StopSending();
        }

    }
}
