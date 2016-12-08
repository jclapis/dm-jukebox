/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */
 
 /* 
 * This file contains a C# implementation of the AVSTREAM_EVENT_FLAG enum
 * as defined in avformat.h of the libavformat project, for interop use.
 * It isn't technically an enum in ffmpeg, just a bunch of macros.
 * 
 * For more information, please see the documentation at
 * https://www.ffmpeg.org/doxygen/trunk/index.html or the source code at
 * https://github.com/FFmpeg/FFmpeg.
 * 
 * Copyright (c) 2016 Joe Clapis.
 */

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a C# implementation of the AVSTREAM_EVENT_FLAG enum in FFmpeg.
    /// It's used in <see cref="AVStream.event_flags"/> by users that want to
    /// get notified of certain events that trigger to the stream during
    /// processing, but there's only one value in it so this is probably for
    /// future stuff.
    /// </summary>
    /// <remarks>
    /// This enum is defined in avformat.h of the libavformat project.
    /// It isn't technically an enum in FFmpeg, just a bunch of macros.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    internal enum AVSTREAM_EVENT_FLAG
    {
        AVSTREAM_EVENT_FLAG_METADATA_UPDATED = 0x0001
    }
}
