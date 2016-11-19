/* 
 * This file contains a C# implementation of the AVSEEK_FLAG enum
 * as defined in avformat.h of the libavformat project, for interop use.
 * It isn't technically an enum in ffmpeg, just a bunch of macros.
 * 
 * The documentation and comments have been largely copied from those headers and
 * are not my own work - they are the work of the contributors to ffmpeg.
 * Credit goes to them. I may have modified them in places where it made sense
 * to help document the C# bindings.
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
        /// <summary>
        /// seek backward
        /// </summary>
        AVSEEK_FLAG_BACKWARD = 1,

        /// <summary>
        /// seeking based on position in bytes
        /// </summary>
        AVSEEK_FLAG_BYTE = 2,

        /// <summary>
        /// seek to any frame, even non-keyframes
        /// </summary>
        AVSEEK_FLAG_ANY = 4,

        /// <summary>
        /// seeking based on frame number
        /// </summary>
        AVSEEK_FLAG_FRAME = 8
    }
}
