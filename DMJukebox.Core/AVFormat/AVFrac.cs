/* 
 * This file contains a C# implementation of the AVFrac struct
 * as defined in avformat.h of the libavformat project, for interop use.
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
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct AVFrac
    {
        public long val;

        public long num;

        public long den;
    }
}
