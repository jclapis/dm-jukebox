/* 
 * This file contains C# wrappers for some of the functions exported by Msvcrt.
 * 
 * This is used for the logging callback passed into libavutil if it gets used - but
 * this is still a work in progress and isn't fully cross platform yet.
 * 
 * Copyright (c) 2016 Joe Clapis.
 */

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DMJukebox.Interop
{
    /// <summary>
    /// This utility class holds P/Invoke wrappers for Msvcrt functions.
    /// </summary>
    public static class MsvcrtInterop
    {
        private const string MsvcrtDll = "Msvcrt.dll";

        [DllImport(MsvcrtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int vsprintf(StringBuilder buffer, string format, IntPtr args);

        [DllImport(MsvcrtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int _vscprintf(string format, IntPtr ptr);

        [DllImport(MsvcrtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr memcpy(IntPtr dest, IntPtr src, IntPtr count);
    }
}
