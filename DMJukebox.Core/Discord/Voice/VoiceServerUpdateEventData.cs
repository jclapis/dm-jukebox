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
    /// This event is sent when the voice server for a guild is changed.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/voice-connections.
    /// </remarks>
    [JsonObject]
    internal class VoiceServerUpdateEventData
    {
        /// <summary>
        /// The unique authentication token for this voice connection session.
        /// </summary>
        [JsonProperty("token")]
        public string VoiceConnectionToken { get; set; }

        /// <summary>
        /// The ID of the guild that this server update is for
        /// </summary>
        [JsonProperty("guild_id")]
        public string GuildID { get; set; }

        /// <summary>
        /// The hostname / endpoint of the voice server
        /// </summary>
        [JsonProperty("endpoint")]
        public string VoiceServerHostname { get; set; }
    }
}
