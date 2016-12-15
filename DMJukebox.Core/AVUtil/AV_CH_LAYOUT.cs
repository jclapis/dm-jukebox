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
    /// This is a C# implementation of the AV_CH_LAYOUT enum in FFmpeg.
    /// It describes common audio channel layouts (speaker placements) by 
    /// combining individual channels from <see cref="AV_CH"/>. 
    /// </summary>
    /// <remarks>
    /// This enum is defined in channel_layout.h of the libavutil project.
    /// It isn't technically an enum in FFmpeg, just a bunch of macros.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    internal enum AV_CH_LAYOUT : ulong
    {
        AV_CH_LAYOUT_MONO = AV_CH.AV_CH_FRONT_CENTER,
        AV_CH_LAYOUT_STEREO = AV_CH.AV_CH_FRONT_LEFT | AV_CH.AV_CH_FRONT_RIGHT,
        AV_CH_LAYOUT_2POINT1 = AV_CH_LAYOUT_STEREO | AV_CH.AV_CH_LOW_FREQUENCY,
        AV_CH_LAYOUT_2_1 = AV_CH_LAYOUT_STEREO | AV_CH.AV_CH_BACK_CENTER,
        AV_CH_LAYOUT_SURROUND = AV_CH_LAYOUT_STEREO | AV_CH.AV_CH_FRONT_CENTER,
        AV_CH_LAYOUT_3POINT1 = AV_CH_LAYOUT_SURROUND | AV_CH.AV_CH_LOW_FREQUENCY,
        AV_CH_LAYOUT_4POINT0 = AV_CH_LAYOUT_SURROUND | AV_CH.AV_CH_BACK_CENTER,
        AV_CH_LAYOUT_4POINT1 = AV_CH_LAYOUT_4POINT0 | AV_CH.AV_CH_LOW_FREQUENCY,
        AV_CH_LAYOUT_2_2 = AV_CH_LAYOUT_STEREO | AV_CH.AV_CH_SIDE_LEFT | AV_CH.AV_CH_SIDE_RIGHT,
        AV_CH_LAYOUT_QUAD = AV_CH_LAYOUT_STEREO | AV_CH.AV_CH_BACK_LEFT | AV_CH.AV_CH_BACK_RIGHT,
        AV_CH_LAYOUT_5POINT0 = AV_CH_LAYOUT_SURROUND | AV_CH.AV_CH_SIDE_LEFT | AV_CH.AV_CH_SIDE_RIGHT,
        AV_CH_LAYOUT_5POINT1 = AV_CH_LAYOUT_5POINT0 | AV_CH.AV_CH_LOW_FREQUENCY,
        AV_CH_LAYOUT_5POINT0_BACK = AV_CH_LAYOUT_SURROUND | AV_CH.AV_CH_BACK_LEFT | AV_CH.AV_CH_BACK_RIGHT,
        AV_CH_LAYOUT_5POINT1_BACK = AV_CH_LAYOUT_5POINT0_BACK | AV_CH.AV_CH_LOW_FREQUENCY,
        AV_CH_LAYOUT_6POINT0 = AV_CH_LAYOUT_5POINT0 | AV_CH.AV_CH_BACK_CENTER,
        AV_CH_LAYOUT_6POINT0_FRONT = AV_CH_LAYOUT_2_2 | AV_CH.AV_CH_FRONT_LEFT_OF_CENTER | AV_CH.AV_CH_FRONT_RIGHT_OF_CENTER,
        AV_CH_LAYOUT_HEXAGONAL = AV_CH_LAYOUT_5POINT0_BACK | AV_CH.AV_CH_BACK_CENTER,
        AV_CH_LAYOUT_6POINT1 = AV_CH_LAYOUT_5POINT1 | AV_CH.AV_CH_BACK_CENTER,
        AV_CH_LAYOUT_6POINT1_BACK = AV_CH_LAYOUT_5POINT1_BACK | AV_CH.AV_CH_BACK_CENTER,
        AV_CH_LAYOUT_6POINT1_FRONT = AV_CH_LAYOUT_6POINT0_FRONT | AV_CH.AV_CH_LOW_FREQUENCY,
        AV_CH_LAYOUT_7POINT0 = AV_CH_LAYOUT_5POINT0 | AV_CH.AV_CH_BACK_LEFT | AV_CH.AV_CH_BACK_RIGHT,
        AV_CH_LAYOUT_7POINT0_FRONT = AV_CH_LAYOUT_5POINT0 | AV_CH.AV_CH_FRONT_LEFT_OF_CENTER | AV_CH.AV_CH_FRONT_RIGHT_OF_CENTER,
        AV_CH_LAYOUT_7POINT1 = AV_CH_LAYOUT_5POINT1 | AV_CH.AV_CH_BACK_LEFT | AV_CH.AV_CH_BACK_RIGHT,
        AV_CH_LAYOUT_7POINT1_WIDE = AV_CH_LAYOUT_5POINT1 | AV_CH.AV_CH_FRONT_LEFT_OF_CENTER | AV_CH.AV_CH_FRONT_RIGHT_OF_CENTER,
        AV_CH_LAYOUT_7POINT1_WIDE_BACK = AV_CH_LAYOUT_5POINT1_BACK | AV_CH.AV_CH_FRONT_LEFT_OF_CENTER | AV_CH.AV_CH_FRONT_RIGHT_OF_CENTER,
        AV_CH_LAYOUT_OCTAGONAL = AV_CH_LAYOUT_5POINT0 | AV_CH.AV_CH_BACK_LEFT | AV_CH.AV_CH_BACK_CENTER | AV_CH.AV_CH_BACK_RIGHT,
        AV_CH_LAYOUT_HEXADECAGONAL = AV_CH_LAYOUT_OCTAGONAL | AV_CH.AV_CH_WIDE_LEFT | AV_CH.AV_CH_WIDE_RIGHT | AV_CH.AV_CH_TOP_BACK_LEFT
            | AV_CH.AV_CH_TOP_BACK_RIGHT | AV_CH.AV_CH_TOP_BACK_CENTER | AV_CH.AV_CH_TOP_FRONT_CENTER | AV_CH.AV_CH_TOP_FRONT_LEFT | AV_CH.AV_CH_TOP_FRONT_RIGHT,
        AV_CH_LAYOUT_STEREO_DOWNMIX = AV_CH.AV_CH_STEREO_LEFT | AV_CH.AV_CH_STEREO_RIGHT
    }
}
