/* 
 * This file contains a C# implementation of the AV_PKT_FLAG enum
 * as defined in avcodec.h of the libavcodec project, for interop use.
 * It isn't technically an enum in ffmpeg, just a bunch of macros.
 * 
 * For more information, please see the documentation at
 * https://www.ffmpeg.org/doxygen/trunk/index.html or the source code at
 * https://github.com/FFmpeg/FFmpeg.
 * 
 * Copyright (c) 2016 Joe Clapis.
 */

namespace DMJukebox.Interop
{
    internal enum AV_PKT_FLAG
    {
        AV_PKT_FLAG_KEY = 0x0001,
        AV_PKT_FLAG_CORRUPT = 0x0002
    }
}
