/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */
 
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
