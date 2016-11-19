/* 
 * This file contains a C# implementation of the AVFMT_AVOID_NEG_TS enum
 * as defined in avformat.h of the libavformat project, for interop use.
 * It isn't technically an enum in ffmpeg, just a bunch of macros.
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
    internal enum AVFMT_AVOID_NEG_TS
    {
        /// <summary>
        /// Enabled when required by target format
        /// </summary>
        AUTO = -1,

        /// <summary>
        /// Shift timestamps so they are non negative
        /// </summary>
        MAKE_NON_NEGATIVE = 1,

        /// <summary>
        /// Shift timestamps so that they start at 0
        /// </summary>
        MAKE_ZERO = 2
    }

}
