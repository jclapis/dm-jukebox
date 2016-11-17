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
    /// This class holds C# bindings for some of the functions exported by libswresample.
    /// </summary>
    internal static class SWResampleInterop
    {
        /// <summary>
        /// The location of the SWResample DLL
        /// </summary>
        private const string SwResampleDll = "swresample-2.dll";

        /// <summary>
        /// Allocate SwrContext.
        /// 
        /// If you use this function you will need to set the parameters
        /// (manually or with swr_alloc_set_opts()) before calling swr_init().
        /// </summary>
        /// <returns>NULL on error, allocated context otherwise</returns>
        [DllImport(SwResampleDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr swr_alloc();

        /// <summary>
        /// Free the given SwrContext and set the pointer to NULL.
        /// </summary>
        /// <param name="s">a pointer to a pointer to Swr context</param>
        [DllImport(SwResampleDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void swr_free(ref IntPtr s);

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
        [DllImport(SwResampleDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR swr_config_frame(IntPtr swr, IntPtr @out, IntPtr @in);

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
        [DllImport(SwResampleDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR swr_convert_frame(IntPtr swr, IntPtr output, IntPtr input);

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
        [DllImport(SwResampleDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long swr_get_delay(IntPtr s, long @base);

        /// <summary>
        /// Initialize context after user parameters have been set.
        /// </summary>
        /// <param name="s">(SwrContext) Swr context to initialize</param>
        /// <returns>AVERROR error code in case of failure.</returns>
        [DllImport(SwResampleDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR swr_init(IntPtr s);
    }
}
