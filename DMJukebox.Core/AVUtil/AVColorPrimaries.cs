/* 
 * This file contains a C# implementation of the AVColorPrimaries enum
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
    /// Chromaticity coordinates of the source primaries.
    /// </summary>
    internal enum AVColorPrimaries
    {
        AVCOL_PRI_RESERVED0 = 0,

        /// <summary>
        /// also ITU-R BT1361 / IEC 61966-2-4 / SMPTE RP177 Annex B
        /// </summary>
        AVCOL_PRI_BT709 = 1,
        AVCOL_PRI_UNSPECIFIED = 2,
        AVCOL_PRI_RESERVED = 3,

        /// <summary>
        /// also FCC Title 47 Code of Federal Regulations 73.682 (a)(20)
        /// </summary>
        AVCOL_PRI_BT470M = 4,

        /// <summary>
        /// also ITU-R BT601-6 625 / ITU-R BT1358 625 / ITU-R BT1700 625 PAL & SECAM
        /// </summary>
        AVCOL_PRI_BT470BG = 5,

        /// <summary>
        /// also ITU-R BT601-6 525 / ITU-R BT1358 525 / ITU-R BT1700 NTSC
        /// </summary>
        AVCOL_PRI_SMPTE170M = 6,

        /// <summary>
        /// functionally identical to above
        /// </summary>
        AVCOL_PRI_SMPTE240M = 7,

        /// <summary>
        /// colour filters using Illuminant C
        /// </summary>
        AVCOL_PRI_FILM = 8,

        /// <summary>
        /// ITU-R BT2020
        /// </summary>
        AVCOL_PRI_BT2020 = 9,

        /// <summary>
        /// SMPTE ST 428-1 (CIE 1931 XYZ)
        /// </summary>
        AVCOL_PRI_SMPTEST428_1 = 10,

        /// <summary>
        /// Not part of ABI
        /// </summary>
        AVCOL_PRI_NB
    }
}
