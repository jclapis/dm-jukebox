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
    /// This is a C# implementation of the AVColorRange enum in FFmpeg.
    /// It defines the color range for video streams encoded in YUV format,
    /// so I don't need it.
    /// </summary>
    /// <remarks>
    /// This enum is defined in pixfmt.h of the libavutil project.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    internal enum AVColorRange
    {
        AVCOL_RANGE_UNSPECIFIED = 0,
        AVCOL_RANGE_MPEG = 1,
        AVCOL_RANGE_JPEG = 2,
        AVCOL_RANGE_NB,
    }
}
