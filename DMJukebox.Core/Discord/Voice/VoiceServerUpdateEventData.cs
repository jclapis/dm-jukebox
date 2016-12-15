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

namespace DMJukebox.Discord
{
    /// <summary>
    /// This event is sent when the voice server for a guild is changed.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/voice-connections.
    /// </remarks>
    [JsonObject]
    internal class VoiceServerUpdateEventData
    {
        /// <summary>
        /// The unique authentication token for this voice connection session.
        /// </summary>
        [JsonProperty("token")]
        public string VoiceConnectionToken { get; set; }

        /// <summary>
        /// The ID of the guild that this server update is for
        /// </summary>
        [JsonProperty("guild_id")]
        public string GuildID { get; set; }

        /// <summary>
        /// The hostname / endpoint of the voice server
        /// </summary>
        [JsonProperty("endpoint")]
        public string VoiceServerHostname { get; set; }
    }
}
