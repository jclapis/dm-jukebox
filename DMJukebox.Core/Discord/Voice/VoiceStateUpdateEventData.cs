using Newtonsoft.Json;

namespace DMJukebox.Discord
{
    /// <summary>
    /// This is a server-sent event (not an OP Code-based message) that gets
    /// sent to us when someone joins, moves between or leaves a voice channel.
    /// </summary>
    [JsonObject]
    internal class VoiceStateUpdateEventData
    {
        /// <summary>
        /// The ID of the user that had a voice state change
        /// </summary>
        [JsonProperty("user_id")]
        public string UserID { get; set; }

        /// <summary>
        /// The ID of the voice session this event is for
        /// </summary>
        [JsonProperty("session_id")]
        public string SessionID { get; set; }
    }
}
