/* 
 * This file contains a C# implementation of the FF_DECODE_ERROR enum
 * as defined in frame.h of the libavutil project, for interop use.
 * It isn't technically an enum in ffmpeg, just a bunch of macros.
 * 
 * For more information, please see the documentation at
 * https://www.ffmpeg.org/doxygen/trunk/index.html or the source code at
 * https://github.com/FFmpeg/FFmpeg.
 * 
 * Copyright (c) 2016 Joe Clapis.
 */

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
