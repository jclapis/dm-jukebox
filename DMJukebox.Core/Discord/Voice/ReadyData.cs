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
    /// This message is returned by the server after it accepts
    /// an <see cref="OpCode.Identify"/> message.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/voice-connections.
    /// </remarks>
    [JsonObject]
    internal class ReadyData
    {
        /// <summary>
        /// This is the synchronization source, which is
        /// used in audio data packets as part of the
        /// encryption.
        /// </summary>
        [JsonProperty("ssrc")]
        public uint SynchronizationSource { get; set; }

        /// <summary>
        /// This is the UDP port to connect to in order to
        /// send audio data to the voice server
        /// </summary>
        [JsonProperty("port")]
        public int Port { get; set; }

        /// <summary>
        /// This is a list of encryption methods that the
        /// voice server supports.
        /// </summary>
        [JsonProperty("modes")]
        public string[] EncryptionMethods { get; set; }

        /// <summary>
        /// This is the interval, in milliseconds, that
        /// the server expects the voice channel to send
        /// heartbeat messages.
        /// </summary>
        [JsonProperty("heartbeat_interval")]
        public int HeartbeatInterval { get; set; }
    }
}
