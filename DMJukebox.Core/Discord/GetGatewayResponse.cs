using Newtonsoft.Json;

namespace DMJukebox.Discord
{
    /// <summary>
    /// This is the response from a GET Gateway URL request to the
    /// Discord REST service.
    /// </summary>
    [JsonObject]
    internal class GetGatewayResponse
    {
        /// <summary>
        /// This is the URL of the gateway websocket service.
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }
}
