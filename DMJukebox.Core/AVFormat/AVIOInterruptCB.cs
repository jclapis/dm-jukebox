/* 
 * This file contains a C# implementation of the AVIOInterruptCB struct
 * as defined in avio.h of the libavformat project, for interop use.
 * 
 * For more information, please see the documentation at
 * https://www.ffmpeg.org/doxygen/trunk/index.html or the source code at
 * https://github.com/FFmpeg/FFmpeg.
 * 
 * Copyright (c) 2016 Joe Clapis.
 */

using System;
using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int CallbackDelegate(IntPtr Opaque);
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct AVIOInterruptCB
    {
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public CallbackDelegate callback;

        public IntPtr opaque;
    }

}
