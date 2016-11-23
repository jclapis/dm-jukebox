/* 
 * This file contains a C# implementation of the SoundIoDevice struct
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
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct SoundIoDevice
    {
        public IntPtr soundio;
        
        public string id;
        
        public string name;
        
        public SoundIoDeviceAim aim;
        
        public IntPtr layouts;

        public int layout_count;
        
        public SoundIoChannelLayout current_layout;
        
        public IntPtr formats;
        
        public int format_count;
        
        public SoundIoFormat current_format;
        
        public IntPtr sample_rates;
        
        public int sample_rate_count;
        
        public int sample_rate_current;
        
        public double software_latency_min;
        
        public double software_latency_max;
        
        public double software_latency_current;
        
        public bool is_raw;
        
        public int ref_count;
        
        public SoundIoError probe_error; 
    }
}
