namespace DiscordJukebox.Interop
{
    /// <summary>
    /// @ingroup lavc_decoding
    /// We leave some space between them for extensions (drop some
    /// keyframes for intra-only or drop just some bidir frames).
    /// </summary>
    internal enum AVDiscard
    {
        /// <summary>
        /// discard nothing
        /// </summary>
        AVDISCARD_NONE = -16,

        /// <summary>
        /// discard useless packets like 0 size packets in avi
        /// </summary>
        AVDISCARD_DEFAULT = 0,

        /// <summary>
        /// discard all non reference
        /// </summary>
        AVDISCARD_NONREF = 8,

        /// <summary>
        /// discard all bidirectional frames
        /// </summary>
        AVDISCARD_BIDIR = 16,

        /// <summary>
        /// discard all non intra frames
        /// </summary>
        AVDISCARD_NONINTRA = 24,

        /// <summary>
        /// discard all frames except keyframes
        /// </summary>
        AVDISCARD_NONKEY = 32,

        /// <summary>
        /// discard all
        /// </summary>
        AVDISCARD_ALL = 48
    }
}
