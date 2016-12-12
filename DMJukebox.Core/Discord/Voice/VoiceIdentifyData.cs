/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

using Newtonsoft.Json;

namespace DMJukebox.Discord.Voice
{
    /// <summary>
    /// This payload is used to establish a connection to a Discord voice server.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/voice-connections.
    /// </remarks>
    [JsonObject]
    internal class VoiceIdentifyData
    {
        /// <summary>
        /// This is actually the ID of the guild that holds the voice channel we're
        /// trying to connect to.
        /// </summary>
        [JsonProperty("server_id")]
        public string ServerID { get; set; }

        /// <summary>
        /// This is the ID of the user that's trying to connect to the voice channel.
        /// </summary>
        [JsonProperty("user_id")]
        public string UserID { get; set; }

        /// <summary>
        /// This is the unique voice session ID returned from the VoiceServerUpdate
        /// event after a VoiceStateUpdate packet is sent to the server.
        /// </summary>
        [JsonProperty("session_id")]
        public string SessionID { get; set; }

        /// <summary>
        /// This is the unique authorization token for this connect, returned by the
        /// VoiceStateUpdate event after a VoiceStateUpdate packet (they're two different
        /// things, I know...) is sent to the server.
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
