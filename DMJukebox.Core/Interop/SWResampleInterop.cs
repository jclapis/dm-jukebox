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
    /// <summary>
    /// This is a utility class that holds the P/Invoke wrappers for libswresample.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    internal static class SWResampleInterop
    {
        /// <summary>
        /// The DLL for Windows
        /// </summary>
        private const string WindowsSWResampleLibrary = "swresample-2.dll";

        /// <summary>
        /// The SO for Linux
        /// </summary>
        private const string LinuxSWResampleLibrary = "swresample-2.so";

        /// <summary>
        /// The Dylib for OSX
        /// </summary>
        private const string MacSWResampleLibrary = "swresample-2.dylib";

        // These regions contain the DllImport function definitions for each OS. Since we can't really set
        // the path of DllImport dynamically (and loading them dynamically using LoadLibrary / dlopen is complicated
        // to manage cross-platform), we have to pre-define them based on the names of the libraries above.

        #region Windows Functions

        [DllImport(WindowsSWResampleLibrary, EntryPoint = nameof(swr_alloc), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr swr_alloc_windows();

        [DllImport(WindowsSWResampleLibrary, EntryPoint = nameof(swr_free), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void swr_free_windows(ref IntPtr s);

        [DllImport(WindowsSWResampleLibrary, EntryPoint = nameof(swr_config_frame), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR swr_config_frame_windows(IntPtr swr, IntPtr @out, IntPtr @in);

        [DllImport(WindowsSWResampleLibrary, EntryPoint = nameof(swr_convert_frame), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR swr_convert_frame_windows(IntPtr swr, IntPtr output, IntPtr input);

        [DllImport(WindowsSWResampleLibrary, EntryPoint = nameof(swr_get_delay), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern long swr_get_delay_windows(IntPtr s, long @base);

        [DllImport(WindowsSWResampleLibrary, EntryPoint = nameof(swr_init), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR swr_init_windows(IntPtr s);

        #endregion

        #region Linux Functions

        [DllImport(LinuxSWResampleLibrary, EntryPoint = nameof(swr_alloc), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr swr_alloc_linux();

        [DllImport(LinuxSWResampleLibrary, EntryPoint = nameof(swr_free), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void swr_free_linux(ref IntPtr s);

        [DllImport(LinuxSWResampleLibrary, EntryPoint = nameof(swr_config_frame), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR swr_config_frame_linux(IntPtr swr, IntPtr @out, IntPtr @in);

        [DllImport(LinuxSWResampleLibrary, EntryPoint = nameof(swr_convert_frame), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR swr_convert_frame_linux(IntPtr swr, IntPtr output, IntPtr input);

        [DllImport(LinuxSWResampleLibrary, EntryPoint = nameof(swr_get_delay), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern long swr_get_delay_linux(IntPtr s, long @base);

        [DllImport(LinuxSWResampleLibrary, EntryPoint = nameof(swr_init), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR swr_init_linux(IntPtr s);

        #endregion

        #region OSX Functions

        [DllImport(MacSWResampleLibrary, EntryPoint = nameof(swr_alloc), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr swr_alloc_osx();

        [DllImport(MacSWResampleLibrary, EntryPoint = nameof(swr_free), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void swr_free_osx(ref IntPtr s);

        [DllImport(MacSWResampleLibrary, EntryPoint = nameof(swr_config_frame), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR swr_config_frame_osx(IntPtr swr, IntPtr @out, IntPtr @in);

        [DllImport(MacSWResampleLibrary, EntryPoint = nameof(swr_convert_frame), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR swr_convert_frame_osx(IntPtr swr, IntPtr output, IntPtr input);

        [DllImport(MacSWResampleLibrary, EntryPoint = nameof(swr_get_delay), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern long swr_get_delay_osx(IntPtr s, long @base);

        [DllImport(MacSWResampleLibrary, EntryPoint = nameof(swr_init), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR swr_init_osx(IntPtr s);

        #endregion

        #region Delegates and Platform-Dependent Loading

        // These delegates all represent the function signatures for the libswresample methods I need to call.

        private delegate IntPtr swr_alloc_delegate();
        private delegate void swr_free_delegate(ref IntPtr s);
        private delegate AVERROR swr_config_frame_delegate(IntPtr swr, IntPtr @out, IntPtr @in);
        private delegate AVERROR swr_convert_frame_delegate(IntPtr swr, IntPtr output, IntPtr input);
        private delegate long swr_get_delay_delegate(IntPtr s, long @base);
        private delegate AVERROR swr_init_delegate(IntPtr s);

        // These fields represent function pointers towards each of the extern functions. They get set
        // to the proper platform-specific functions by the static constructor. For example, if this is
        // running on a Windows machine, each of these pointers will point to the various XXX_windows
        // extern functions listed above.

        private static swr_alloc_delegate swr_alloc_impl;
        private static swr_free_delegate swr_free_impl;
        private static swr_config_frame_delegate swr_config_frame_impl;
        private static swr_convert_frame_delegate swr_convert_frame_impl;
        private static swr_get_delay_delegate swr_get_delay_impl;
        private static swr_init_delegate swr_init_impl;

        /// <summary>
        /// The static constructor figures out which library to use for P/Invoke based
        /// on the current OS platform.
        /// </summary>
        static SWResampleInterop()
        {
            NativePathFinder.AddNativeLibraryPathToEnvironmentVariable();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                swr_alloc_impl = swr_alloc_windows;
                swr_free_impl = swr_free_windows;
                swr_config_frame_impl = swr_config_frame_windows;
                swr_convert_frame_impl = swr_convert_frame_windows;
                swr_get_delay_impl = swr_get_delay_windows;
                swr_init_impl = swr_init_windows;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                swr_alloc_impl = swr_alloc_linux;
                swr_free_impl = swr_free_linux;
                swr_config_frame_impl = swr_config_frame_linux;
                swr_convert_frame_impl = swr_convert_frame_linux;
                swr_get_delay_impl = swr_get_delay_linux;
                swr_init_impl = swr_init_linux;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                swr_alloc_impl = swr_alloc_osx;
                swr_free_impl = swr_free_osx;
                swr_config_frame_impl = swr_config_frame_osx;
                swr_convert_frame_impl = swr_convert_frame_osx;
                swr_get_delay_impl = swr_get_delay_osx;
                swr_init_impl = swr_init_osx;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region Public API
        
        /// <summary>
        /// Allocates a new resample context.
        /// </summary>
        /// <returns>An opaque pointer to the resample context</returns>
        public static IntPtr swr_alloc()
        {
            return swr_alloc_impl();
        }
        
        /// <summary>
        /// Frees a resample context.
        /// </summary>
        /// <param name="s">The resample context to free. The pointer will be set to
        /// <see cref="IntPtr.Zero"/>.</param>
        public static void swr_free(ref IntPtr s)
        {
            swr_free_impl(ref s);
        }

        /// <summary>
        /// Configures the given resample context using the parameters defined within
        /// the provided input and output <see cref="AVFrame"/> arguments.
        /// </summary>
        /// <param name="swr">The resample context to use</param>
        /// <param name="out">(<see cref="AVFrame"/>*) The output frame, with the output
        /// conversion settings defined</param>
        /// <param name="in">(<see cref="AVFrame"/>*) The input frame, with the input
        /// conversion settings defined</param>
        /// <returns><see cref="AVERROR.AVERROR_SUCCESS"/> on a success, or an error code 
        /// on a failure.</returns>
        public static AVERROR swr_config_frame(IntPtr swr, IntPtr @out, IntPtr @in)
        {
            return swr_config_frame_impl(swr, @out, @in);
        }

        /// <summary>
        /// Converts data from the input <see cref="AVFrame"/> into the target format,
        /// and stores it in the output frame. This might not perform the whole
        /// conversion at once; it's possible that you'll have to call this function
        /// a few more times with <see cref="IntPtr.Zero"/> as the input in order to
        /// get all of the converted output before calling it again with a new input
        /// frame.
        /// </summary>
        /// <param name="swr">The resample context to use</param>
        /// <param name="output">(<see cref="AVFrame"/>*) The output frame to receive
        /// the converted data</param>
        /// <param name="input">(<see cref="AVFrame"/>*) The input frame with the raw
        /// data to be converted</param>
        /// <returns><see cref="AVERROR.AVERROR_SUCCESS"/> on a success, or an error code 
        /// on a failure.</returns>
        public static AVERROR swr_convert_frame(IntPtr swr, IntPtr output, IntPtr input)
        {
            return swr_convert_frame_impl(swr, output, input);
        }
        
        /// <summary>
        /// Returns the expected delay / additional overhead that the converter will
        /// produce when converting between the target input format and the target
        /// output format. The delay will be in 1 / <paramref name="base"/> units.
        /// </summary>
        /// <param name="s">The resample context to use</param>
        /// <param name="base">The units to return the delay in. This can be:
        /// 1 (for seconds), 1000 (for milliseconds), the input sample rate
        /// (for number of input samples), the output sample rate (for number of
        /// output samples), or the least common multiple of the input and output
        /// sample rates.</param>
        /// <returns>The delay caused by the conversion,
        /// in 1 / <paramref name="base"/> units.</returns>
        public static long swr_get_delay(IntPtr s, long @base)
        {
            return swr_get_delay_impl(s, @base);
        }

        /// <summary>
        /// Initializes the resample context once the parameters have been set,
        /// preparing it for conversion.
        /// </summary>
        /// <param name="s">The resample context to use</param>
        /// <returns><see cref="AVERROR.AVERROR_SUCCESS"/> on a success, or an error code 
        /// on a failure.</returns>
        public static AVERROR swr_init(IntPtr s)
        {
            return swr_init_impl(s);
        }

        #endregion
    }
}
