/* 
 * This file contains C# wrappers for some of the functions exported by libavutil.
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

namespace DMJukebox.Interop
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
    /// This is a utility class that holds the P/Invoke wrappers for libavutil.
    /// </summary>
    internal static class AVUtilInterop
    {
        /// <summary>
        /// The DLL for Windows
        /// </summary>
        private const string WindowsAVUtilLibrary = "avutil-55.dll";

        /// <summary>
        /// The SO for Linux
        /// </summary>
        private const string LinuxAVUtilLibrary = "avutil-55.so";

        /// <summary>
        /// The Dylib for OSX
        /// </summary>
        private const string MacAVUtilLibrary = "avutil-55.so";

        // These regions contain the DllImport function definitions for each OS. Since we can't really set
        // the path of DllImport dynamically (and loading them dynamically using LoadLibrary / dlopen is complicated
        // to manage cross-platform), we have to pre-define them based on the names of the libraries above.

        #region Windows Functions

        [DllImport(WindowsAVUtilLibrary, EntryPoint = nameof(av_log_set_callback), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_log_set_callback_windows([MarshalAs(UnmanagedType.FunctionPtr)] LogCallback callback);

        [DllImport(WindowsAVUtilLibrary, EntryPoint = nameof(av_frame_alloc), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr av_frame_alloc_windows();

        [DllImport(WindowsAVUtilLibrary, EntryPoint = nameof(av_frame_free), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_free_windows(ref IntPtr frame);

        [DllImport(WindowsAVUtilLibrary, EntryPoint = nameof(av_frame_get_buffer), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR av_frame_get_buffer_windows(IntPtr frame, int align);

        [DllImport(WindowsAVUtilLibrary, EntryPoint = nameof(av_frame_get_channels), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_frame_get_channels_windows(IntPtr frame);

        [DllImport(WindowsAVUtilLibrary, EntryPoint = nameof(av_frame_set_channels), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_set_channels_windows(IntPtr frame, int val);

        [DllImport(WindowsAVUtilLibrary, EntryPoint = nameof(av_frame_unref), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_unref_windows(IntPtr frame);

        #endregion

        #region Linux Functions

        [DllImport(LinuxAVUtilLibrary, EntryPoint = nameof(av_log_set_callback), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_log_set_callback_linux([MarshalAs(UnmanagedType.FunctionPtr)] LogCallback callback);

        [DllImport(LinuxAVUtilLibrary, EntryPoint = nameof(av_frame_alloc), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr av_frame_alloc_linux();

        [DllImport(LinuxAVUtilLibrary, EntryPoint = nameof(av_frame_free), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_free_linux(ref IntPtr frame);

        [DllImport(LinuxAVUtilLibrary, EntryPoint = nameof(av_frame_get_buffer), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR av_frame_get_buffer_linux(IntPtr frame, int align);

        [DllImport(LinuxAVUtilLibrary, EntryPoint = nameof(av_frame_get_channels), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_frame_get_channels_linux(IntPtr frame);

        [DllImport(LinuxAVUtilLibrary, EntryPoint = nameof(av_frame_set_channels), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_set_channels_linux(IntPtr frame, int val);

        [DllImport(LinuxAVUtilLibrary, EntryPoint = nameof(av_frame_unref), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_unref_linux(IntPtr frame);

        #endregion

        #region OSX Functions

        [DllImport(MacAVUtilLibrary, EntryPoint = nameof(av_log_set_callback), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_log_set_callback_osx([MarshalAs(UnmanagedType.FunctionPtr)] LogCallback callback);

        [DllImport(MacAVUtilLibrary, EntryPoint = nameof(av_frame_alloc), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr av_frame_alloc_osx();

        [DllImport(MacAVUtilLibrary, EntryPoint = nameof(av_frame_free), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_free_osx(ref IntPtr frame);

        [DllImport(MacAVUtilLibrary, EntryPoint = nameof(av_frame_get_buffer), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR av_frame_get_buffer_osx(IntPtr frame, int align);

        [DllImport(MacAVUtilLibrary, EntryPoint = nameof(av_frame_get_channels), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int av_frame_get_channels_osx(IntPtr frame);

        [DllImport(MacAVUtilLibrary, EntryPoint = nameof(av_frame_set_channels), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_set_channels_osx(IntPtr frame, int val);

        [DllImport(MacAVUtilLibrary, EntryPoint = nameof(av_frame_unref), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_unref_osx(IntPtr frame);

        #endregion

        #region Delegates and Platform-Dependent Loading

        // These delegates all represent the function signatures for the libavutil methods I need to call.

        private delegate void av_log_set_callback_delegate([MarshalAs(UnmanagedType.FunctionPtr)] LogCallback callback);
        private delegate IntPtr av_frame_alloc_delegate();
        private delegate void av_frame_free_delegate(ref IntPtr frame);
        private delegate AVERROR av_frame_get_buffer_delegate(IntPtr frame, int align);
        private delegate int av_frame_get_channels_delegate(IntPtr frame);
        private delegate void av_frame_set_channels_delegate(IntPtr frame, int val);
        private delegate void av_frame_unref_delegate(IntPtr frame);

        // These fields represent function pointers towards each of the extern functions. They get set
        // to the proper platform-specific functions by the static constructor. For example, if this is
        // running on a Windows machine, each of these pointers will point to the various avutil_XXX_windows
        // extern functions listed above.

        private static av_log_set_callback_delegate av_log_set_callback_impl;
        private static av_frame_alloc_delegate av_frame_alloc_impl;
        private static av_frame_free_delegate av_frame_free_impl;
        private static av_frame_get_buffer_delegate av_frame_get_buffer_impl;
        private static av_frame_get_channels_delegate av_frame_get_channels_impl;
        private static av_frame_set_channels_delegate av_frame_set_channels_impl;
        private static av_frame_unref_delegate av_frame_unref_impl;

        /// <summary>
        /// The static constructor figures out which library to use for P/Invoke based
        /// on the current OS platform.
        /// </summary>
        static AVUtilInterop()
        {
            NativePathFinder.AddNativeLibraryPathToEnvironmentVariable();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                av_log_set_callback_impl = av_log_set_callback_windows;
                av_frame_alloc_impl = av_frame_alloc_windows;
                av_frame_free_impl = av_frame_free_windows;
                av_frame_get_buffer_impl = av_frame_get_buffer_windows;
                av_frame_get_channels_impl = av_frame_get_channels_windows;
                av_frame_set_channels_impl = av_frame_set_channels_windows;
                av_frame_unref_impl = av_frame_unref_windows;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                av_log_set_callback_impl = av_log_set_callback_linux;
                av_frame_alloc_impl = av_frame_alloc_linux;
                av_frame_free_impl = av_frame_free_linux;
                av_frame_get_buffer_impl = av_frame_get_buffer_linux;
                av_frame_get_channels_impl = av_frame_get_channels_linux;
                av_frame_set_channels_impl = av_frame_set_channels_linux;
                av_frame_unref_impl = av_frame_unref_linux;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                av_log_set_callback_impl = av_log_set_callback_osx;
                av_frame_alloc_impl = av_frame_alloc_osx;
                av_frame_free_impl = av_frame_free_osx;
                av_frame_get_buffer_impl = av_frame_get_buffer_osx;
                av_frame_get_channels_impl = av_frame_get_channels_osx;
                av_frame_set_channels_impl = av_frame_set_channels_osx;
                av_frame_unref_impl = av_frame_unref_osx;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region Public API

        /// <summary>
        /// Set the logging callback
        /// 
        /// The callback must be thread safe, even if the application does not use
        /// threads itself as some codecs are multithreaded.
        /// </summary>
        /// <param name="callback">A logging function with a compatible signature.</param>
        public static void av_log_set_callback([MarshalAs(UnmanagedType.FunctionPtr)] LogCallback callback)
        {
            av_log_set_callback_impl(callback);
        }

        /// <summary>
        /// Allocate an AVFrame and set its fields to default values. The resulting
        /// struct must be freed using av_frame_free().
        /// 
        /// This only allocates the AVFrame itself, not the data buffers. Those
        /// must be allocated through other means, e.g. with av_frame_get_buffer() or
        /// manually.
        /// </summary>
        /// <returns>(AVFrame) An AVFrame filled with default values or NULL on failure.</returns>
        public static IntPtr av_frame_alloc()
        {
            return av_frame_alloc_impl();
        }

        /// <summary>
        /// Free the frame and any dynamically allocated objects in it,
        /// e.g. extended_data. If the frame is reference counted, it will be
        /// unreferenced first.
        /// </summary>
        /// <param name="frame">(AVFrame) frame to be freed. The pointer will be set to NULL.</param>
        public static void av_frame_free(ref IntPtr frame)
        {
            av_frame_free_impl(ref frame);
        }

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
        public static AVERROR av_frame_get_buffer(IntPtr frame, int align)
        {
            return av_frame_get_buffer_impl(frame, align);
        }

        /// <summary>
        /// Gets the number of audio channels in an AVFrame.
        /// </summary>
        /// <param name="frame">(AVFrame) The frame</param>
        /// <returns>The number of audio channels in the frame</returns>
        public static int av_frame_get_channels(IntPtr frame)
        {
            return av_frame_get_channels_impl(frame);
        }

        /// <summary>
        /// Sets the number of audio channels for an AVFrame.
        /// </summary>
        /// <param name="frame">(AVFrame) The frame</param>
        /// <param name="val">The number of audio channels</param>
        public static void av_frame_set_channels(IntPtr frame, int val)
        {
            av_frame_set_channels_impl(frame, val);
        }

        /// <summary>
        /// Unreference all the buffers referenced by frame and reset the frame fields.
        /// </summary>
        /// <param name="frame">(AVFrame) The frame</param>
        public static void av_frame_unref(IntPtr frame)
        {
            av_frame_unref_impl(frame);
        }

        #endregion
    }
}
