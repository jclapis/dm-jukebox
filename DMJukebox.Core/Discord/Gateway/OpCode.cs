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

namespace DMJukebox.Discord.Gateway
{
    /// <summary>
    /// OpCode defines the type of the message payload
    /// that was sent to / received from the Discord gateway.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/gateway.
    /// </remarks>
    internal enum OpCode
    {
        /// <summary>
        /// This message holds a gateway event, not a
        /// discrete well-defined message payload
        /// </summary>
        Dispatch,

        /// <summary>
        /// This is a heartbeat message, letting the
        /// server know that we're still alive
        /// </summary>
        Heartbeat,

        /// <summary>
        /// This is an identification message for
        /// establishing a handshake with the server
        /// </summary>
        Identify,

        /// <summary>
        /// Informs the server that the client's status
        /// has changed
        /// </summary>
        StatusUpdate,

        /// <summary>
        /// Informs the server that the client has joined,
        /// moved, or left a voice channel
        /// </summary>
        VoiceStateUpdate,

        /// <summary>
        /// This is used to ping the voice server to check
        /// if it's still alive, maybe? Not sure what it
        /// does, really.
        /// </summary>
        VoiceServerPing,

        /// <summary>
        /// This is used to re-open a previously closed
        /// connection with the server
        /// </summary>
        Resume,

        /// <summary>
        /// This message is sent by the server to tell
        /// clients that they need to reconnect to the
        /// gateway.
        /// </summary>
        Reconnect,

        /// <summary>
        /// Sent by the client to the server to request
        /// a list of guild members
        /// </summary>
        RequestGuildMembers,

        /// <summary>
        /// The server sends this message when the client's
        /// session ID is invalid
        /// </summary>
        InvalidSession,

        /// <summary>
        /// Sent by the server after establishing a
        /// connection to the gateway, as part of the
        /// handshake
        /// </summary>
        Hello,

        /// <summary>
        /// A response from the server after it
        /// successfully receives a heartbeat
        /// </summary>
        HeartbeatAck
    }
}
