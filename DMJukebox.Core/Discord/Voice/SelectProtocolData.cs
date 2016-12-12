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
    /// This is the data object used in
    /// <see cref="OpCode.SelectProtocol"/>
    /// messages sent to the server.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/voice-connections.
    /// </remarks>
    [JsonObject]
    internal class SelectProtocolData
    {
        /// <summary>
        /// This is our local IP address (for
        /// the client)
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }

        /// <summary>
        /// This is the local UDP port that
        /// the server should accept the voice
        /// connection from.
        /// </summary>
        [JsonProperty("port")]
        public int Port { get; set; }

        /// <summary>
        /// This is the encryption mode that will
        /// be used in the UDP voice connection
        /// </summary>
        [JsonProperty("mode")]
        public string Mode { get; set; }
    }
}
