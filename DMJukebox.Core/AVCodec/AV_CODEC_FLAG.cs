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
    /// This is a C# implementation of the AV_CODEC_FLAG enum in FFmpeg.
    /// It's used in <see cref="AVCodecContext.flags"/> to describe some
    /// attributes of the loaded codec context.
    /// </summary>
    /// <remarks>
    /// This enum is defined in avcodec.h of the libavcodec project.
    /// It isn't technically an enum in FFmpeg, just a bunch of macros.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    [Flags]
    internal enum AV_CODEC_FLAG : uint
    {
        AV_CODEC_FLAG_UNALIGNED = 0x1,
        AV_CODEC_FLAG_QSCALE = 0x2,
        AV_CODEC_FLAG_4MV = 0x4,
        AV_CODEC_FLAG_OUTPUT_CORRUPT = 0x8,
        AV_CODEC_FLAG_QPEL = 0x10,
        AV_CODEC_FLAG_PASS1 = 0x200,
        AV_CODEC_FLAG_PASS2 = 0x400,
        AV_CODEC_FLAG_LOOP_FILTER = 0x800,
        AV_CODEC_FLAG_GRAY = 0x2000,
        AV_CODEC_FLAG_PSNR = 0x8000,
        AV_CODEC_FLAG_TRUNCATED = 0x10000,
        AV_CODEC_FLAG_INTERLACED_DCT = 0x40000,
        AV_CODEC_FLAG_LOW_DELAY = 0x80000,
        AV_CODEC_FLAG_GLOBAL_HEADER = 0x400000,
        AV_CODEC_FLAG_BITEXACT = 0x800000,
        AV_CODEC_FLAG_AC_PRED = 0x1000000,
        AV_CODEC_FLAG_INTERLACED_ME = 0x20000000,
        AV_CODEC_FLAG_CLOSED_GOP = 0x80000000
    }
}
