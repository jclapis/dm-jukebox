/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a C# implementation of the AV_PKT_FLAG enum in FFmpeg.
    /// </summary>
    /// <remarks>
    /// This enum is defined in avcodec.h of the libavcodec project.
    /// It isn't technically an enum in FFmpeg, just a bunch of macros.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    internal enum AV_PKT_FLAG
    {
        AV_PKT_FLAG_KEY = 0x0001,
        AV_PKT_FLAG_CORRUPT = 0x0002
    }
}
