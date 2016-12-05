using Newtonsoft.Json;

namespace DMJukebox.Discord.Voice
{
    [JsonObject]
    internal class SessionDescription
    {
        [JsonProperty("secret_key")]
        public byte[] SecretKey { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }
    }
}
