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
