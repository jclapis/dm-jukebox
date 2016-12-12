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
    /// This is the data structure for a Hello message. It gets sent to us by the Discord
    /// server as soon as we connect.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/gateway.
    /// </remarks>
    [JsonObject]
    internal class HelloData
    {
        /// <summary>
        /// This is the interval (in milliseconds) that we should wait between sending
        /// heartbeat messages.
        /// </summary>
        [JsonProperty("heartbeat_interval")]
        public int HeartbeatInterval { get; set; }

        /// <summary>
        /// This is just some debugging info, it's a list of the servers that we're
        /// currently connected to but you can ignore it.
        /// </summary>
        [JsonProperty("_trace")]
        public string[] Trace { get; set; }
    }
}
