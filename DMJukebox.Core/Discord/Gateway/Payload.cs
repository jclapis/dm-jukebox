using Newtonsoft.Json;

namespace DMJukebox.Discord.Gateway
{
    /// <summary>
    /// This structure is used to transfer information back and forth to the
    /// Discord server.
    /// </summary>
    [JsonObject]
    internal class Payload
    {
        /// <summary>
        /// This is the event name for the payload. It only gets used on 
        /// Dispatch messages (Code 0).
        /// </summary>
        [JsonProperty("t", Required = Required.AllowNull)]
        public string EventName { get; set; }
        
        /// <summary>
        /// This is the sequence number, which is used by the heartbeat
        /// and resume session systems. This only gets used on Dispatch
        /// messages (Code 0).
        /// </summary>
        [JsonProperty("s", Required = Required.AllowNull)]
        public int? SequenceNumber { get; set; }

        /// <summary>
        /// This is the OP Code for the payload (what kind of message
        /// it is)
        /// </summary>
        [JsonProperty("op")]
        public OpCode OpCode { get; set; }

        /// <summary>
        /// This is the message-specific data structure that holds the
        /// important info for each message.
        /// </summary>
        [JsonProperty("d")]
        public object Data { get; set; }
    }
}
