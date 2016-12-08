/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

using System;
using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a C# implementation of the AVPacket struct in FFmpeg.
    /// This is one of the key ones that gets used a lot: while reading
    /// from an audio track, FFmpeg breaks the compressed data into discrete
    /// chunks. AVPacket represents those chunks. Each packet can contain
    /// one or multiple compressed audio frames.
    /// </summary>
    /// <remarks>
    /// This struct is defined in avcodec.h of the libavcodec project.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
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
