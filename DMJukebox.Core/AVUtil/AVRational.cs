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
    /// This is a C# implementation of the AVRational struct in FFmpeg.
    /// It represents a rational (fractional) number as a discrete
    /// numerator and denominator pair to avoid losses in converting to
    /// floats or doubles.
    /// </summary>
    /// <remarks>
    /// This struct is defined in rational.h of the libavutil project.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct AVRational
    {
        public int num;
        
        public int den;
    }
}
