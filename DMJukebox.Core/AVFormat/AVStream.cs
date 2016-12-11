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
    /// This is a C# implementation of the AVStream struct in FFmpeg.
    /// It represents a single audio or video stream within a media file,
    /// so it gets used by <see cref="AudioTrack"/> while loading audio
    /// tracks.
    /// </summary>
    /// <remarks>
    /// This struct is defined in avformat.h of the libavformat project.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct AVStream
    {
        /// <summary>
        /// The index of this stream in the parent <see cref="AVFormatContext"/>
        /// </summary>
        public int index;
        
        /// <summary>
        /// Unique ID for the stream
        /// </summary>
        public int id;
        
        /// <summary>
        /// (<see cref="AVCodecContext"/>*) The context for the stream's codec
        /// </summary>
        public IntPtr codec;

        public IntPtr priv_data;
        
        public AVFrac pts;
        
        /// <summary>
        /// The unit of time (in seconds) that describes the base unit to use
        /// in each of the timestamps from this stream, as well as packets and
        /// frames from it.
        /// </summary>
        public AVRational time_base;
        
        /// <summary>
        /// The timestamp of the stream's first frame
        /// </summary>
        public long start_time;
        
        /// <summary>
        /// The length of the stream, in <see cref="time_base"/> units.
        /// </summary>
        public long duration;
        
        /// <summary>
        /// The number of frames contained within the stream, or 0 if not specified.
        /// </summary>
        public long nb_frames;

        public AV_DISPOSITION disposition;
        
        public AVDiscard discard;
        
        public AVRational sample_aspect_ratio;

        public IntPtr metadata;
        
        public AVRational avg_frame_rate;
        
        public AVPacket attached_pic;
        
        public IntPtr side_data;

        public int nb_side_data;
        
        public AVSTREAM_EVENT_FLAG event_flags;

        // The rest of the fields are supposed to be internal to libavformat, so they aren't included here.
    }
}
