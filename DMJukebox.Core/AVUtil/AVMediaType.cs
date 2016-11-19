/* 
 * This file contains a C# implementation of the AVMediaType enum
 * as defined in avutil.h of the libavutil project, for interop use.
 * 
 * The documentation and comments have been largely copied from those headers and
 * are not my own work - they are the work of the contributors to ffmpeg.
 * Credit goes to them. I may have modified them in places where it made sense
 * to help document the C# bindings.
 * 
 * For more information, please see the documentation at
 * https://www.ffmpeg.org/doxygen/trunk/index.html or the source code at
 * https://github.com/FFmpeg/FFmpeg.
 * 
 * Copyright (c) 2016 Joe Clapis.
 */

namespace DMJukebox.Interop
{
    internal enum AVMediaType
    {
        /// <summary>
        /// Usually treated as AVMEDIA_TYPE_DATA
        /// </summary>
        AVMEDIA_TYPE_UNKNOWN = -1,

        AVMEDIA_TYPE_VIDEO,

        AVMEDIA_TYPE_AUDIO,

        /// <summary>
        /// Opaque data information usually continuous
        /// </summary>
        AVMEDIA_TYPE_DATA,

        AVMEDIA_TYPE_SUBTITLE,

        /// <summary>
        /// Opaque data information usually sparse
        /// </summary>
        AVMEDIA_TYPE_ATTACHMENT,

        AVMEDIA_TYPE_NB
    }
}
