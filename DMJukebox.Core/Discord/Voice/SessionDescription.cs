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
    /// This message is sent by the server as the final step
    /// in the voice connection handshake. It describes the
    /// final UDP voice connection parameters.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/voice-connections.
    /// </remarks>
    [JsonObject]
    internal class SessionDescription
    {
        /// <summary>
        /// The secret key to use while encrypting voice data
        /// </summary>
        [JsonProperty("secret_key")]
        public byte[] SecretKey { get; set; }

        /// <summary>
        /// The encryption mode the channel uses
        /// </summary>
        [JsonProperty("mode")]
        public string Mode { get; set; }
    }
}
