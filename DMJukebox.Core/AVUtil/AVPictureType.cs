/* 
 * This file contains a C# implementation of the AVPictureType enum
 * as defined in avutil.h of the libavutil project, for interop use.
 * 
 * For more information, please see the documentation at
 * https://www.ffmpeg.org/doxygen/trunk/index.html or the source code at
 * https://github.com/FFmpeg/FFmpeg.
 * 
 * Copyright (c) 2016 Joe Clapis.
 */

namespace DMJukebox.Interop
{
    internal enum AVPictureType
    {
        AV_PICTURE_TYPE_NONE,
        AV_PICTURE_TYPE_I,
        AV_PICTURE_TYPE_P,
        AV_PICTURE_TYPE_B,
        AV_PICTURE_TYPE_S,
        AV_PICTURE_TYPE_SI,
        AV_PICTURE_TYPE_SP,
        AV_PICTURE_TYPE_BI
    }
}
