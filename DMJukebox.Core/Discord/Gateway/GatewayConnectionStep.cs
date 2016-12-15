/* ========================================================================
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

namespace DMJukebox.Discord.Gateway
{
    /// <summary>
    /// This is a list of steps involved in connecting to
    /// the Discord gateway.
    /// </summary>
    internal enum GatewayConnectionStep
    {
        /// <summary>
        /// The system isn't currently connected.
        /// </summary>
        Disconnected,

        /// <summary>
        /// Waiting for a Hello message from the gateway
        /// </summary>
        WaitingForHello,

        /// <summary>
        /// Waiting for a Ready message from the gateway
        /// </summary>
        WaitingForReady,

        /// <summary>
        /// Waiting for the gateway to provide info
        /// about the voice server to connect to
        /// </summary>
        WaitingForVoiceServerInfo,

        /// <summary>
        /// Connected to the gateway!
        /// </summary>
        Connected
    }
}
