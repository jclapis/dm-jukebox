/* 
 * This file contains a C# implementation of the SLICE_FLAG enum
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

using System;

namespace DMJukebox.Interop
{
    [Flags]
    internal enum SLICE_FLAG
    {
        /// <summary>
        /// draw_horiz_band() is called in coded order instead of display
        /// </summary>
        SLICE_FLAG_CODED_ORDER = 0x1,

        /// <summary>
        /// allow draw_horiz_band() with field slices (MPEG-2 field pics)
        /// </summary>
        SLICE_FLAG_ALLOW_FIELD = 0x2,

        /// <summary>
        /// allow draw_horiz_band() with 1 component at a time (SVQ1)
        /// </summary>
        SLICE_FLAG_ALLOW_PLANE = 0x4
    }
}
