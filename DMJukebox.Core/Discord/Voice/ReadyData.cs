using Newtonsoft.Json;

namespace DMJukebox.Discord.Voice
{
    [JsonObject]
    internal class ReadyData
    {
        [JsonProperty("ssrc")]
        public uint SynchronizationSource { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("modes")]
        public string[] EncryptionMethods { get; set; }

        [JsonProperty("heartbeat_interval")]
        public int HeartbeatInterval { get; set; }
    }
}
