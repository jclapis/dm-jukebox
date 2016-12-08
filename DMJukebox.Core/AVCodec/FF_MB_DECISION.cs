/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a C# implementation of the FF_MB_DECISION enum in FFmpeg.
    /// Apparently this is used in macroblock processing, which is for
    /// video streams so I just ignore it. It shows up in
    /// <see cref="AVCodecContext.mb_decision"/>.
    /// </summary>
    /// <remarks>
    /// This enum is defined in avcodec.h of the libavcodec project.
    /// It isn't technically an enum in FFmpeg, just a bunch of macros.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    internal enum FF_MB_DECISION
    {
        FF_MB_DECISION_SIMPLE,
        FF_MB_DECISION_BITS,
        FF_MB_DECISION_RD
    }
}
