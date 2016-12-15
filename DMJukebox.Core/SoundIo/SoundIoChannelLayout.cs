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

using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a C# implementation of the SoundIoChannelLayout struct in libsoundio.
    /// It defines a physical configuration of audio channels, representing actual
    /// speaker placement.
    /// </summary>
    /// <remarks>
    /// This struct is defined in soundio.h of libsoundio.
    /// For more information, please see the documentation at
    /// http://libsound.io/doc-1.1.0/soundio_8h.html
    /// or the source code at https://github.com/andrewrk/libsoundio.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct SoundIoChannelLayout
    {
        /// <summary>
        /// The human-readable name of the channel layout
        /// </summary>
        public string name;

        /// <summary>
        /// The number of channels stored in <see cref="channels"/>
        /// </summary>
        public int channel_count;

        /// <summary>
        /// A collection of channels that this layout uses
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public SoundIoChannelId[] channels;
    }
}
