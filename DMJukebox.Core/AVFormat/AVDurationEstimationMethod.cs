/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a C# implementation of the AVDurationEstimationMethod enum in FFmpeg.
    /// It describes the technique that FFmpeg used to figure out the duration of
    /// a media file, since some of them don't have that information built in.
    /// </summary>
    /// <remarks>
    /// This enum is defined in avformat.h of the libavformat project.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    internal enum AVDurationEstimationMethod
    {
        AVFMT_DURATION_FROM_PTS,
        AVFMT_DURATION_FROM_STREAM,
        AVFMT_DURATION_FROM_BITRATE
    }
}
