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
        /// <summary>
        /// (<see cref="AVBufferRef"/>*) The AV buffer that holds the data
        /// for this packet (not used)
        /// </summary>
        public IntPtr buf;

        /// <summary>
        /// The timestamp where this packet is located in the parent stream,
        /// in <see cref="AVStream.time_base"/> units.
        /// </summary>
        public long pts;
        
        /// <summary>
        /// The decompression timestamp, which defines when this packet should
        /// be decompressed, in <see cref="AVStream.time_base"/> units.
        /// </summary>
        public long dts;

        /// <summary>
        /// (byte*) The data buffer for this packet
        /// </summary>
        public IntPtr data;

        /// <summary>
        /// The size of the data buffer
        /// </summary>
        public int size;

        /// <summary>
        /// The index of the parent <see cref="AVStream"/> within its
        /// <see cref="AVFormatContext"/>'s list of streams
        /// </summary>
        public int stream_index;
        
        public AV_PKT_FLAG flags;
        
        public IntPtr side_data;

        public int side_data_elems;
        
        /// <summary>
        /// The duration of the packet, in <see cref="AVStream.time_base"/>
        /// units.
        /// </summary>
        public long duration;
        
        /// <summary>
        /// The byte position of the packet within the parent
        /// <see cref="AVStream"/>
        /// </summary>
        public long pos;
        
        public long convergence_duration;
    }
}
