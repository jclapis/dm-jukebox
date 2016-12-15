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
    /// This is a C# implementation of the SLICE_FLAG enum in FFmpeg.
    /// This has something to do with the way horizontal bands are
    /// processed in video streams, so I just ignore it. It shows up
    /// in <see cref="AVCodecContext.slice_flags"/>. 
    /// </summary>
    /// <remarks>
    /// This enum is defined in avcodec.h of the libavcodec project.
    /// It isn't technically an enum in FFmpeg, just a bunch of macros.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    [Flags]
    internal enum SLICE_FLAG
    {
        SLICE_FLAG_CODED_ORDER = 0x1,
        SLICE_FLAG_ALLOW_FIELD = 0x2,
        SLICE_FLAG_ALLOW_PLANE = 0x4
    }
}
