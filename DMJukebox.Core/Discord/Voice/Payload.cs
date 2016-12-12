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
    /// This structure is used to transfer information back and forth to the
    /// Discord voice server.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/voice-connections.
    /// </remarks>
    [JsonObject]
    internal class Payload
    {
        /// <summary>
        /// This is the OP Code for the payload (what kind of message
        /// it is)
        /// </summary>
        [JsonProperty("op")]
        public OpCode OpCode { get; set; }

        /// <summary>
        /// This is the message-specific data structure that holds the
        /// important info for each message.
        /// </summary>
        [JsonProperty("d")]
        public object Data { get; set; }
    }
}