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
    /// This structure is used to notify the server when the
    /// client joins or leaves a voice channel.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/voice-connections.
    /// </remarks>
    [JsonObject]
    internal class VoiceStateUpdateData
    {
        /// <summary>
        /// The ID of the guild that the channel is in
        /// </summary>
        [JsonProperty("guild_id")]
        public string GuildID { get; set; }

        /// <summary>
        /// The ID of the voice channel to connect to (set this
        /// to null in order to disconnect)
        /// </summary>
        [JsonProperty("channel_id")]
        public string ChannelID { get; set; }

        /// <summary>
        /// True if the client has muted itself, false if it
        /// can speak
        /// </summary>
        [JsonProperty("self_mute")]
        public bool IsMuted { get; set; }

        /// <summary>
        /// True if the client has deafened itself (can't
        /// receive audio), false if it can hear sound
        /// </summary>
        [JsonProperty("self_deaf")]
        public bool IsDeafened { get; set; }
    }
}
