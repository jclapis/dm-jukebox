/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a C# implementation of the AVFrac struct in FFmpeg.
    /// It's used all over the place in there for fractional numbers.
    /// It's more accurate to represent them this way instead of floats
    /// or doubles. The represented value is: <see cref="val"/> +
    /// <see cref="num"/> / <see cref="den"/>.
    /// </summary>
    /// <remarks>
    /// This struct is defined in avformat.h of the libavformat project.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct AVFrac
    {
        public long val;

        public long num;

        public long den;
    }
}
