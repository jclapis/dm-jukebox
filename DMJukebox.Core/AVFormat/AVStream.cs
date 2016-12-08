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
        public int index;
        
        public int id;
        
        public IntPtr codec;

        public IntPtr priv_data;
        
        public AVFrac pts;
        
        public AVRational time_base;
        
        public long start_time;
        
        public long duration;
        
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
