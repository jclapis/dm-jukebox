/* 
 * This file contains a C# implementation of the SLICE_FLAG enum
 * as defined in avcodec.h of the libavcodec project, for interop use.
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
    internal enum SLICE_FLAG
    {
        SLICE_FLAG_CODED_ORDER = 0x1,
        SLICE_FLAG_ALLOW_FIELD = 0x2,
        SLICE_FLAG_ALLOW_PLANE = 0x4
    }
}
