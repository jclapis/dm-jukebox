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
