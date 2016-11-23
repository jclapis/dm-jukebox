/* 
 * This file contains a C# implementation of the AVFMT_FLAG enum
 * as defined in avformat.h of the libavformat project, for interop use.
 * It isn't technically an enum in ffmpeg, just a bunch of macros.
 * 
 * For more information, please see the documentation at
 * https://www.ffmpeg.org/doxygen/trunk/index.html or the source code at
 * https://github.com/FFmpeg/FFmpeg.
 * 
 * Copyright (c) 2016 Joe Clapis.
 */

using System;

namespace DMJukebox.Interop
{
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
