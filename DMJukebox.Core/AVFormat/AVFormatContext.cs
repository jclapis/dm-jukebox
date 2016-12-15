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
    internal delegate int av_format_control_message(IntPtr s, int type, IntPtr data, UIntPtr data_size);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate int io_open_Delegate(IntPtr s, ref IntPtr pb, string url, int flags, ref IntPtr options);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void io_close_Delegate(IntPtr s, IntPtr pb);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate int open_cb_Delegate(IntPtr s, ref IntPtr p, string url, int flags, IntPtr int_cb, ref IntPtr options);

    /// <summary>
    /// This is a C# implementation of the AVFormatContext struct in FFmpeg.
    /// This is one of the main structs that I use - it represents a media
    /// file that FFmpeg was able to open and read, so each <see cref="AudioTrack"/>
    /// has one under the hood.
    /// </summary>
    /// <remarks>
    /// This struct is defined in avformat.h of the libavformat project.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct AVFormatContext
    {
        /// <summary>
        /// (<see cref="AVClass"/>*) A class that holds logging and
        /// other options.
        /// </summary>
        public IntPtr av_class;
        
        /// <summary>
        /// (<see cref="AVInputFormat"/>*) The input container format
        /// </summary>
        public IntPtr iformat;
        
        /// <summary>
        /// (<see cref="AVOutputFormat"/>*) The output container format
        /// </summary>
        public IntPtr oformat;
        
        /// <summary>
        /// Private data used internally.
        /// </summary>
        public IntPtr priv_data;
        
        /// <summary>
        /// (<see cref="AVIOContext"/>*) The context for I/O operations
        /// </summary>
        public IntPtr pb;

        /// <summary>
        /// A set of AVFMTCTX flags describing some of the common stream
        /// properties. I haven't implemented the enum because this field
        /// probably won't be used.
        /// </summary>
        public int ctx_flags;
        
        /// <summary>
        /// The number of streams contained within the file
        /// </summary>
        public uint nb_streams;
        
        /// <summary>
        /// (<see cref="AVStream"/>**) An array of stream pointers for the
        /// streams contained in the file
        /// </summary>
        public IntPtr streams;
        
        /// <summary>
        /// The name for this file
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string filename;
        
        // This is basically all I need so I'm not going to document the rest
        // of the struct until there's some reason to do so or I have time.

        public long start_time;
        
        public long duration;
        
        public long bit_rate;
        
        public uint packet_size;

        public int max_delay;
        
        public AVFMT_FLAG flags;
        
        public long probesize;
        
        public long max_analyze_duration;

        public IntPtr key;

        public int keylen;

        public uint nb_programs;

        public IntPtr programs;
        
        public AVCodecID video_codec_id;
        
        public AVCodecID audio_codec_id;
        
        public AVCodecID subtitle_codec_id;
        
        public uint max_index_size;
        
        public uint max_picture_buffer;
        
        public uint nb_chapters;

        public IntPtr chapters;
        
        public IntPtr metadata;
        
        public long start_time_realtime;
        
        public int fps_probe_size;
        
        public int error_recognition;
        
        public AVIOInterruptCB interrupt_callback;
        
        public int debug;
        
        public long max_interleave_delta;
        
        public int strict_std_compliance;
        
        public int event_flags;
        
        public int max_ts_probe;
        
        public AVFMT_AVOID_NEG_TS avoid_negative_ts;
        
        public int ts_id;
        
        public int audio_preload;
        
        public int max_chunk_duration;
        
        public int max_chunk_size;
        
        public int use_wallclock_as_timestamps;
        
        public int avio_flags;
        
        public AVDurationEstimationMethod duration_estimation_method;
        
        public long skip_initial_bytes;
        
        public uint correct_ts_overflow;
        
        public int seek2any;
        
        public int flush_packets;
        
        public int probe_score;
        
        public int format_probesize;
        
        public string codec_whitelist;
        
        public string format_whitelist;
        
        public IntPtr @internal;
        
        public int io_repositioned;
        
        public IntPtr video_codec;
        
        public IntPtr audio_codec;
        
        public IntPtr subtitle_codec;
        
        public IntPtr data_codec;
        
        public int metadata_header_padding;
        
        public IntPtr opaque;
        
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public av_format_control_message control_message_cb;
        
        public long output_ts_offset;
        
        public IntPtr dump_separator;
        
        public AVCodecID data_codec_id;
        
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public open_cb_Delegate open_cb;
        
        public string protocol_whitelist;
        
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public io_open_Delegate io_open;
        
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public io_close_Delegate io_close;
        
        public string protocol_blacklist;
    }

}
