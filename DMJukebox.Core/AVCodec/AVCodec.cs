/* 
 * This file contains a C# implementation of the AVCodec struct
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
    internal struct AVCodec
    {
        public string name;

        public string long_name;

        public AVMediaType type;

        public AVCodecID id;
        
        public AV_CODEC_CAP capabilities;
        
        public IntPtr supported_framerates;
        
        public IntPtr pix_fmts;
        
        public IntPtr supported_samplerates;
        
        public IntPtr sample_fmts;
        
        public IntPtr channel_layouts;
        
        public byte max_lowres;
        
        public IntPtr priv_class;
        
        public IntPtr profiles;
    }

}
