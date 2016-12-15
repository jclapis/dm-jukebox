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
    /// This message is sent by the server as the final step
    /// in the voice connection handshake. It describes the
    /// final UDP voice connection parameters.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/voice-connections.
    /// </remarks>
    [JsonObject]
    internal class SessionDescription
    {
        /// <summary>
        /// The secret key to use while encrypting voice data
        /// </summary>
        [JsonProperty("secret_key")]
        public byte[] SecretKey { get; set; }

        /// <summary>
        /// The encryption mode the channel uses
        /// </summary>
        [JsonProperty("mode")]
        public string Mode { get; set; }
    }
}
