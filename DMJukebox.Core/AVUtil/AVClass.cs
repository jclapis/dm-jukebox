/* 
 * This file contains a C# implementation of the AVClass struct
 * as defined in log.h of the libavutil project, for interop use.
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
