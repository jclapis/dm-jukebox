/* 
 * This file contains C# wrappers for some of the functions exported by libavutil.
 * They come from log.h and frame.h. 
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

namespace DiscordJukebox.Interop
{
    /// <summary>
    /// This is a delegate for a logging function that ffmpeg can use to write log messages.
    /// </summary>
    /// <param name="avcl">(void*) A pointer to an arbitrary struct of which the first field is a
    /// pointer to an AVClass struct.</param>
    /// <param name="level">The importance level of the message expressed using a
    /// lavu_log_constants "Logging Constant".</param>
    /// <param name="fmt">The format string (printf-compatible) that specifies how
    /// subsequent arguments are converted to output.</param>
    /// <param name="args">(va_list) The arguments referenced by the format string.</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public delegate void LogCallback(IntPtr avcl, int level, string fmt, IntPtr args);

    /// <summary>
    /// This class holds C# bindings for some of the functions exported by libavutil.
    /// </summary>
    internal static class AVUtilInterop
    {
        /// <summary>
        /// The location of the AVUtil DLL
        /// </summary>
        private const string AvUtilDll = "avutil-55.dll";

        /// <summary>
        /// Set the logging callback
        /// 
        /// The callback must be thread safe, even if the application does not use
        /// threads itself as some codecs are multithreaded.
        /// </summary>
        /// <param name="callback">A logging function with a compatible signature.</param>
        [DllImport(AvUtilDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_log_set_callback(
            [MarshalAs(UnmanagedType.FunctionPtr)] LogCallback callback);

        /// <summary>
        /// Allocate an AVFrame and set its fields to default values. The resulting
        /// struct must be freed using av_frame_free().
        /// 
        /// This only allocates the AVFrame itself, not the data buffers. Those
        /// must be allocated through other means, e.g. with av_frame_get_buffer() or
        /// manually.
        /// </summary>
        /// <returns>(AVFrame) An AVFrame filled with default values or NULL on failure.</returns>
        [DllImport(AvUtilDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr av_frame_alloc();

        /// <summary>
        /// Free the frame and any dynamically allocated objects in it,
        /// e.g. extended_data. If the frame is reference counted, it will be
        /// unreferenced first.
        /// </summary>
        /// <param name="frame">(AVFrame) frame to be freed. The pointer will be set to NULL.</param>
        [DllImport(AvUtilDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_free(ref IntPtr frame);

        /// <summary>
        /// Allocate new buffer(s) for audio or video data.
        ///
        /// The following fields must be set on frame before calling this function:
        /// - format(pixel format for video, sample format for audio)
        /// - width and height for video
        /// - nb_samples and channel_layout for audio
        ///
        /// This function will fill AVFrame.data and AVFrame.buf arrays and, if
        /// necessary, allocate and fill AVFrame.extended_data and AVFrame.extended_buf.
        /// For planar formats, one buffer will be allocated for each plane.
        /// </summary>
        /// <param name="frame">(AVFrame) frame in which to store the new buffers.</param>
        /// <param name="align">required buffer size alignment</param>
        /// <returns>0 on success, a negative AVERROR on error.</returns>
        [DllImport(AvUtilDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR av_frame_get_buffer(IntPtr frame, int align);

        /// <summary>
        /// Gets the number of audio channels in an AVFrame.
        /// </summary>
        /// <param name="frame">(AVFrame) The frame</param>
        /// <returns>The number of audio channels in the frame</returns>
        [DllImport(AvUtilDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_frame_get_channels(IntPtr frame);

        /// <summary>
        /// Sets the number of audio channels for an AVFrame.
        /// </summary>
        /// <param name="frame">(AVFrame) The frame</param>
        /// <param name="val">The number of audio channels</param>
        [DllImport(AvUtilDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_set_channels(IntPtr frame, int val);
    }
}
