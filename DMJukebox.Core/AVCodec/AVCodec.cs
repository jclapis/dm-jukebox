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
    /// This is a C# implementation of the AVCodec struct in FFmpeg.
    /// It describes some details about a particular codec. I use
    /// it in <see cref="AudioTrack"/> to get these details for info
    /// about a loaded track.
    /// </summary>
    /// <remarks>
    /// This struct is defined in avcodec.h of the libavcodec project.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct AVCodec
    {
        /// <summary>
        /// The unique name for this codec
        /// </summary>
        public string name;

        /// <summary>
        /// A human-readable version of this codec's name
        /// </summary>
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
