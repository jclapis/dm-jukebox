/* 
 * This file contains a C# implementation of the AV_CODEC_FLAG2 enum
 * as defined in avcodec.h of the libavcodec project, for interop use.
 * It isn't technically an enum in ffmpeg, just a bunch of macros.
 * 
 * The documentation and comments have been largely copied from those headers and
 * are not my own work - they are the work of the contributors to ffmpeg.
 * Credit goes to them. I may have modified them in places where it made sense
 * to help document the C# bindings.
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
        /// <summary>
        /// Allow non spec compliant speedup tricks.
        /// </summary>
        AV_CODEC_FLAG2_FAST = 0x1,

        /// <summary>
        /// Skip bitstream encoding.
        /// </summary>
        AV_CODEC_FLAG2_NO_OUTPUT = 0x4,

        /// <summary>
        /// Place global headers at every keyframe instead of in extradata.
        /// </summary>
        AV_CODEC_FLAG2_LOCAL_HEADER = 0x8,

        /// <summary>
        /// timecode is in drop frame format. DEPRECATED!!!!
        /// </summary>
        AV_CODEC_FLAG2_DROP_FRAME_TIMECODE = 0x2000,

        /// <summary>
        /// Input bitstream might be truncated at a packet boundaries
        /// instead of only at frame boundaries.
        /// </summary>
        AV_CODEC_FLAG2_CHUNKS = 0x20000,

        /// <summary>
        /// Discard cropping information from SPS.
        /// </summary>
        AV_CODEC_FLAG2_IGNORE_CROP = 0x40000,

        /// <summary>
        /// Show all frames before the first keyframe
        /// </summary>
        AV_CODEC_FLAG2_SHOW_ALL = 0x400000,

        /// <summary>
        /// Export motion vectors through frame side data
        /// </summary>
        AV_CODEC_FLAG2_EXPORT_MVS = 0x10000000,

        /// <summary>
        /// Do not skip samples and export skip information as frame side data
        /// </summary>
        AV_CODEC_FLAG2_SKIP_MANUAL = 0x20000000,

        /// <summary>
        /// Do not reset ASS ReadOrder field on flush (subtitles decoding)
        /// </summary>
        AV_CODEC_FLAG2_RO_FLUSH_NOOP = 0x40000000
    }
}
