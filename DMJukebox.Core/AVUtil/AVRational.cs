/* 
 * This file contains a C# implementation of the AVRational struct
 * as defined in rational.h of the libavutil project, for interop use.
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

using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    /// <summary>
    /// rational number numerator/denominator
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct AVRational
    {
        /// <summary>
        /// numerator
        /// </summary>
        public int num;

        /// <summary>
        /// denominator
        /// </summary>
        public int den;
    }
}
