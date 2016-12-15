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
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate string item_name(IntPtr ctx);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate IntPtr child_next(IntPtr obj, IntPtr prev);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate IntPtr child_class_next(IntPtr prev);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate AVClassCategory get_category(IntPtr ctx);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate int query_ranges(ref IntPtr ranges, IntPtr obj, string key, int flags);

    /// <summary>
    /// This is a C# implementation of the AVClass struct in FFmpeg.
    /// I think FFmpeg uses this internally to keep track of what
    /// type a struct is, but I don't use it. The only reason it's
    /// included is because <see cref="AVCodecContext.av_class"/>
    /// points to one of these in case we ever need it for something.
    /// </summary>
    /// <remarks>
    /// This struct is defined in log.h of the libavutil project.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct AVClass
    {
        public string class_name;
        
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public item_name item_name;
        
        public IntPtr option;
        
        public int version;
        
        public int log_level_offset_offset;
        
        public int parent_log_context_offset;
        
        public child_next child_next;
        
        public child_class_next child_class_next;
        
        public AVClassCategory category;
        
        public get_category get_category;
        
        public query_ranges query_ranges;
    }
}
