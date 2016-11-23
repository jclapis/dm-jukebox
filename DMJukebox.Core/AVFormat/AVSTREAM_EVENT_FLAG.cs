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
    internal enum AVSTREAM_EVENT_FLAG
    {
        AVSTREAM_EVENT_FLAG_METADATA_UPDATED = 0x0001
    }
}
