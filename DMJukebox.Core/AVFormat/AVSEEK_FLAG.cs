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
    /// This is a C# implementation of the AVSEEK_FLAG enum in FFmpeg.
    /// These flags are used to determine the behavior that FFmpeg should
    /// take while seeking manually through a media file. I use them
    /// during resets in <see cref="AudioTrack"/> once a file finishes or
    /// is stopped.
    /// </summary>
    /// <remarks>
    /// This enum is defined in avformat.h of the libavformat project.
    /// It isn't technically an enum in FFmpeg, just a bunch of macros.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    [Flags]
    internal enum AVSEEK_FLAG
    {
        AVSEEK_FLAG_BACKWARD = 1,
        AVSEEK_FLAG_BYTE = 2,
        AVSEEK_FLAG_ANY = 4,
        AVSEEK_FLAG_FRAME = 8
    }
}
