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
using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a C# implementation of the SoundIoDevice struct in libsoundio.
    /// It describes a physical audio I/O device that can be used to play sound
    /// out to the speakers.
    /// </summary>
    /// <remarks>
    /// This struct is defined in soundio.h of libsoundio.
    /// For more information, please see the documentation at
    /// http://libsound.io/doc-1.1.0/soundio_8h.html
    /// or the source code at https://github.com/andrewrk/libsoundio.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct SoundIoDevice
    {
        /// <summary>
        /// (<see cref="SoundIo"/>*) A readonly pointer to the context
        /// </summary>
        public IntPtr soundio;
        
        /// <summary>
        /// The unique ID for this device
        /// </summary>
        public string id;
        
        /// <summary>
        /// The human-readable name for this device
        /// </summary>
        public string name;
        
        /// <summary>
        /// The direction of the device (input or output)
        /// </summary>
        public SoundIoDeviceAim aim;
        
        /// <summary>
        /// (<see cref="SoundIoChannelLayout"/>*) The list of
        /// layouts that this device supports
        /// </summary>
        public IntPtr layouts;

        /// <summary>
        /// The number of elements in <see cref="layouts"/>
        /// </summary>
        public int layout_count;
        
        /// <summary>
        /// The active layout for the device
        /// </summary>
        public SoundIoChannelLayout current_layout;
        
        /// <summary>
        /// (<see cref="SoundIoFormat"/>*) The list of formats
        /// for incoming sample data that the device supports
        /// </summary>
        public IntPtr formats;
        
        /// <summary>
        /// The number of elements in <see cref="formats"/>
        /// </summary>
        public int format_count;
        
        /// <summary>
        /// The format the device is currently set to use. All
        /// incoming data will be converted to this format
        /// behind the scenes.
        /// </summary>
        public SoundIoFormat current_format;
        
        /// <summary>
        /// (<see cref="SoundIoSampleRateRange"/>*) The list of
        /// sample rates that the device supports
        /// </summary>
        public IntPtr sample_rates;
        
        /// <summary>
        /// The number of elements in <see cref="sample_rates"/>
        /// </summary>
        public int sample_rate_count;
        
        /// <summary>
        /// The active sample rate
        /// </summary>
        public int sample_rate_current;
        
        /// <summary>
        /// The minimum amount of software latency, in seconds
        /// </summary>
        public double software_latency_min;
        
        /// <summary>
        /// The maximum amount of software latency, in seconds
        /// </summary>
        public double software_latency_max;
        
        /// <summary>
        /// The active amount of software latency, in seconds
        /// </summary>
        public double software_latency_current;
        
        /// <summary>
        /// True if this is a raw output device (straight to hardware
        /// with exclusive access to it), false if this is a virtual
        /// device with a backend proxy that allows multiple applications
        /// to use it simultaneously
        /// </summary>
        public bool is_raw;
        
        /// <summary>
        /// An internal tracker for counting the number of times this
        /// device has been referenced
        /// </summary>
        public int ref_count;
        
        /// <summary>
        /// If something went wrong while probing the device to get info
        /// about it, this will hold the error code.
        /// </summary>
        public SoundIoError probe_error; 
    }
}
