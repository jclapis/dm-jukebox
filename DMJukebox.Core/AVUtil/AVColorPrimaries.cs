/* 
 * This file contains a C# implementation of the AVColorPrimaries enum
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
    internal enum AVColorPrimaries
    {
        AVCOL_PRI_RESERVED0 = 0,
        AVCOL_PRI_BT709 = 1,
        AVCOL_PRI_UNSPECIFIED = 2,
        AVCOL_PRI_RESERVED = 3,
        AVCOL_PRI_BT470M = 4,
        AVCOL_PRI_BT470BG = 5,
        AVCOL_PRI_SMPTE170M = 6,
        AVCOL_PRI_SMPTE240M = 7,
        AVCOL_PRI_FILM = 8,
        AVCOL_PRI_BT2020 = 9,
        AVCOL_PRI_SMPTEST428_1 = 10,
        AVCOL_PRI_NB
    }
}
