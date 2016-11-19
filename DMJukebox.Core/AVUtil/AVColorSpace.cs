/* 
 * This file contains a C# implementation of the AVColorSpace enum
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
    /// YUV colorspace type.
    /// </summary>
    internal enum AVColorSpace
    {
        /// <summary>
        /// order of coefficients is actually GBR, also IEC 61966-2-1 (sRGB)
        /// </summary>
        AVCOL_SPC_RGB = 0,

        /// <summary>
        /// also ITU-R BT1361 / IEC 61966-2-4 xvYCC709 / SMPTE RP177 Annex B
        /// </summary>
        AVCOL_SPC_BT709 = 1,
        AVCOL_SPC_UNSPECIFIED = 2,
        AVCOL_SPC_RESERVED = 3,

        /// <summary>
        /// FCC Title 47 Code of Federal Regulations 73.682 (a)(20)
        /// </summary>
        AVCOL_SPC_FCC = 4,

        /// <summary>
        /// also ITU-R BT601-6 625 / ITU-R BT1358 625 / ITU-R BT1700 625 PAL & SECAM / IEC 61966-2-4 xvYCC601
        /// </summary>
        AVCOL_SPC_BT470BG = 5,

        /// <summary>
        /// also ITU-R BT601-6 525 / ITU-R BT1358 525 / ITU-R BT1700 NTSC
        /// </summary>
        AVCOL_SPC_SMPTE170M = 6,

        /// <summary>
        /// functionally identical to above
        /// </summary>
        AVCOL_SPC_SMPTE240M = 7,

        /// <summary>
        /// Used by Dirac / VC-2 and H.264 FRext, see ITU-T SG16
        /// </summary>
        AVCOL_SPC_YCOCG = 8,

        /// <summary>
        /// ITU-R BT2020 non-constant luminance system
        /// </summary>
        AVCOL_SPC_BT2020_NCL = 9,

        /// <summary>
        /// ITU-R BT2020 constant luminance system
        /// </summary>
        AVCOL_SPC_BT2020_CL = 10,

        /// <summary>
        /// Not part of ABI
        /// </summary>
        AVCOL_SPC_NB
    }
}
