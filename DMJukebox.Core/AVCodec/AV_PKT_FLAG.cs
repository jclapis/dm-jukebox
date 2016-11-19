/* 
 * This file contains a C# implementation of the AV_PKT_FLAG enum
 * as defined in avcodec.h of the libavcodec project, for interop use.
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

namespace DMJukebox.Interop
{
    internal enum AV_PKT_FLAG
    {
        /// <summary>
        /// The packet contains a keyframe
        /// </summary>
        AV_PKT_FLAG_KEY = 0x0001,

        /// <summary>
        /// The packet content is corrupted
        /// </summary>
        AV_PKT_FLAG_CORRUPT = 0x0002
    }
}
