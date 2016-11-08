namespace DiscordJukebox.Interop
{
    /// <summary>
    /// MPEG vs JPEG YUV range.
    /// </summary>
    internal enum AVColorRange
    {
        AVCOL_RANGE_UNSPECIFIED = 0,

        /// <summary>
        /// the normal 219*2^(n-8) "MPEG" YUV ranges
        /// </summary>
        AVCOL_RANGE_MPEG = 1,

        /// <summary>
        /// the normal 2^n-1 "JPEG" YUV ranges
        /// </summary>
        AVCOL_RANGE_JPEG = 2,

        /// <summary>
        /// Not part of ABI
        /// </summary>
        AVCOL_RANGE_NB,
    }
}
