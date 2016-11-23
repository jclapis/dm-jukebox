namespace DMJukebox.Interop
{
    internal enum OpusErrorCode
    {
        OPUS_OK = 0,
        OPUS_BAD_ARG = -1,
        OPUS_BUFFER_TOO_SMALL = -2,
        OPUS_INTERNAL_ERROR = -3,
        OPUS_INVALID_PACKET = -4,
        OPUS_UNIMPLEMENTED = -5,
        OPUS_INVALID_STATE = -6,
        OPUS_ALLOC_FAIL = -7
    }
}
