/* 
 * This file contains a C# implementation of the SoundIo struct
 * as defined in soundio.h of the libsoundio project, for interop use.
 * 
 * For more information, please see the documentation at
 * http://libsound.io/doc-1.1.0/soundio_8h.html or the source code at
 * https://github.com/andrewrk/libsoundio.
 * 
 * Copyright (c) 2016 Joe Clapis.
 */

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
