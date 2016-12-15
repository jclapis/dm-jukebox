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
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void on_devices_change(IntPtr soundio);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void on_backend_disconnect(IntPtr soundio, int err);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void on_events_signal(IntPtr soundio);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void emit_rtprio_warning();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void jack_info_callback(
        [MarshalAs(UnmanagedType.LPStr)] string msg);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void jack_error_callback(
        [MarshalAs(UnmanagedType.LPStr)] string msg);

    /// <summary>
    /// This is a C# implementation of the SoundIo struct in libsoundio.
    /// It holds the state/context for working with the library.
    /// </summary>
    /// <remarks>
    /// This struct is defined in soundio.h of libsoundio.
    /// For more information, please see the documentation at
    /// http://libsound.io/doc-1.1.0/soundio_8h.html
    /// or the source code at https://github.com/andrewrk/libsoundio.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct SoundIo
    {
        public IntPtr userdata;
        
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public on_devices_change on_devices_change;
        
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public on_backend_disconnect on_backend_disconnect;
        
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public on_events_signal on_events_signal;
        
        public SoundIoBackend current_backend;
        
        public string app_name;
        
        public emit_rtprio_warning emit_rtprio_warning;
        
        public jack_info_callback jack_info_callback;
        
        public jack_error_callback jack_error_callback;
    }
}
