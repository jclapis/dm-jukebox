/* 
 * This file contains a C# implementation of the AV_CODEC_FLAG2 enum
 * as defined in avcodec.h of the libavcodec project, for interop use.
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
    internal enum AV_CODEC_FLAG2
    {
        AV_CODEC_FLAG2_FAST = 0x1,
        AV_CODEC_FLAG2_NO_OUTPUT = 0x4,
        AV_CODEC_FLAG2_LOCAL_HEADER = 0x8,
        AV_CODEC_FLAG2_DROP_FRAME_TIMECODE = 0x2000,
        AV_CODEC_FLAG2_CHUNKS = 0x20000,
        AV_CODEC_FLAG2_IGNORE_CROP = 0x40000,
        AV_CODEC_FLAG2_SHOW_ALL = 0x400000,
        AV_CODEC_FLAG2_EXPORT_MVS = 0x10000000,
        AV_CODEC_FLAG2_SKIP_MANUAL = 0x20000000,
        AV_CODEC_FLAG2_RO_FLUSH_NOOP = 0x40000000
    }
}
