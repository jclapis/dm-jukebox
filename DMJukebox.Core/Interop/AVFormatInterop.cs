/* 
 * This file contains C# wrappers for some of the functions exported by libavformat.
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
    /// This is a utility class that holds the P/Invoke wrappers for libavformat.
    /// </summary>
    internal static class AVFormatInterop
    {
        /// <summary>
        /// The DLL for Windows
        /// </summary>
        private const string WindowsAVFormatLibrary = "avformat-57.dll";

        /// <summary>
        /// The SO for Linux
        /// </summary>
        private const string LinuxAVFormatLibrary = "avformat-57.so";

        /// <summary>
        /// The Dylib for OSX
        /// </summary>
        private const string MacAVFormatLibrary = "avformat-57.dylib";

        // These regions contain the DllImport function definitions for each OS. Since we can't really set
        // the path of DllImport dynamically (and loading them dynamically using LoadLibrary / dlopen is complicated
        // to manage cross-platform), we have to pre-define them based on the names of the libraries above.

        #region Windows Functions

        [DllImport(WindowsAVFormatLibrary, EntryPoint = nameof(av_register_all), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_register_all_windows();

        [DllImport(WindowsAVFormatLibrary, EntryPoint = nameof(avformat_alloc_context), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr avformat_alloc_context_windows();

        [DllImport(WindowsAVFormatLibrary, EntryPoint = nameof(avformat_free_context), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void avformat_free_context_windows(IntPtr s);

        [DllImport(WindowsAVFormatLibrary, EntryPoint = nameof(avformat_open_input), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR avformat_open_input_windows(ref IntPtr ps, string url, IntPtr fmt, ref IntPtr options);

        [DllImport(WindowsAVFormatLibrary, EntryPoint = nameof(avformat_close_input), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void avformat_close_input_windows(ref IntPtr s);

        [DllImport(WindowsAVFormatLibrary, EntryPoint = nameof(avformat_find_stream_info), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR avformat_find_stream_info_windows(IntPtr ic, ref IntPtr options);

        [DllImport(WindowsAVFormatLibrary, EntryPoint = nameof(av_dump_format), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_dump_format_windows(IntPtr ic, int index, string url, int is_output);

        [DllImport(WindowsAVFormatLibrary, EntryPoint = nameof(av_read_frame), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR av_read_frame_windows(IntPtr s, IntPtr pkt);

        [DllImport(WindowsAVFormatLibrary, EntryPoint = nameof(av_seek_frame), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR av_seek_frame_windows(IntPtr s, int stream_index, long timestamp, AVSEEK_FLAG flags);

        [DllImport(WindowsAVFormatLibrary, EntryPoint = nameof(avformat_seek_file), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR avformat_seek_file_windows(IntPtr s, int stream_index, long min_ts, long ts, long max_ts, AVSEEK_FLAG flags);

        #endregion

        #region Linux Functions

        [DllImport(LinuxAVFormatLibrary, EntryPoint = nameof(av_register_all), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_register_all_linux();

        [DllImport(LinuxAVFormatLibrary, EntryPoint = nameof(avformat_alloc_context), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr avformat_alloc_context_linux();

        [DllImport(LinuxAVFormatLibrary, EntryPoint = nameof(avformat_free_context), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void avformat_free_context_linux(IntPtr s);

        [DllImport(LinuxAVFormatLibrary, EntryPoint = nameof(avformat_open_input), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR avformat_open_input_linux(ref IntPtr ps, string url, IntPtr fmt, ref IntPtr options);

        [DllImport(LinuxAVFormatLibrary, EntryPoint = nameof(avformat_close_input), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void avformat_close_input_linux(ref IntPtr s);

        [DllImport(LinuxAVFormatLibrary, EntryPoint = nameof(avformat_find_stream_info), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR avformat_find_stream_info_linux(IntPtr ic, ref IntPtr options);

        [DllImport(LinuxAVFormatLibrary, EntryPoint = nameof(av_dump_format), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_dump_format_linux(IntPtr ic, int index, string url, int is_output);

        [DllImport(LinuxAVFormatLibrary, EntryPoint = nameof(av_read_frame), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR av_read_frame_linux(IntPtr s, IntPtr pkt);

        [DllImport(LinuxAVFormatLibrary, EntryPoint = nameof(av_seek_frame), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR av_seek_frame_linux(IntPtr s, int stream_index, long timestamp, AVSEEK_FLAG flags);

        [DllImport(LinuxAVFormatLibrary, EntryPoint = nameof(avformat_seek_file), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR avformat_seek_file_linux(IntPtr s, int stream_index, long min_ts, long ts, long max_ts, AVSEEK_FLAG flags);

        #endregion

        #region OSX Functions

        [DllImport(MacAVFormatLibrary, EntryPoint = nameof(av_register_all), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_register_all_osx();

        [DllImport(MacAVFormatLibrary, EntryPoint = nameof(avformat_alloc_context), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr avformat_alloc_context_osx();

        [DllImport(MacAVFormatLibrary, EntryPoint = nameof(avformat_free_context), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void avformat_free_context_osx(IntPtr s);

        [DllImport(MacAVFormatLibrary, EntryPoint = nameof(avformat_open_input), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR avformat_open_input_osx(ref IntPtr ps, string url, IntPtr fmt, ref IntPtr options);

        [DllImport(MacAVFormatLibrary, EntryPoint = nameof(avformat_close_input), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void avformat_close_input_osx(ref IntPtr s);

        [DllImport(MacAVFormatLibrary, EntryPoint = nameof(avformat_find_stream_info), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR avformat_find_stream_info_osx(IntPtr ic, ref IntPtr options);

        [DllImport(MacAVFormatLibrary, EntryPoint = nameof(av_dump_format), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_dump_format_osx(IntPtr ic, int index, string url, int is_output);

        [DllImport(MacAVFormatLibrary, EntryPoint = nameof(av_read_frame), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR av_read_frame_osx(IntPtr s, IntPtr pkt);

        [DllImport(MacAVFormatLibrary, EntryPoint = nameof(av_seek_frame), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR av_seek_frame_osx(IntPtr s, int stream_index, long timestamp, AVSEEK_FLAG flags);

        [DllImport(MacAVFormatLibrary, EntryPoint = nameof(avformat_seek_file), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR avformat_seek_file_osx(IntPtr s, int stream_index, long min_ts, long ts, long max_ts, AVSEEK_FLAG flags);

        #endregion

        #region Delegates and Platform-Dependent Loading

        // These delegates all represent the function signatures for the libavformat methods I need to call.

        private delegate void av_register_all_delegate();
        private delegate IntPtr avformat_alloc_context_delegate();
        private delegate void avformat_free_context_delegate(IntPtr s);
        private delegate AVERROR avformat_open_input_delegate(ref IntPtr ps, string url, IntPtr fmt, ref IntPtr options);
        private delegate void avformat_close_input_delegate(ref IntPtr s);
        private delegate AVERROR avformat_find_stream_info_delegate(IntPtr ic, ref IntPtr options);
        private delegate void av_dump_format_delegate(IntPtr ic, int index, string url, int is_output);
        private delegate AVERROR av_read_frame_delegate(IntPtr s, IntPtr pkt);
        private delegate AVERROR av_seek_frame_delegate(IntPtr s, int stream_index, long timestamp, AVSEEK_FLAG flags);
        private delegate AVERROR avformat_seek_file_delegate(IntPtr s, int stream_index, long min_ts, long ts, long max_ts, AVSEEK_FLAG flags);

        // These fields represent function pointers towards each of the extern functions. They get set
        // to the proper platform-specific functions by the static constructor. For example, if this is
        // running on a Windows machine, each of these pointers will point to the various avformat_XXX_windows
        // extern functions listed above.

        private static av_register_all_delegate av_register_all_impl;
        private static avformat_alloc_context_delegate avformat_alloc_context_impl;
        private static avformat_free_context_delegate avformat_free_context_impl;
        private static avformat_open_input_delegate avformat_open_input_impl;
        private static avformat_close_input_delegate avformat_close_input_impl;
        private static avformat_find_stream_info_delegate avformat_find_stream_info_impl;
        private static av_dump_format_delegate av_dump_format_impl;
        private static av_read_frame_delegate av_read_frame_impl;
        private static av_seek_frame_delegate av_seek_frame_impl;
        private static avformat_seek_file_delegate avformat_seek_file_impl;

        /// <summary>
        /// The static constructor figures out which library to use for P/Invoke based
        /// on the current OS platform.
        /// </summary>
        static AVFormatInterop()
        {
            NativePathFinder.AddNativeLibraryPathToEnvironmentVariable();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                av_register_all_impl = av_register_all_windows;
                avformat_alloc_context_impl = avformat_alloc_context_windows;
                avformat_free_context_impl = avformat_free_context_windows;
                avformat_open_input_impl = avformat_open_input_windows;
                avformat_close_input_impl = avformat_close_input_windows;
                avformat_find_stream_info_impl = avformat_find_stream_info_windows;
                av_dump_format_impl = av_dump_format_windows;
                av_read_frame_impl = av_read_frame_windows;
                av_seek_frame_impl = av_seek_frame_windows;
                avformat_seek_file_impl = avformat_seek_file_windows;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                av_register_all_impl = av_register_all_linux;
                avformat_alloc_context_impl = avformat_alloc_context_linux;
                avformat_free_context_impl = avformat_free_context_linux;
                avformat_open_input_impl = avformat_open_input_linux;
                avformat_close_input_impl = avformat_close_input_linux;
                avformat_find_stream_info_impl = avformat_find_stream_info_linux;
                av_dump_format_impl = av_dump_format_linux;
                av_read_frame_impl = av_read_frame_linux;
                av_seek_frame_impl = av_seek_frame_linux;
                avformat_seek_file_impl = avformat_seek_file_linux;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                av_register_all_impl = av_register_all_osx;
                avformat_alloc_context_impl = avformat_alloc_context_osx;
                avformat_free_context_impl = avformat_free_context_osx;
                avformat_open_input_impl = avformat_open_input_osx;
                avformat_close_input_impl = avformat_close_input_osx;
                avformat_find_stream_info_impl = avformat_find_stream_info_osx;
                av_dump_format_impl = av_dump_format_osx;
                av_read_frame_impl = av_read_frame_osx;
                av_seek_frame_impl = av_seek_frame_osx;
                avformat_seek_file_impl = avformat_seek_file_osx;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region Public API

        public static void av_register_all()
        {
            av_register_all_impl();
        }
        
        public static IntPtr avformat_alloc_context()
        {
            return avformat_alloc_context_impl();
        }
        
        public static void avformat_free_context(IntPtr s)
        {
            avformat_free_context_impl(s);
        }
        
        public static AVERROR avformat_open_input(ref IntPtr ps, string url, IntPtr fmt, ref IntPtr options)
        {
            return avformat_open_input_impl(ref ps, url, fmt, ref options);
        }
        
        public static void avformat_close_input(ref IntPtr s)
        {
            avformat_close_input_impl(ref s);
        }
        
        public static AVERROR avformat_find_stream_info(IntPtr ic, ref IntPtr options)
        {
            return avformat_find_stream_info_impl(ic, ref options);
        }
        
        public static void av_dump_format(IntPtr ic, int index, string url, int is_output)
        {
            av_dump_format_impl(ic, index, url, is_output);
        }
        
        public static AVERROR av_read_frame(IntPtr s, IntPtr pkt)
        {
            return av_read_frame_impl(s, pkt);
        }
        
        public static AVERROR av_seek_frame(IntPtr s, int stream_index, long timestamp, AVSEEK_FLAG flags)
        {
            return av_seek_frame_impl(s, stream_index, timestamp, flags);
        }
        
        public static AVERROR avformat_seek_file(IntPtr s, int stream_index, long min_ts, long ts, long max_ts, AVSEEK_FLAG flags)
        {
            return avformat_seek_file_impl(s, stream_index, min_ts, ts, max_ts, flags);
        }

        #endregion
    }
}
