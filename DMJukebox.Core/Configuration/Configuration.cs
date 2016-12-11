/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */
 
 using Newtonsoft.Json;

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
    }
}
