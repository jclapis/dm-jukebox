/* 
 * This file contains a C# implementation of the AVFrac struct
 * as defined in avformat.h of the libavformat project, for interop use.
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
    /// fractional numbers for exact pts handling
    /// The exact value of the fractional number is: 'val + num / den'.
    /// num is assumed to be 0 <= num<den.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct AVFrac
    {
        public long val;

        public long num;

        public long den;
    }
}
