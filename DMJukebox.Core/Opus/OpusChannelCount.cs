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
    /// This describes the number of channels in the input audio data to encode with Opus.
    /// </summary>
    /// <remarks>
    /// This enum doesn't exist in libopus, I created it as a helper to enforce the
    /// restriction on the channels parameter of <see cref="OpusInterop.opus_encoder_create(
    /// OpusSampleRate, OpusChannelCount, OPUS_APPLICATION, out OpusErrorCode)"/> 
    /// which must be 1 or 2.
    /// </remarks>
    internal enum OpusChannelCount
    {
        /// <summary>
        /// 1 channel (mono)
        /// </summary>
        _1 = 1,

        /// <summary>
        /// 2 channels (stereo)
        /// </summary>
        _2 = 2
    }
}
