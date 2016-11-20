/* 
 * This file contains C# wrappers for some of the functions exported by libswresample.
 * They come from swresample.h. 
 * 
 * The documentation and comments have been largely copied from that header and
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
    /// <summary>
    /// This is a utility class that holds the P/Invoke wrappers for libswresample.
    /// </summary>
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
        // running on a Windows machine, each of these pointers will point to the various swr_XXX_windows
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
        /// Allocate SwrContext.
        /// 
        /// If you use this function you will need to set the parameters
        /// (manually or with swr_alloc_set_opts()) before calling swr_init().
        /// </summary>
        /// <returns>NULL on error, allocated context otherwise</returns>
        public static IntPtr swr_alloc()
        {
            return swr_alloc_impl();
        }

        /// <summary>
        /// Free the given SwrContext and set the pointer to NULL.
        /// </summary>
        /// <param name="s">a pointer to a pointer to Swr context</param>
        public static void swr_free(ref IntPtr s)
        {
            swr_free_impl(ref s);
        }

        /// <summary>
        /// Configure or reconfigure the SwrContext using the information provided by the AVFrames.
        /// 
        /// The original resampling context is reset even on failure.
        /// The function calls swr_close() internally if the context is open.
        /// </summary>
        /// <param name="swr">audio resample context</param>
        /// <param name="out">(AVFrame) output AVFrame</param>
        /// <param name="in">(AVFrame) input AVFrame</param>
        /// <returns>0 on success, AVERROR on failure.</returns>
        public static AVERROR swr_config_frame(IntPtr swr, IntPtr @out, IntPtr @in)
        {
            return swr_config_frame_impl(swr, @out, @in);
        }

        /// <summary>
        /// Convert the samples in the input AVFrame and write them to the output AVFrame.
        /// 
        /// Input and output AVFrames must have channel_layout, sample_rate and format set.
        /// 
        /// If the output AVFrame does not have the data pointers allocated the nb_samples
        /// field will be set using av_frame_get_buffer() is called to allocate the frame.
        /// 
        /// The output AVFrame can be NULL or have fewer allocated samples than required.
        /// In this case, any remaining samples not written to the output will be added to
        /// an internal FIFO buffer, to be returned at the next call to this function or
        /// to swr_convert().
        /// 
        /// If converting sample rate, there may be data remaining in the internal resampling
        /// delay buffer. swr_get_delay() tells the number of remaining samples. To get this
        /// data as output, call this function or swr_convert() with NULL input.
        /// 
        /// If the SwrContext configuration does not match the output and input AVFrame
        /// settings the conversion does not take place and depending on which AVFrame is
        /// not matching AVERROR_OUTPUT_CHANGED, AVERROR_INPUT_CHANGED or the result of a
        /// bitwise-OR of them is returned.
        /// </summary>
        /// <param name="swr">audio resample context</param>
        /// <param name="output">(AVFrame) output AVFrame</param>
        /// <param name="input">(AVFrame) input AVFrame</param>
        /// <returns>0 on success, AVERROR on failure or nonmatching configuration.</returns>
        public static AVERROR swr_convert_frame(IntPtr swr, IntPtr output, IntPtr input)
        {
            return swr_convert_frame_impl(swr, output, input);
        }

        /// <summary>
        /// Gets the delay the next input sample will experience relative to the next output sample.
        ///
        /// Swresample can buffer data if more input has been provided than available
        /// output space, also converting between sample rates needs a delay.
        /// This function returns the sum of all such delays.
        /// The exact delay is not necessarily an integer value in either input or
        /// output sample rate. Especially when downsampling by a large value, the
        /// output sample rate may be a poor choice to represent the delay, similarly
        /// for upsampling and the input sample rate.
        /// </summary>
        /// <param name="s">(SwrContext) swr context</param>
        /// <param name="base">timebase in which the returned delay will be:
        /// if it's set to 1 the returned delay is in seconds
        /// if it's set to 1000 the returned delay is in milliseconds
        /// if it's set to the input sample rate then the returned delay is in input samples
        /// if it's set to the output sample rate then the returned delay is in output samples
        /// if it's the least common multiple of in_sample_rate and out_sample_rate then an
        /// exact rounding-free delay will be returned</param>
        /// <returns>the delay in 1 / base units</returns>
        public static long swr_get_delay(IntPtr s, long @base)
        {
            return swr_get_delay_impl(s, @base);
        }

        /// <summary>
        /// Initialize context after user parameters have been set.
        /// </summary>
        /// <param name="s">(SwrContext) Swr context to initialize</param>
        /// <returns>AVERROR error code in case of failure.</returns>
        public static AVERROR swr_init(IntPtr s)
        {
            return swr_init_impl(s);
        }

        #endregion
    }
}
