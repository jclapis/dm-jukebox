namespace DiscordJukebox.Interop
{
    /// <summary>
    /// Audio sample formats
    ///
    /// - The data described by the sample format is always in native-endian order.
    ///   Sample values can be expressed by native C types, hence the lack of a signed
    ///   24-bit sample format even though it is a common raw audio data format.
    ///
    /// - The floating-point formats are based on full volume being in the range
    ///   [-1.0, 1.0]. Any values outside this range are beyond full volume level.
    ///
    /// - The data layout as used in av_samples_fill_arrays() and elsewhere in FFmpeg
    ///   (such as AVFrame in libavcodec) is as follows:
    ///
    /// For planar sample formats, each audio channel is in a separate data plane,
    /// and linesize is the buffer size, in bytes, for a single plane.All data
    /// planes must be the same size. For packed sample formats, only the first data
    /// plane is used, and samples for each channel are interleaved. In this case,
    /// linesize is the buffer size, in bytes, for the 1 plane.
    /// </summary>
    internal enum AVSampleFormat
    {
        AV_SAMPLE_FMT_NONE = -1,

        /// <summary>
        /// unsigned 8 bits
        /// </summary>
        AV_SAMPLE_FMT_U8,

        /// <summary>
        /// signed 16 bits
        /// </summary>
        AV_SAMPLE_FMT_S16,

        /// <summary>
        /// signed 32 bits
        /// </summary>
        AV_SAMPLE_FMT_S32,

        /// <summary>
        /// float
        /// </summary>
        AV_SAMPLE_FMT_FLT,

        /// <summary>
        /// double
        /// </summary>
        AV_SAMPLE_FMT_DBL,

        /// <summary>
        /// unsigned 8 bits, planar
        /// </summary>
        AV_SAMPLE_FMT_U8P,

        /// <summary>
        /// signed 16 bits, planar
        /// </summary>
        AV_SAMPLE_FMT_S16P,

        /// <summary>
        /// signed 32 bits, planar
        /// </summary>
        AV_SAMPLE_FMT_S32P,

        /// <summary>
        /// float, planar
        /// </summary>
        AV_SAMPLE_FMT_FLTP,

        /// <summary>
        /// double, planar
        /// </summary>
        AV_SAMPLE_FMT_DBLP,

        /// <summary>
        /// Number of sample formats. DO NOT USE if linking dynamically
        /// </summary>
        AV_SAMPLE_FMT_NB
    }
}
