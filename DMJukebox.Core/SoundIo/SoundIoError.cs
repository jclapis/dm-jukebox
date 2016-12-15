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
    /// This is a C# implementation of the SoundIoError enum in libsoundio.
    /// It describes the different error codes that the library might return
    /// during operations.
    /// </summary>
    /// This enum is defined in soundio.h of libsoundio.
    /// For more information, please see the documentation at
    /// http://libsound.io/doc-1.1.0/soundio_8h.html
    /// or the source code at https://github.com/andrewrk/libsoundio.
    /// </remarks>
    internal enum SoundIoError
    {
        /// <summary>
        /// Success
        /// </summary>
        SoundIoErrorNone,

        /// <summary>
        /// Out of memory, failed allocating a new buffer
        /// </summary>
        SoundIoErrorNoMem,

        /// <summary>
        /// Something is wrong with the audio backend
        /// </summary>
        SoundIoErrorInitAudioBackend,

        /// <summary>
        /// Failed to acquire a system resource (other than memory)
        /// </summary>
        SoundIoErrorSystemResources,

        /// <summary>
        /// Opening a sound device failed
        /// </summary>
        SoundIoErrorOpeningDevice,

        /// <summary>
        /// The specified device could not be found
        /// </summary>
        SoundIoErrorNoSuchDevice,

        /// <summary>
        /// One of the arguments was invalid
        /// </summary>
        SoundIoErrorInvalid,

        /// <summary>
        /// libsoundio wasn't compiled with support for the
        /// requested backend.
        /// </summary>
        SoundIoErrorBackendUnavailable,

        /// <summary>
        /// Something went wrong while reading from / writing to
        /// a stream, so it must be destroyed and reopened.
        /// </summary>
        SoundIoErrorStreaming,

        /// <summary>
        /// The device doesn't support the requested parameters
        /// </summary>
        SoundIoErrorIncompatibleDevice,

        /// <summary>
        /// The backend can't find the requested client
        /// </summary>
        SoundIoErrorNoSuchClient,

        /// <summary>
        /// The backend doesn't support the requested parameters
        /// </summary>
        SoundIoErrorIncompatibleBackend,

        /// <summary>
        /// The connection to the backend was lost 
        /// </summary>
        SoundIoErrorBackendDisconnected,

        /// <summary>
        /// The connection to the backend was interrupted during
        /// operations
        /// </summary>
        SoundIoErrorInterrupted,

        /// <summary>
        /// A buffer underrun was detected, not enough data is
        /// currently available to process
        /// </summary>
        SoundIoErrorUnderflow,

        /// <summary>
        /// A string couldn't be converted from UTF8 to the native format
        /// </summary>
        SoundIoErrorEncodingString
    }
}
