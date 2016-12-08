/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

using System;
using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int CallbackDelegate(IntPtr Opaque);

    /// <summary>
    /// This is a C# implementation of the AVIOInterruptCB struct in FFmpeg.
    /// It's used when you want to implement a custom callback that tells
    /// the muxer or demuxer to abort during blocking functions.
    /// </summary>
    /// <remarks>
    /// This struct is defined in avformat.h of the libavformat project.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct AVIOInterruptCB
    {
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public CallbackDelegate callback;

        public IntPtr opaque;
    }

}
