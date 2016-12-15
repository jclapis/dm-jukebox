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
    /// This describes the sample rate of input audio data to encode with Opus.
    /// </summary>
    /// <remarks>
    /// This enum doesn't exist in libopus, I created it as a helper to enforce the
    /// restriction on the Fs parameter of <see cref="OpusInterop.opus_encoder_create(
    /// OpusSampleRate, OpusChannelCount, OPUS_APPLICATION, out OpusErrorCode)"/>.
    /// </remarks>
    internal enum OpusSampleRate
    {
        /// <summary>
        /// 8 kHz
        /// </summary>
        _8000 = 8000,

        /// <summary>
        /// 12 kHz 
        /// </summary>
        _12000 = 12000,

        /// <summary>
        /// 16 kHz
        /// </summary>
        _16000 = 16000,

        /// <summary>
        /// 24 kHz
        /// </summary>
        _24000 = 24000,

        /// <summary>
        /// 48 kHz
        /// </summary>
        _48000 = 48000
    }
}
