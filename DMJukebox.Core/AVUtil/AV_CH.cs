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
    /// This is a C# implementation of the AV_CH enum in FFmpeg.
    /// It's used to define the name / location of an audio channel
    /// (like speaker placement).
    /// </summary>
    /// <remarks>
    /// This enum is defined in channel_layout.h of the libavutil project.
    /// It isn't technically an enum in FFmpeg, just a bunch of macros.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    [Flags]
    internal enum AV_CH : ulong
    {
        AV_CH_FRONT_LEFT = 0x00000001,
        AV_CH_FRONT_RIGHT = 0x00000002,
        AV_CH_FRONT_CENTER = 0x00000004,
        AV_CH_LOW_FREQUENCY = 0x00000008,
        AV_CH_BACK_LEFT = 0x00000010,
        AV_CH_BACK_RIGHT = 0x00000020,
        AV_CH_FRONT_LEFT_OF_CENTER = 0x00000040,
        AV_CH_FRONT_RIGHT_OF_CENTER = 0x00000080,
        AV_CH_BACK_CENTER = 0x00000100,
        AV_CH_SIDE_LEFT = 0x00000200,
        AV_CH_SIDE_RIGHT = 0x00000400,
        AV_CH_TOP_CENTER = 0x00000800,
        AV_CH_TOP_FRONT_LEFT = 0x00001000,
        AV_CH_TOP_FRONT_CENTER = 0x00002000,
        AV_CH_TOP_FRONT_RIGHT = 0x00004000,
        AV_CH_TOP_BACK_LEFT = 0x00008000,
        AV_CH_TOP_BACK_CENTER = 0x00010000,
        AV_CH_TOP_BACK_RIGHT = 0x00020000,
        AV_CH_STEREO_LEFT = 0x20000000,
        AV_CH_STEREO_RIGHT = 0x40000000,
        AV_CH_WIDE_LEFT = 0x0000000080000000,
        AV_CH_WIDE_RIGHT = 0x0000000100000000,
        AV_CH_SURROUND_DIRECT_LEFT = 0x0000000200000000,
        AV_CH_SURROUND_DIRECT_RIGHT = 0x0000000400000000,
        AV_CH_LOW_FREQUENCY_2 = 0x0000000800000000,
        AV_CH_LAYOUT_NATIVE = 0x8000000000000000
    }
}
