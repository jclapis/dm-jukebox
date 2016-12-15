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
    /// This is sent by the client to the server as a handshake and
    /// authentication message.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/gateway.
    /// </remarks>
    [JsonObject]
    internal class IdentifyData
    {
        /// <summary>
        /// The authentication token for identifying the current user
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// Client-specific properties
        /// </summary>
        [JsonProperty("properties")]
        public IdentifyDataProperties Properties { get; set; }

        /// <summary>
        /// True if this client supports compression of the initial
        /// ready packet, false if it doesn't
        /// </summary>
        [JsonProperty("compress")]
        public bool IsCompressionSupported { get; set; }

        /// <summary>
        /// This is a value between 50 and 250 which describes the 
        /// maximum number of guild members that can be present
        /// before the gateway truncates the list of offline members.
        /// </summary>
        [JsonProperty("large_threshold")]
        public int LargeThreshold { get; set; }

        /// <summary>
        /// This is an array of two integers (shard_id and num_shards)
        /// used for guild sharding in bots.
        /// </summary>
        [JsonProperty("shard")]
        public int[] ShardInfo { get; set; }
    }
}
