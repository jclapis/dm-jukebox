using Newtonsoft.Json;

namespace DMJukebox.Discord.Voice
{
    [JsonObject]
    internal class SetSpeaking
    {
        [JsonProperty("speaking")]
        public bool IsSpeaking { get; set; }

        [JsonProperty("delay")]
        public int Delay { get; set; }
    }
}
