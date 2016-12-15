﻿/* ========================================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * ====================================================================== */

using Newtonsoft.Json;

namespace DMJukebox.Discord.Gateway
{
    /// <summary>
    /// This structure is used to transfer information back and forth to the
    /// Discord server.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/gateway.
    /// </remarks>
    [JsonObject]
    internal class Payload
    {
        /// <summary>
        /// This is the event name for the payload. It only gets used on 
        /// <see cref="OpCode.Dispatch"/> messages.
        /// </summary>
        [JsonProperty("t", Required = Required.AllowNull)]
        public string EventName { get; set; }
        
        /// <summary>
        /// This is the sequence number, which is used by the heartbeat
        /// and resume session systems. This only gets used on 
        /// <see cref="OpCode.Dispatch"/> messages.
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
