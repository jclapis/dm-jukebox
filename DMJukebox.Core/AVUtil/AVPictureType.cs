/* 
 * This file contains a C# implementation of the AVPictureType enum
 * as defined in avutil.h of the libavutil project, for interop use.
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
    /// AVPicture types, pixel formats and basic image planes manipulation.
    /// </summary>
    internal enum AVPictureType
    {
        /// <summary>
        /// Undefined
        /// </summary>
        AV_PICTURE_TYPE_NONE,

        /// <summary>
        /// Intra
        /// </summary>
        AV_PICTURE_TYPE_I,

        /// <summary>
        /// Predicted
        /// </summary>
        AV_PICTURE_TYPE_P,

        /// <summary>
        /// Bi-dir predicted
        /// </summary>
        AV_PICTURE_TYPE_B,

        /// <summary>
        /// S(GMC)-VOP MPEG-4
        /// </summary>
        AV_PICTURE_TYPE_S,

        /// <summary>
        /// Switching Intra
        /// </summary>
        AV_PICTURE_TYPE_SI,

        /// <summary>
        /// Switching Predicted
        /// </summary>
        AV_PICTURE_TYPE_SP,

        /// <summary>
        /// BI type
        /// </summary>
        AV_PICTURE_TYPE_BI
    }
}
