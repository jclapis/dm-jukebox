/* 
 * This file contains a C# implementation of the AVDiscard enum
 * as defined in avcodec.h of the libavcodec project, for interop use.
 * 
 * For more information, please see the documentation at
 * https://www.ffmpeg.org/doxygen/trunk/index.html or the source code at
 * https://github.com/FFmpeg/FFmpeg.
 * 
 * Copyright (c) 2016 Joe Clapis.
 */

namespace DMJukebox.Interop
{
    internal enum AVDiscard
    {
        AVDISCARD_NONE = -16,
        AVDISCARD_DEFAULT = 0,
        AVDISCARD_NONREF = 8,
        AVDISCARD_BIDIR = 16,
        AVDISCARD_NONINTRA = 24,
        AVDISCARD_NONKEY = 32,
        AVDISCARD_ALL = 48
    }
}
