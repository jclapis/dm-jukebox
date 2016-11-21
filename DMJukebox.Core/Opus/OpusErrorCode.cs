namespace DMJukebox.Interop
{
    /// <summary>
    /// Error codes
    /// </summary>
    internal enum OpusErrorCode
    {
        /// <summary>
        /// No error
        /// </summary>
        OPUS_OK = 0,

        /// <summary>
        /// One or more invalid/out of range arguments
        /// </summary>
        OPUS_BAD_ARG = -1,

        /// <summary>
        /// Not enough bytes allocated in the buffer
        /// </summary>
        OPUS_BUFFER_TOO_SMALL = -2,

        /// <summary>
        /// An internal error was detected
        /// </summary>
        OPUS_INTERNAL_ERROR = -3,

        /// <summary>
        /// The compressed data passed is corrupted
        /// </summary>
        OPUS_INVALID_PACKET = -4,

        /// <summary>
        /// Invalid/unsupported request number
        /// </summary>
        OPUS_UNIMPLEMENTED = -5,

        /// <summary>
        /// An encoder or decoder structure is invalid or already freed
        /// </summary>
        OPUS_INVALID_STATE = -6,

        /// <summary>
        /// Memory allocation has failed
        /// </summary>
        OPUS_ALLOC_FAIL = -7
    }
}
