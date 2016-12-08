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
    /// This is a C# implementation of the AV_CODEC_CAP enum in FFmpeg.
    /// It describes the capabilities of a codec, as seen in
    /// <see cref="AVCodec.capabilities"/>. 
    /// </summary>
    /// <remarks>
    /// This enum is defined in avcodec.h of the libavcodec project.
    /// It isn't technically an enum in FFmpeg, just a bunch of macros.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    [Flags]
    internal enum AV_CODEC_CAP : uint
    {
        AV_CODEC_CAP_DRAW_HORIZ_BAND = 0x1,
        AV_CODEC_CAP_DR1 = 0x2,
        AV_CODEC_CAP_TRUNCATED = 0x8,
        AV_CODEC_CAP_DELAY = 0x20,
        AV_CODEC_CAP_SMALL_LAST_FRAME = 0x40,
        AV_CODEC_CAP_HWACCEL_VDPAU = 0x80,
        AV_CODEC_CAP_SUBFRAMES = 0x100,
        AV_CODEC_CAP_EXPERIMENTAL = 0x200,
        AV_CODEC_CAP_CHANNEL_CONF = 0x400,
        AV_CODEC_CAP_FRAME_THREADS = 0x1000,
        AV_CODEC_CAP_SLICE_THREADS = 0x2000,
        AV_CODEC_CAP_PARAM_CHANGE = 0x4000,
        AV_CODEC_CAP_AUTO_THREADS = 0x8000,
        AV_CODEC_CAP_VARIABLE_FRAME_SIZE = 0x10000,
        AV_CODEC_CAP_AVOID_PROBING = 0x20000,
        AV_CODEC_CAP_INTRA_ONLY = 0x40000000,
        AV_CODEC_CAP_LOSSLESS = 0x80000000
    }
}
