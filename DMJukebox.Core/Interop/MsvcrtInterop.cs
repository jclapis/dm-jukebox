/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DMJukebox.Interop
{
    /// <summary>
    /// This utility class holds P/Invoke wrappers for Msvcrt functions.
    /// </summary>
    /// <remarks>
    /// This is obviously not a cross-platform class. I used it during early
    /// development to hook into FFmpeg's logging system and give it a
    /// managed callback so I could see what it was doing internally. Now
    /// that I have a good grasp on it, I don't need to use the logging
    /// functions anymore so this is simply a relic from those times that
    /// I keep around just in case it ever becomes useful again.
    /// 
    /// For more information, take a look at the MSDN documentation
    /// (for exmaple, https://msdn.microsoft.com/en-us/library/w05tbk72.aspx)
    /// </remarks>
    internal static class MsvcrtInterop
    {
        /// <summary>
        /// The DLL for msvcrt
        /// </summary>
        private const string MsvcrtDll = "Msvcrt.dll";

        /// <summary>
        /// This writes formatted output to the specified buffer, using the format string
        /// as the template and filling in its parameter marks with the provided arguments.
        /// </summary>
        /// <param name="buffer">The buffer to write the formatted string to</param>
        /// <param name="format">The input string</param>
        /// <param name="args">(va_list) The list of arguments for the input string</param>
        /// <returns>The number of characters written into the buffer</returns>
        [DllImport(MsvcrtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int vsprintf(StringBuilder buffer, string format, IntPtr args);

        /// <summary>
        /// Returns the number of characters that will be written by a call to
        /// <see cref="vsprintf(StringBuilder, string, IntPtr)"/> if you provide it with
        /// <paramref name="format"/> and <paramref name="argptr"/>. Use this to figure
        /// out the size of the buffer to allocate when calling it.
        /// </summary>
        /// <param name="format">The input string</param>
        /// <param name="argptr">(va_list) The list of arguments for the input string</param>
        /// <returns>The number of characters that <see cref="vsprintf(StringBuilder, string, IntPtr)"/>
        /// will return with these arguments</returns>
        [DllImport(MsvcrtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int _vscprintf(string format, IntPtr argptr);

        /// <summary>
        /// Copies data from one buffer to another.
        /// </summary>
        /// <param name="dest">The target to copy data into</param>
        /// <param name="src">The source to copy data from</param>
        /// <param name="count">The number of bytes to copy</param>
        /// <returns>The same pointer as the <paramref name="dest"/> parameter.</returns>
        [DllImport(MsvcrtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr memcpy(IntPtr dest, IntPtr src, IntPtr count);
    }
}
