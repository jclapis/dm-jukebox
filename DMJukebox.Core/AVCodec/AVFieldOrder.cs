/* 
 * This file contains a C# implementation of the AVFieldOrder enum
 * as defined in avcodec.h of the libavcodec project, for interop use.
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
    internal enum AVFieldOrder
    {
        AV_FIELD_UNKNOWN,
        AV_FIELD_PROGRESSIVE,

        /// <summary>
        /// Top coded_first, top displayed first
        /// </summary>
        AV_FIELD_TT,

        /// <summary>
        /// Bottom coded first, bottom displayed first
        /// </summary>
        AV_FIELD_BB,

        /// <summary>
        /// Top coded first, bottom displayed first
        /// </summary>
        AV_FIELD_TB,

        /// <summary>
        /// Bottom coded first, top displayed first
        /// </summary>
        AV_FIELD_BT
    }
}
