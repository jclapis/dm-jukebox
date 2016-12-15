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
        /// <summary>
        /// The buffer for audio channel 0
        /// </summary>
        /// <remarks>
        /// This is supposed to be a fixed size array with 8 elements, but C# doesn't
        /// support fixed buffers of pointer types, so we have to split it up into
        /// individual fields.
        /// </remarks>
        public byte* data0;

        /// <summary>
        /// The buffer for audio channel 1
        /// </summary>
        public byte* data1;

        /// <summary>
        /// The buffer for audio channel 2
        /// </summary>
        public byte* data2;

        /// <summary>
        /// The buffer for audio channel 3
        /// </summary>
        public byte* data3;

        /// <summary>
        /// The buffer for audio channel 4
        /// </summary>
        public byte* data4;

        /// <summary>
        /// The buffer for audio channel 5
        /// </summary>
        public byte* data5;

        /// <summary>
        /// The buffer for audio channel 6
        /// </summary>
        public byte* data6;

        /// <summary>
        /// The buffer for audio channel 7
        /// </summary>
        public byte* data7;
        
        /// <summary>
        /// The size of the audio channel buffers, in bytes,
        /// is stored in linesize[0] here.
        /// </summary>
        public fixed int linesize[8];
        
        /// <summary>
        /// For planar audio with more than 8 channels, the buffers for
        /// the extra channels will be stored here.
        /// </summary>
        public byte** extended_data;
        
        /// <summary>
        /// The width of the video data (not used)
        /// </summary>
        public int width;
        
        /// <summary>
        /// The height of the video data (not used)
        /// </summary>
        public int height;
        
        /// <summary>
        /// The number of audio samples per channel contained in this frame
        /// </summary>
        public int nb_samples;
        
        /// <summary>
        /// The binary format for the audio samples in this frame
        /// </summary>
        public AVSampleFormat format;
        
        /// <summary>
        /// True if this is a keyframe, false if it isn't.
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public bool key_frame;
        
        /// <summary>
        /// The picture type of the video frame (not used)
        /// </summary>
        public AVPictureType pict_type;
        
        /// <summary>
        /// The aspect ratio of the video frame (not used)
        /// </summary>
        public AVRational sample_aspect_ratio;
        
        /// <summary>
        /// The timestamp (in <see cref="AVCodecContext.time_base"/> units)
        /// where this sample is located
        /// </summary>
        public long pts;
        
        /// <summary>
        /// The timestamp from the <see cref="AVPacket.pts"/> field of the packet
        /// that produced this frame
        /// </summary>
        public long pkt_pts;
        
        /// <summary>
        /// The decompression timestamp from the <see cref="AVPacket.dts"/> field
        /// of the packet that produced this frame
        /// </summary>
        public long pkt_dts;
        
        /// <summary>
        /// The index of the picture from within the raw bitstream (not used)
        /// </summary>
        public int coded_picture_number;
        
        /// <summary>
        /// The index of the picture in presentation order (the order shown to
        /// the user, which might be different from the bitstream order) (not used)
        /// </summary>
        public int display_picture_number;
        
        /// <summary>
        /// The quality of the picture, between 1 and 32767. Lower is better. (not used)
        /// </summary>
        public int quality;
        
        /// <summary>
        /// Private user-specified data
        /// </summary>
        public void* opaque;
        
        /// <summary>
        /// Apparently this is some deprecated field that isn't used anymore.
        /// </summary>
        public fixed ulong error[8];
        
        /// <summary>
        /// Indicates the amount of time to stay on this video frame without
        /// switching to the next one (not used)
        /// </summary>
        public int repeat_pict;
        
        /// <summary>
        /// Determines whether or not the picture is interlaced. I think this should
        /// probably be a bool? (not used)
        /// </summary>
        public int interlaced_frame;
        
        /// <summary>
        /// Determines whether or not the top field is displayed first in
        /// interlaced video. I think this should probably be a bool? (not used)
        /// </summary>
        public int top_field_first;
        
        /// <summary>
        /// Determines whether or not to tell the user of a palette swap if the
        /// new palette is different from the previous frame. I think this should
        /// probably be a bool? (not used)
        /// </summary>
        public int palette_has_changed;
        
        /// <summary>
        /// This has something to do with reordering video frames (not used)
        /// </summary>
        public long reordered_opaque;
        
        /// <summary>
        /// The sample rate of the audio data, in Hz.
        /// </summary>
        public int sample_rate;
        
        /// <summary>
        /// The audio data's channel layout
        /// </summary>
        public AV_CH_LAYOUT channel_layout;

        /// <summary>
        /// The AVBufferRef* for data0.
        /// </summary>
        /// <remarks>
        /// This is supposed to be a fixed size array with 8 elements, but C# doesn't
        /// support fixed buffers of pointer types, so we have to split it up into
        /// individual fields.
        /// Also, this should be an AVBufferRef* instead of a void* but I don't need
        /// to use that type, so I haven't implemented it.
        /// </remarks>
        public void* buf0;

        /// <summary>
        /// The AVBufferRef* for data1.
        /// </summary>
        public void* buf1;

        /// <summary>
        /// The AVBufferRef* for data2.
        /// </summary>
        public void* buf2;

        /// <summary>
        /// The AVBufferRef* for data3.
        /// </summary>
        public void* buf3;

        /// <summary>
        /// The AVBufferRef* for data4.
        /// </summary>
        public void* buf4;

        /// <summary>
        /// The AVBufferRef* for data5.
        /// </summary>
        public void* buf5;

        /// <summary>
        /// The AVBufferRef* for data6.
        /// </summary>
        public void* buf6;

        /// <summary>
        /// The AVBufferRef* for data7.
        /// </summary>
        public void* buf7;

        /// <summary>
        /// If there are more than 8 audio channels, this array will store
        /// AVBufferRef pointers to the buffers for each data channel in
        /// <see cref="extended_data"/>.
        /// </summary>
        public void** extended_buf;
        
        /// <summary>
        /// The number of buffers in <see cref="extended_data"/>
        /// </summary>
        public int nb_extended_buf;

        /// <summary>
        /// No idea what this does, but the pointer is the unimplemented
        /// AVFrameSideData struct.
        /// </summary>
        public void** side_data;

        /// <summary>
        /// The number of elements in <see cref="side_data"/>
        /// </summary>
        public int nb_side_data;
        
        /// <summary>
        /// Flags that describe some of this frame's extra decoding info
        /// </summary>
        public AV_FRAME_FLAGS flags;

        // The rest of the fields are supposed to be hidden from external
        // users, so I haven't included them here.
    }
}
