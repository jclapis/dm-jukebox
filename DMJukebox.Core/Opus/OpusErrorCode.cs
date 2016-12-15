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
    /// This is a C# implementation of the list of error codes
    /// defined in Opus.
    /// </summary>
    /// <remarks>These values are listed in opus_defines.h of libopus.
    /// They don't form an enum, they're just constants defined in macros.
    /// For more information, please see the documentation at 
    /// https://opus-codec.org/docs/opus_api-1.1.3/index.html
    /// or the source code at https://git.xiph.org/?p=opus.git.
    /// </remarks>
    internal enum OpusErrorCode
    {
        /// <summary>
        /// Success
        /// </summary>
        OPUS_OK = 0,

        /// <summary>
        /// One of the arguments is invalid or out of range
        /// </summary>
        OPUS_BAD_ARG = -1,

        /// <summary>
        /// The buffer being used is too small
        /// </summary>
        OPUS_BUFFER_TOO_SMALL = -2,

        /// <summary>
        /// Something went wrong with Opus's internal code
        /// </summary>
        OPUS_INTERNAL_ERROR = -3,

        /// <summary>
        /// The encoded audio being provided is corrupt
        /// </summary>
        OPUS_INVALID_PACKET = -4,

        /// <summary>
        /// The functionality being requested hasn't been
        /// implemented yet
        /// </summary>
        OPUS_UNIMPLEMENTED = -5,

        /// <summary>
        /// The encoder context's state is invalid, or it
        /// has already been destroyed
        /// </summary>
        OPUS_INVALID_STATE = -6,

        /// <summary>
        /// Memory allocation failed
        /// </summary>
        OPUS_ALLOC_FAIL = -7
    }
}
