/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

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
