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

namespace DMJukebox.Discord.Voice
{
    /// <summary>
    /// This message is returned by the server after it accepts
    /// an <see cref="OpCode.Identify"/> message.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/voice-connections.
    /// </remarks>
    [JsonObject]
    internal class ReadyData
    {
        /// <summary>
        /// This is the synchronization source, which is
        /// used in audio data packets as part of the
        /// encryption.
        /// </summary>
        [JsonProperty("ssrc")]
        public uint SynchronizationSource { get; set; }

        /// <summary>
        /// This is the UDP port to connect to in order to
        /// send audio data to the voice server
        /// </summary>
        [JsonProperty("port")]
        public int Port { get; set; }

        /// <summary>
        /// This is a list of encryption methods that the
        /// voice server supports.
        /// </summary>
        [JsonProperty("modes")]
        public string[] EncryptionMethods { get; set; }

        /// <summary>
        /// This is the interval, in milliseconds, that
        /// the server expects the voice channel to send
        /// heartbeat messages.
        /// </summary>
        [JsonProperty("heartbeat_interval")]
        public int HeartbeatInterval { get; set; }
    }
}
