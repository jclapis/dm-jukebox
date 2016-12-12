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
    /// This message is sent to the server after receiving
    /// an <see cref="OpCode.Ready"/> message to set the
    /// protocol for the voice connection.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/voice-connections.
    /// </remarks>
    [JsonObject]
    internal class SelectProtocol
    {
        /// <summary>
        /// The protocol we want to use to send audio data
        /// to the voice server
        /// </summary>
        [JsonProperty("protocol")]
        public string Protocol { get; set; }

        /// <summary>
        /// Data about the client's UDP endpoint
        /// that will receive incoming voice data
        /// </summary>
        [JsonProperty("data")]
        public SelectProtocolData Data { get; set; }
    }
}
