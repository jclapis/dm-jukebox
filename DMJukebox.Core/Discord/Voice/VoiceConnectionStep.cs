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

namespace DMJukebox.Discord.Voice
{
    /// <summary>
    /// This describes the steps involved in connecting
    /// to the voice server.
    /// </summary>
    internal enum VoiceConnectionStep
    {
        /// <summary>
        /// Not currently connected
        /// </summary>
        Disconnected,

        /// <summary>
        /// The connection process is waiting for the
        /// server to send an <see cref="OpCode.Ready"/>
        /// message.
        /// </summary>
        WaitForReady,

        /// <summary>
        /// The connection process is waiting for the
        /// server to send an <see cref="OpCode.SessionDescription"/>
        /// message.
        /// </summary>
        WaitForSessionDescription
    }
}
