/* 
 * This file contains a C# implementation of the AVSEEK_FLAG enum
 * as defined in avformat.h of the libavformat project, for interop use.
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
    internal enum AVSEEK_FLAG
    {
        AVSEEK_FLAG_BACKWARD = 1,
        AVSEEK_FLAG_BYTE = 2,
        AVSEEK_FLAG_ANY = 4,
        AVSEEK_FLAG_FRAME = 8
    }
}
