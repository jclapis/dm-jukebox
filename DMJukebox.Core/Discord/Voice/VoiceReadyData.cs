using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMJukebox.Discord
{
    [JsonObject]
    internal class VoiceReadyData
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
