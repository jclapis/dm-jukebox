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
    /// This is a C# implementation of the AVColorTransferCharacteristic enum in FFmpeg.
    /// It has something to do with "color transfer" which is beyond me. It's all video
    /// stuff so I'm not going to use it.
    /// </summary>
    /// <remarks>
    /// This enum is defined in pixfmt.h of the libavutil project.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    public enum AVColorTransferCharacteristic
    {
        AVCOL_TRC_RESERVED0 = 0,
        AVCOL_TRC_BT709 = 1,
        AVCOL_TRC_UNSPECIFIED = 2,
        AVCOL_TRC_RESERVED = 3,
        AVCOL_TRC_GAMMA22 = 4,
        AVCOL_TRC_GAMMA28 = 5,
        AVCOL_TRC_SMPTE170M = 6,
        AVCOL_TRC_SMPTE240M = 7,
        AVCOL_TRC_LINEAR = 8,
        AVCOL_TRC_LOG = 9,
        AVCOL_TRC_LOG_SQRT = 10,
        AVCOL_TRC_IEC61966_2_4 = 11,
        AVCOL_TRC_BT1361_ECG = 12,
        AVCOL_TRC_IEC61966_2_1 = 13,
        AVCOL_TRC_BT2020_10 = 14,
        AVCOL_TRC_BT2020_12 = 15,
        AVCOL_TRC_SMPTEST2084 = 16,
        AVCOL_TRC_SMPTEST428_1 = 17,
        AVCOL_TRC_ARIB_STD_B67 = 18,
        AVCOL_TRC_NB
    }
}
