using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMJukebox.Discord
{
    [JsonObject]
    internal class ReadyEventData
    {
        /// <summary>
        /// The protocol version of the connected Gateway
        /// </summary>
        [JsonProperty("v")]
        public int ProtocolVersion { get; set; }

        /// <summary>
        /// This is supposed to be a collection of client settings
        /// for this user, but bot accounts don't have any so we can
        /// ignore it.
        /// </summary>
        [JsonProperty("user_settings")]
        public object UserSettings { get; set; }

        /// <summary>
        /// Information about the bot account.
        /// </summary>
        [JsonProperty("user")]
        public UserInfo UserInfo { get; set; }

        /// <summary>
        /// Information about the user's shard setup if this is
        /// a bot account.
        /// </summary>
        [JsonProperty("shard")]
        public int[] ShardInfo { get; set; }

        /// <summary>
        /// The unique ID for this Discord session
        /// </summary>
        [JsonProperty("session_id")]
        public string SessionID { get; set; }

        /// <summary>
        /// The user's list of friends (this is empty for bots)
        /// </summary>
        [JsonProperty("relationships")]
        public object[] Relationships { get; set; }

        /// <summary>
        /// A collection of private conversation channels
        /// (this is probably empty for bots).
        /// </summary>
        [JsonProperty("private_channels")]
        public object[] PrivateChannels { get; set; }

        /// <summary>
        /// A list of friends' prensences (this is empty for
        /// bot accounts)
        /// </summary>
        [JsonProperty("presences")]
        public object[] Presences { get; set; }

        /// <summary>
        /// A collection of offline / uninitialized guild
        /// information descriptors
        /// </summary>
        [JsonProperty("guilds")]
        public object[] UnavailableGuilds { get; set; }

        /// <summary>
        /// The list of servers currently connected to for
        /// debugging purposes
        /// </summary>
        [JsonProperty("_trace")]
        public string[] Trace { get; set; }
    }
}
