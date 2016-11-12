﻿using System;
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

    internal static class AVUtilInterop
    {
        /// <summary>
        /// The location of the AVUtil DLL
        /// </summary>
        private const string AvUtilDll = "lib/avutil-55.dll";

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
    }
}
