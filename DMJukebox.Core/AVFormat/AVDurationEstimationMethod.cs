namespace DMJukebox.Interop
{
    /// <summary>
    /// The duration of a video can be estimated through various ways, and this enum can be used
    /// to know how the duration was estimated.
    /// </summary>
    internal enum AVDurationEstimationMethod
    {
        /// <summary>
        /// Duration accurately estimated from PTSes
        /// </summary>
        AVFMT_DURATION_FROM_PTS,

        /// <summary>
        /// Duration estimated from a stream with a known duration
        /// </summary>
        AVFMT_DURATION_FROM_STREAM,

        /// <summary>
        /// Duration estimated from bitrate (less accurate)
        /// </summary>
        AVFMT_DURATION_FROM_BITRATE
    }

}
