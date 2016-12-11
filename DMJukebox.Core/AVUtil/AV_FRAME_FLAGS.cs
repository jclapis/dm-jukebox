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
    /// This is a C# implementation of the AV_FRAME_FLAGS enum in FFmpeg.
    /// It describes some extra details about processing an <see cref="AVFrame"/>.
    /// </summary>
    /// <remarks>
    /// This enum is defined in frame.h of the libavutil project.
    /// It isn't technically an enum in FFmpeg, just a bunch of macros.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    [Flags]
    internal enum AV_FRAME_FLAGS
    {
        /// <summary>
        /// The frame might be corrupted or encountered errors during decoding
        /// </summary>
        AV_FRAME_FLAG_CORRUPT = 1,

        /// <summary>
        /// This frame should be decoded but not displayed.
        /// </summary>
        AV_FRAME_FLAG_DISCARD = 4
    }
}
