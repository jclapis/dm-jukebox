/* ========================================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * ====================================================================== */

using System;

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a C# implementation of the AV_CODEC_FLAG2 enum in FFmpeg.
    /// It's used in <see cref="AVCodecContext.flags2"/> to describe some
    /// additional attributes of the loaded codec context that weren't
    /// covered by <see cref="AVCodecContext.flags"/>. 
    /// </summary>
    /// <remarks>
    /// This enum is defined in avcodec.h of the libavcodec project.
    /// It isn't technically an enum in FFmpeg, just a bunch of macros.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
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
