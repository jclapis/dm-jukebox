/* 
 * This file contains a C# implementation of the AVColorRange enum
 * as defined in pixfmt.h of the libavutil project, for interop use.
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
    /// <summary>
    /// MPEG vs JPEG YUV range.
    /// </summary>
    internal enum AVColorRange
    {
        AVCOL_RANGE_UNSPECIFIED = 0,

        /// <summary>
        /// the normal 219*2^(n-8) "MPEG" YUV ranges
        /// </summary>
        AVCOL_RANGE_MPEG = 1,

        /// <summary>
        /// the normal 2^n-1 "JPEG" YUV ranges
        /// </summary>
        AVCOL_RANGE_JPEG = 2,

        /// <summary>
        /// Not part of ABI
        /// </summary>
        AVCOL_RANGE_NB,
    }
}
