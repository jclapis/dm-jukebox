using Newtonsoft.Json;

namespace DMJukebox.Discord.Voice
{
    [JsonObject]
    internal class SelectProtocol
    {
        [JsonProperty("protocol")]
        public string Protocol { get; set; }

        [JsonProperty("data")]
        public SelectProtocolData Data { get; set; }
    }
}
