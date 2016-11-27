using Newtonsoft.Json;

namespace DMJukebox.Discord
{
    /// <summary>
    /// This structure is used to notify the server when the
    /// client joins or leaves a voice channel.
    /// </summary>
    [JsonObject]
    internal class VoiceStateUpdateData
    {
        /// <summary>
        /// The ID of the guild that the channel is in
        /// </summary>
        [JsonProperty("guild_id")]
        public string GuildID { get; set; }

        /// <summary>
        /// The ID of the voice channel to connect to (set this
        /// to null in order to disconnect)
        /// </summary>
        [JsonProperty("channel_id")]
        public string ChannelID { get; set; }

        /// <summary>
        /// True if the client has muted itself, false if it
        /// can speak
        /// </summary>
        [JsonProperty("self_mute")]
        public bool IsMuted { get; set; }

        /// <summary>
        /// True if the client has deafened itself (can't
        /// receive audio), false if it can hear sound
        /// </summary>
        [JsonProperty("self_deaf")]
        public bool IsDeafened { get; set; }
    }
}
