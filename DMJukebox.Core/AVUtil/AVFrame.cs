/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a C# implementation of the AVFrame struct in FFmpeg.
    /// This is one of the main ones I use. It holds a single frame of raw, decoded
    /// audio ready for post-processing and playback.
    /// </summary>
    /// <remarks>
    /// This is one of the few unsafe structs, because for performance reasons I need
    /// to access it directly from unmanaged memory instead of having to copy it over
    /// to managed space every time it gets refreshed.
    /// 
    /// This struct is defined in frame.h of the libavutil project.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    unsafe internal struct AVFrame
    {
        /// <remarks>
        /// This is supposed to be a fixed size array with 8 elements, but C# doesn't
        /// support fixed buffers of pointer types, so we have to split it up into
        /// individual fields.
        /// </remarks>
        public byte* data0;
        public byte* data1;
        public byte* data2;
        public byte* data3;
        public byte* data4;
        public byte* data5;
        public byte* data6;
        public byte* data7;
        
        public fixed int linesize[8];
        
        public byte** extended_data;
        
        public int width;
        
        public int height;
        
        public int nb_samples;
        
        public AVSampleFormat format;
        
        public int key_frame;
        
        public AVPictureType pict_type;
        
        public AVRational sample_aspect_ratio;
        
        public long pts;
        
        public long pkt_pts;
        
        public long pkt_dts;
        
        public int coded_picture_number;
        
        public int display_picture_number;
        
        public int quality;
        
        public void* opaque;
        
        public fixed ulong error[8];
        
        public int repeat_pict;
        
        public int interlaced_frame;
        
        public int top_field_first;
        
        public int palette_has_changed;
        
        public long reordered_opaque;
        
        public int sample_rate;
        
        public AV_CH_LAYOUT channel_layout;
        
        /// <remarks>
        /// This is supposed to be a fixed size array with 8 elements, but C# doesn't
        /// support fixed buffers of pointer types, so we have to split it up into
        /// individual fields.
        /// Also, this should be an AVBufferRef* instead of a void* but I don't need
        /// to use that type, so I haven't implemented it.
        /// </remarks>
        public void* buf0;
        public void* buf1;
        public void* buf2;
        public void* buf3;
        public void* buf4;
        public void* buf5;
        public void* buf6;
        public void* buf7;
        
        public void** extended_buf;
        
        public int nb_extended_buf;

        public void** side_data;

        public int nb_side_data;
        
        public int flags;

        // The rest of the fields are supposed to be hidden from external
        // users, so I haven't included them here.
    }
}
