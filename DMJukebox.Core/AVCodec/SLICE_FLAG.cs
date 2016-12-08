/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

using System;

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a C# implementation of the SLICE_FLAG enum in FFmpeg.
    /// This has something to do with the way horizontal bands are
    /// processed in video streams, so I just ignore it. It shows up
    /// in <see cref="AVCodecContext.slice_flags"/>. 
    /// </summary>
    /// <remarks>
    /// This enum is defined in avcodec.h of the libavcodec project.
    /// It isn't technically an enum in FFmpeg, just a bunch of macros.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    [Flags]
    internal enum SLICE_FLAG
    {
        SLICE_FLAG_CODED_ORDER = 0x1,
        SLICE_FLAG_ALLOW_FIELD = 0x2,
        SLICE_FLAG_ALLOW_PLANE = 0x4
    }
}
