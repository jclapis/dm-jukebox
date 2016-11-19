/* 
 * This file contains a C# implementation of the AVIOInterruptCB struct
 * as defined in avio.h of the libavformat project, for interop use.
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

using System;
using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int CallbackDelegate(IntPtr Opaque);

    /// <summary>
    /// Callback for checking whether to abort blocking functions.
    /// AVERROR_EXIT is returned in this case by the interrupted
    /// function. During blocking operations, callback is called with
    /// opaque as parameter.If the callback returns 1, the
    /// blocking operation will be aborted.
    /// No members can be added to this struct without a major bump, if
    /// new elements have been added after this struct in AVFormatContext
    /// or AVIOContext.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct AVIOInterruptCB
    {
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public CallbackDelegate callback;

        public IntPtr opaque;
    }

}
