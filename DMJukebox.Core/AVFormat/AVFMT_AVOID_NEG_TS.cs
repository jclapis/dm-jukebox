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

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a C# implementation of the AVFMT_AVOID_NEG_TS enum in FFmpeg.
    /// It's used to determine how FFmpeg should deal with negative timestamps
    /// while muxing streams together. Since I don't do any of that, I can
    /// ignore this enum.
    /// </summary>
    /// <remarks>
    /// This enum is defined in avformat.h of the libavformat project.
    /// It isn't technically an enum in FFmpeg, just a bunch of macros.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    internal enum AVFMT_AVOID_NEG_TS
    {
        AUTO = -1,
        MAKE_NON_NEGATIVE = 1,
        MAKE_ZERO = 2
    }
}
