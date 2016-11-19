/* 
 * This file contains a C# implementation of the AVDiscard enum
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
    /// <summary>
    /// @ingroup lavc_decoding
    /// We leave some space between them for extensions (drop some
    /// keyframes for intra-only or drop just some bidir frames).
    /// </summary>
    internal enum AVDiscard
    {
        /// <summary>
        /// discard nothing
        /// </summary>
        AVDISCARD_NONE = -16,

        /// <summary>
        /// discard useless packets like 0 size packets in avi
        /// </summary>
        AVDISCARD_DEFAULT = 0,

        /// <summary>
        /// discard all non reference
        /// </summary>
        AVDISCARD_NONREF = 8,

        /// <summary>
        /// discard all bidirectional frames
        /// </summary>
        AVDISCARD_BIDIR = 16,

        /// <summary>
        /// discard all non intra frames
        /// </summary>
        AVDISCARD_NONINTRA = 24,

        /// <summary>
        /// discard all frames except keyframes
        /// </summary>
        AVDISCARD_NONKEY = 32,

        /// <summary>
        /// discard all
        /// </summary>
        AVDISCARD_ALL = 48
    }
}
