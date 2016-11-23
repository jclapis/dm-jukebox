/* 
 * This file contains a C# implementation of the AVColorRange enum
 * as defined in pixfmt.h of the libavutil project, for interop use.
 * 
 * For more information, please see the documentation at
 * https://www.ffmpeg.org/doxygen/trunk/index.html or the source code at
 * https://github.com/FFmpeg/FFmpeg.
 * 
 * Copyright (c) 2016 Joe Clapis.
 */

namespace DMJukebox.Interop
{
    internal enum AVColorRange
    {
        AVCOL_RANGE_UNSPECIFIED = 0,
        AVCOL_RANGE_MPEG = 1,
        AVCOL_RANGE_JPEG = 2,
        AVCOL_RANGE_NB,
    }
}
