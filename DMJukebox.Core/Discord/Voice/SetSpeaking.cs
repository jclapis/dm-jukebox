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
    /// This message is sent by the client to inform the server that the
    /// bot's speaking flag has changed.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/voice-connections.
    /// </remarks>
    [JsonObject]
    internal class SetSpeaking
    {
        /// <summary>
        /// True if the bot is speaking, false if it is not.
        /// </summary>
        [JsonProperty("speaking")]
        public bool IsSpeaking { get; set; }

        /// <summary>
        /// Not sure what this is, I just set it to zero.
        /// </summary>
        [JsonProperty("delay")]
        public int Delay { get; set; }
    }
}
