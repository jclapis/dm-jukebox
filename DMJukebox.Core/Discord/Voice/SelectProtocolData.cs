using Newtonsoft.Json;

namespace DMJukebox.Discord.Voice
{
    [JsonObject]
    internal class SelectProtocolData
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }
    }
}
