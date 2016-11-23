/* 
 * This file contains a C# implementation of the SoundIoOutStream struct
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
    internal delegate void write_callback(IntPtr outstream, int frame_count_min, int frame_count_max);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void underflow_callback(IntPtr outstream);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void error_callback(IntPtr outstream, SoundIoError err);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct SoundIoOutStream
    {
        public IntPtr device;
        
        public SoundIoFormat format;
        
        public int sample_rate;
        
        public SoundIoChannelLayout layout;
        
        public double software_latency;
        
        public IntPtr userdata;
        
        public write_callback write_callback;
        
        public underflow_callback underflow_callback;
        
        public error_callback error_callback;
        
        public string name;
        
        public bool non_terminal_hint;
        
        public int bytes_per_frame;
        
        public int bytes_per_sample;
        
        public SoundIoError layout_error;
    }
}
