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

namespace DMJukebox.Discord
{
    /// <summary>
    /// This is a collection of miscellaneous properties that describe
    /// the Discord client.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/gateway.
    /// </remarks>
    [JsonObject]
    internal class IdentifyDataProperties
    {
        /// <summary>
        /// The Operating System that the client is running on
        /// </summary>
        [JsonProperty("$os")]
        public string OperatingSystem { get; set; }

        /// <summary>
        /// Name of the Discord client application
        /// </summary>
        [JsonProperty("$device")]
        public string Device { get; set; }
    }
}
