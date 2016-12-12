/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */
 
namespace DMJukebox.Discord.Voice
{
    /// <summary>
    /// OpCode defines the type of the message payload that was
    /// sent to / received from the Discord voice server.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/voice-connections.
    /// </remarks>
    internal enum OpCode
    {
        /// <summary>
        /// This message is sent from the client to the server to
        /// initiate the handshake procedure.
        /// </summary>
        Identify,

        /// <summary>
        /// This message is sent from the client to the server at
        /// the end of the handshake procedure, once we are ready
        /// to connect to the UDP socket.
        /// </summary>
        SelectProtocol,

        /// <summary>
        /// This message is a response from the server once it
        /// receives and processes our Identify message.
        /// </summary>
        Ready,

        /// <summary>
        /// This is sent both ways. We send it to the server as
        /// a heartbeat to tell it that we're still alive, and
        /// it responds with a heartbeat back. This acts similarly
        /// to <see cref="Gateway.OpCode.HeartbeatAck"/> when it
        /// is sent by the server.
        /// </summary>
        Heartbeat,

        /// <summary>
        /// This message is sent by the server at the end of the
        /// voice handshake procedure, to signal that the handshake
        /// worked and was accepted.
        /// </summary>
        SessionDescription,

        /// <summary>
        /// This message is sent both ways when someone's speaking
        /// state changes from speaking to not-speaking and vice
        /// versa.
        /// </summary>
        Speaking
    }
}
