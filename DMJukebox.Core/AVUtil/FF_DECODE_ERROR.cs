using System;

namespace DMJukebox.Interop
{
    [Flags]
    internal enum FF_DECODE_ERROR
    {
        FF_DECODE_ERROR_INVALID_BITSTREAM = 1,
        FF_DECODE_ERROR_MISSING_REFERENCE = 2
    }
}
