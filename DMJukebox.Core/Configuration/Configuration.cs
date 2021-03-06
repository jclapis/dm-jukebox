﻿/* ========================================================================
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
using System.Collections.Generic;

namespace DMJukebox
{
    /// <summary>
    /// Configuration stores the persistent configuration settings for the 
    /// Jukebox core.
    /// </summary>
    [JsonObject]
    public class Configuration
    {
        /// <summary>
        /// The settings for connecting to Discord
        /// </summary>
        [JsonProperty]
        public DiscordSettings DiscordSettings { get; set; }

        /// <summary>
        /// The serialized playlists
        /// </summary>
        [JsonProperty]
        internal List<Playlist> Playlists { get; set; }
    }
}
