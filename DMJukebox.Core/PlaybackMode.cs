/*
 * Copyright (c) 2016 Joe Clapis.
 */

namespace DMJukebox
{
    /// <summary>
    /// PlaybackMode is used to set where output audio will be sent.
    /// Only one of these modes can be active at a time.
    /// </summary>
    public enum PlaybackMode
    {
        /// <summary>
        /// Play sound to the local machine's speakers
        /// </summary>
        LocalSpeakers,

        /// <summary>
        /// Send sound over the network to a Discord server
        /// </summary>
        Discord
    }
}
