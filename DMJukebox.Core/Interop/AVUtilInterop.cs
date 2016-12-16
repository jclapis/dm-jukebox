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
    internal delegate void LogCallback(IntPtr avcl, int level, string fmt, IntPtr args);

    /// <summary>
    /// This is a utility class that holds the P/Invoke wrappers for libavutil.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
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
        private const string MacAVUtilLibrary = "avutil-55.dylib";

        // These regions contain the DllImport function definitions for each OS. Since we can't really set
        // the path of DllImport dynamically (and loading them dynamically using LoadLibrary / dlopen is complicated
        // to manage cross-platform), we have to pre-define them based on the names of the libraries above.

        #region Windows Functions

        [DllImport(WindowsAVUtilLibrary, EntryPoint = nameof(av_log_set_callback), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_log_set_callback_windows([MarshalAs(UnmanagedType.FunctionPtr)] LogCallback callback);

        [DllImport(WindowsAVUtilLibrary, EntryPoint = nameof(av_frame_alloc), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr av_frame_alloc_windows();

        [DllImport(WindowsAVUtilLibrary, EntryPoint = nameof(av_frame_free), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_frame_free_windows(ref IntPtr frame);

        [DllImport(WindowsAVUtilLibrary, EntryPoint = nameof(av_frame_get_buffer), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR av_frame_get_buffer_windows(IntPtr frame, int align);

        [DllImport(WindowsAVUtilLibrary, EntryPoint = nameof(av_frame_get_channels), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int av_frame_get_channels_windows(IntPtr frame);

        [DllImport(WindowsAVUtilLibrary, EntryPoint = nameof(av_frame_set_channels), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_frame_set_channels_windows(IntPtr frame, int val);

        [DllImport(WindowsAVUtilLibrary, EntryPoint = nameof(av_frame_unref), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_frame_unref_windows(IntPtr frame);

        [DllImport(WindowsAVUtilLibrary, EntryPoint = nameof(av_get_default_channel_layout), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AV_CH_LAYOUT av_get_default_channel_layout_windows(int nb_channels);

        #endregion

        #region Linux Functions

        [DllImport(LinuxAVUtilLibrary, EntryPoint = nameof(av_log_set_callback), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_log_set_callback_linux([MarshalAs(UnmanagedType.FunctionPtr)] LogCallback callback);

        [DllImport(LinuxAVUtilLibrary, EntryPoint = nameof(av_frame_alloc), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr av_frame_alloc_linux();

        [DllImport(LinuxAVUtilLibrary, EntryPoint = nameof(av_frame_free), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_frame_free_linux(ref IntPtr frame);

        [DllImport(LinuxAVUtilLibrary, EntryPoint = nameof(av_frame_get_buffer), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR av_frame_get_buffer_linux(IntPtr frame, int align);

        [DllImport(LinuxAVUtilLibrary, EntryPoint = nameof(av_frame_get_channels), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int av_frame_get_channels_linux(IntPtr frame);

        [DllImport(LinuxAVUtilLibrary, EntryPoint = nameof(av_frame_set_channels), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_frame_set_channels_linux(IntPtr frame, int val);

        [DllImport(LinuxAVUtilLibrary, EntryPoint = nameof(av_frame_unref), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_frame_unref_linux(IntPtr frame);

        [DllImport(LinuxAVUtilLibrary, EntryPoint = nameof(av_get_default_channel_layout), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AV_CH_LAYOUT av_get_default_channel_layout_linux(int nb_channels);

        #endregion

        #region OSX Functions

        [DllImport(MacAVUtilLibrary, EntryPoint = nameof(av_log_set_callback), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_log_set_callback_osx([MarshalAs(UnmanagedType.FunctionPtr)] LogCallback callback);

        [DllImport(MacAVUtilLibrary, EntryPoint = nameof(av_frame_alloc), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr av_frame_alloc_osx();

        [DllImport(MacAVUtilLibrary, EntryPoint = nameof(av_frame_free), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_frame_free_osx(ref IntPtr frame);

        [DllImport(MacAVUtilLibrary, EntryPoint = nameof(av_frame_get_buffer), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR av_frame_get_buffer_osx(IntPtr frame, int align);

        [DllImport(MacAVUtilLibrary, EntryPoint = nameof(av_frame_get_channels), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int av_frame_get_channels_osx(IntPtr frame);

        [DllImport(MacAVUtilLibrary, EntryPoint = nameof(av_frame_set_channels), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_frame_set_channels_osx(IntPtr frame, int val);

        [DllImport(MacAVUtilLibrary, EntryPoint = nameof(av_frame_unref), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_frame_unref_osx(IntPtr frame);

        [DllImport(MacAVUtilLibrary, EntryPoint = nameof(av_get_default_channel_layout), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AV_CH_LAYOUT av_get_default_channel_layout_osx(int nb_channels);

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
        private delegate AV_CH_LAYOUT av_get_default_channel_layout_delegate(int nb_channels);

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
        private static av_get_default_channel_layout_delegate av_get_default_channel_layout_impl;

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
                av_get_default_channel_layout_impl = av_get_default_channel_layout_windows;
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
                av_get_default_channel_layout_impl = av_get_default_channel_layout_linux;
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
                av_get_default_channel_layout_impl = av_get_default_channel_layout_osx;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region Public API
        
        /// <summary>
        /// Sets the logging callback that will receive internal messages. The callback must be thread-safe.
        /// </summary>
        /// <param name="callback">The callback to receive logging messages</param>
        public static void av_log_set_callback([MarshalAs(UnmanagedType.FunctionPtr)] LogCallback callback)
        {
            av_log_set_callback_impl(callback);
        }
        
        /// <summary>
        /// Allocates a new, empty <see cref="AVFrame"/>.
        /// </summary>
        /// <returns>(<see cref="AVFrame"/>*) The new frame</returns>
        public static IntPtr av_frame_alloc()
        {
            return av_frame_alloc_impl();
        }
        
        /// <summary>
        /// Frees an <see cref="AVFrame"/> and its contents. The pointer in
        /// <paramref name="frame"/> will be set to <see cref="IntPtr.Zero"/>.
        /// </summary>
        /// <param name="frame">(<see cref="AVFrame"/>*) The frame to free</param>
        public static void av_frame_free(ref IntPtr frame)
        {
            av_frame_free_impl(ref frame);
        }

        /// <summary>
        /// Allocates new buffers for audio or video data. Before calling this,
        /// the <see cref="AVFrame.nb_samples"/>, <see cref="AVFrame.channel_layout"/>,
        /// and <see cref="AVFrame.format"/> fields need to be set properly.
        /// </summary>
        /// <param name="frame">(<see cref="AVFrame"/>*) The frame to hold the new buffers</param>
        /// <param name="align">Required alignment for the buffer size (I think it's safe to
        /// just leave this as zero)</param>
        /// <returns><see cref="AVERROR.AVERROR_SUCCESS"/> on a success, or an error code on a failure.</returns>
        public static AVERROR av_frame_get_buffer(IntPtr frame, int align)
        {
            return av_frame_get_buffer_impl(frame, align);
        }
        
        /// <summary>
        /// Returns the number of audio channels contained within the given <see cref="AVFrame"/>.
        /// </summary>
        /// <param name="frame">(<see cref="AVFrame"/>*) The frame to get the channel count of</param>
        /// <returns>The number of channels contained within the frame</returns>
        public static int av_frame_get_channels(IntPtr frame)
        {
            return av_frame_get_channels_impl(frame);
        }
        
        /// <summary>
        /// Sets the number of audio channels that an <see cref="AVFrame"/> is expected to have.
        /// </summary>
        /// <param name="frame">(<see cref="AVFrame"/>*) The frame to set the channel count of</param>
        /// <param name="val">The new number of channels the frame should have</param>
        public static void av_frame_set_channels(IntPtr frame, int val)
        {
            av_frame_set_channels_impl(frame, val);
        }
        
        /// <summary>
        /// Resets an <see cref="AVFrame"/>, returning its fields to their default values and 
        /// unreferencing the data buffer(s) inside.
        /// </summary>
        /// <param name="frame">(<see cref="AVFrame"/>*) The frame to reset</param>
        public static void av_frame_unref(IntPtr frame)
        {
            av_frame_unref_impl(frame);
        }
        
        /// <summary>
        /// Returns the default preferred layout for the given number of channels.
        /// </summary>
        /// <param name="nb_channels">The number of channels to get the layout for</param>
        /// <returns>The default layout for this channel count</returns>
        public static AV_CH_LAYOUT av_get_default_channel_layout(int nb_channels)
        {
            return av_get_default_channel_layout_impl(nb_channels);
        }

        #endregion
    }
}
