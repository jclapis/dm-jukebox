using Newtonsoft.Json;

namespace DMJukebox
{
    /// <summary>
    /// Configuration stores the persistent configuration settings for the 
    /// Jukebox core.
    /// </summary>
    [JsonObject]
    public class Configuration
    {
        /// <summary>
        /// The settings for connecting to Discord
        /// </summary>
        [JsonProperty]
        public DiscordSettings DiscordSettings { get; set; }
    }
}
