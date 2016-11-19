/* 
 * This file contains a C# implementation of the FF_MB_DECISION enum
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
    internal enum FF_MB_DECISION
    {
        /// <summary>
        /// uses mb_cmp
        /// </summary>
        FF_MB_DECISION_SIMPLE,

        /// <summary>
        /// chooses the one which needs the fewest bits
        /// </summary>
        FF_MB_DECISION_BITS,

        /// <summary>
        /// rate distortion
        /// </summary>
        FF_MB_DECISION_RD
    }
}
