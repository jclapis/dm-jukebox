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

namespace DMJukebox
{
    /// <summary>
    /// This object stores the configuration for the Discord connection.
    /// </summary>
    [JsonObject]
    public class DiscordSettings
    {
        /// <summary>
        /// This is the Token of the Discord bot account to connect with
        /// </summary>
        [JsonProperty]
        public string BotTokenID { get; set; }

        /// <summary>
        /// This is the ID of the Discord Guild that the voice channel
        /// belongs to
        /// </summary>
        [JsonProperty]
        public string GuildID { get; set; }

        /// <summary>
        /// This is the ID of the voice channel to connect to for music
        /// playback
        /// </summary>
        [JsonProperty]
        public string ChannelID { get; set; }
    }
}
