/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

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
