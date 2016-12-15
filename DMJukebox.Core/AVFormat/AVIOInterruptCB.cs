/* ========================================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * ====================================================================== */

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
