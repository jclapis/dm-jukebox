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
    /// This is the data structure for a Hello message. It gets sent to us by the Discord
    /// server as soon as we connect.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/gateway.
    /// </remarks>
    [JsonObject]
    internal class HelloData
    {
        /// <summary>
        /// This is the interval (in milliseconds) that we should wait between sending
        /// heartbeat messages.
        /// </summary>
        [JsonProperty("heartbeat_interval")]
        public int HeartbeatInterval { get; set; }

        /// <summary>
        /// This is just some debugging info, it's a list of the servers that we're
        /// currently connected to but you can ignore it.
        /// </summary>
        [JsonProperty("_trace")]
        public string[] Trace { get; set; }
    }
}
