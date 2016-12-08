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
    /// This is a C# implementation of the AVFMT_FLAG enum in FFmpeg.
    /// These modify how the muxer and demuxer processes media files.
    /// I don't have much of a use for these settings.
    /// </summary>
    /// <remarks>
    /// This enum is defined in avformat.h of the libavformat project.
    /// It isn't technically an enum in FFmpeg, just a bunch of macros.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    [Flags]
    internal enum AVFMT_FLAG : int
    {
        AVFMT_FLAG_GENPTS = 0x0001,
        AVFMT_FLAG_IGNIDX = 0x0002,
        AVFMT_FLAG_NONBLOCK = 0x0004,
        AVFMT_FLAG_IGNDTS = 0x0008,
        AVFMT_FLAG_NOFILLIN = 0x0010,
        AVFMT_FLAG_NOPARSE = 0x0020,
        AVFMT_FLAG_NOBUFFER = 0x0040,
        AVFMT_FLAG_CUSTOM_IO = 0x0080,
        AVFMT_FLAG_DISCARD_CORRUPT = 0x0100,
        AVFMT_FLAG_FLUSH_PACKETS = 0x0200,
        AVFMT_FLAG_BITEXACT = 0x0400,
        AVFMT_FLAG_MP4A_LATM = 0x8000,
        AVFMT_FLAG_SORT_DTS = 0x10000,
        AVFMT_FLAG_PRIV_OPT = 0x20000,
        AVFMT_FLAG_KEEP_SIDE_DATA = 0x40000,
        AVFMT_FLAG_FAST_SEEK = 0x80000
    }
}
