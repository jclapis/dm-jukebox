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
    /// This is a C# implementation of the FF_DECODE_ERROR enum in FFmpeg.
    /// It describes errors that might have occurred during decoding a media
    /// stream. It's stored internally by <see cref="AVFrame"/>. I don't use
    /// it yet but I should add support for it to get more info when something
    /// goes wrong at some point.
    /// </summary>
    /// <remarks>
    /// To get this, add a wrapper for av_frame_get_decode_error_flags(frame).
    /// 
    /// This enum is defined in frame.h of the libavutil project.
    /// It isn't technically an enum in FFmpeg, just a bunch of macros.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    [Flags]
    internal enum FF_DECODE_ERROR
    {
        FF_DECODE_ERROR_INVALID_BITSTREAM = 1,
        FF_DECODE_ERROR_MISSING_REFERENCE = 2
    }
}
