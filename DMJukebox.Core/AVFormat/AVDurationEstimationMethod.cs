/* 
 * This file contains a C# implementation of the AVDurationEstimationMethod enum
 * as defined in avformat.h of the libavformat project, for interop use.
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
    /// The duration of a video can be estimated through various ways, and this enum can be used
    /// to know how the duration was estimated.
    /// </summary>
    internal enum AVDurationEstimationMethod
    {
        /// <summary>
        /// Duration accurately estimated from PTSes
        /// </summary>
        AVFMT_DURATION_FROM_PTS,

        /// <summary>
        /// Duration estimated from a stream with a known duration
        /// </summary>
        AVFMT_DURATION_FROM_STREAM,

        /// <summary>
        /// Duration estimated from bitrate (less accurate)
        /// </summary>
        AVFMT_DURATION_FROM_BITRATE
    }

}
