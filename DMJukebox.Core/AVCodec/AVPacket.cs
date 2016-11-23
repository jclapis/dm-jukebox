/* 
 * This file contains a C# implementation of the AVPacket struct
 * as defined in avcodec.h of the libavcodec project, for interop use.
 * 
 * For more information, please see the documentation at
 * https://www.ffmpeg.org/doxygen/trunk/index.html or the source code at
 * https://github.com/FFmpeg/FFmpeg.
 * 
 * Copyright (c) 2016 Joe Clapis.
 */

using System;
using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct AVPacket
    {
        public IntPtr buf;

        public long pts;
        
        public long dts;

        public IntPtr data;

        public int size;

        public int stream_index;
        
        public AV_PKT_FLAG flags;
        
        public IntPtr side_data;

        public int side_data_elems;
        
        public long duration;
        
        public long pos;
        
        public long convergence_duration;
    }
}
