/* 
 * This file contains a C# implementation of the AVDurationEstimationMethod enum
 * as defined in avformat.h of the libavformat project, for interop use.
 * 
 * For more information, please see the documentation at
 * https://www.ffmpeg.org/doxygen/trunk/index.html or the source code at
 * https://github.com/FFmpeg/FFmpeg.
 * 
 * Copyright (c) 2016 Joe Clapis.
 */

namespace DMJukebox.Interop
{
    internal enum AVDurationEstimationMethod
    {
        AVFMT_DURATION_FROM_PTS,
        AVFMT_DURATION_FROM_STREAM,
        AVFMT_DURATION_FROM_BITRATE
    }

}
