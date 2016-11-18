/* 
 * This file contains C# wrappers for some of the functions exported by libavformat.
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
    /// This is a utility class that holds the P/Invoke wrappers for libavformat.
    /// </summary>
    public static class AVFormatInterop
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
        public static extern void av_register_all_windows();

        [DllImport(WindowsAVFormatLibrary, EntryPoint = nameof(avformat_alloc_context), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr avformat_alloc_context_windows();

        [DllImport(WindowsAVFormatLibrary, EntryPoint = nameof(avformat_free_context), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avformat_free_context_windows(IntPtr s);

        [DllImport(WindowsAVFormatLibrary, EntryPoint = nameof(avformat_open_input), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR avformat_open_input_windows(ref IntPtr ps, string url, IntPtr fmt, ref IntPtr options);

        [DllImport(WindowsAVFormatLibrary, EntryPoint = nameof(avformat_close_input), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avformat_close_input_windows(ref IntPtr s);

        [DllImport(WindowsAVFormatLibrary, EntryPoint = nameof(avformat_find_stream_info), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR avformat_find_stream_info_windows(IntPtr ic, ref IntPtr options);

        [DllImport(WindowsAVFormatLibrary, EntryPoint = nameof(av_dump_format), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_dump_format_windows(IntPtr ic, int index, string url, int is_output);

        [DllImport(WindowsAVFormatLibrary, EntryPoint = nameof(av_read_frame), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR av_read_frame_windows(IntPtr s, IntPtr pkt);

        #endregion

        #region Linux Functions

        [DllImport(LinuxAVFormatLibrary, EntryPoint = nameof(av_register_all), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_register_all_linux();

        [DllImport(LinuxAVFormatLibrary, EntryPoint = nameof(avformat_alloc_context), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr avformat_alloc_context_linux();

        [DllImport(LinuxAVFormatLibrary, EntryPoint = nameof(avformat_free_context), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avformat_free_context_linux(IntPtr s);

        [DllImport(LinuxAVFormatLibrary, EntryPoint = nameof(avformat_open_input), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR avformat_open_input_linux(ref IntPtr ps, string url, IntPtr fmt, ref IntPtr options);

        [DllImport(LinuxAVFormatLibrary, EntryPoint = nameof(avformat_close_input), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avformat_close_input_linux(ref IntPtr s);

        [DllImport(LinuxAVFormatLibrary, EntryPoint = nameof(avformat_find_stream_info), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR avformat_find_stream_info_linux(IntPtr ic, ref IntPtr options);

        [DllImport(LinuxAVFormatLibrary, EntryPoint = nameof(av_dump_format), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_dump_format_linux(IntPtr ic, int index, string url, int is_output);

        [DllImport(LinuxAVFormatLibrary, EntryPoint = nameof(av_read_frame), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR av_read_frame_linux(IntPtr s, IntPtr pkt);

        #endregion

        #region OSX Functions

        [DllImport(MacAVFormatLibrary, EntryPoint = nameof(av_register_all), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_register_all_osx();

        [DllImport(MacAVFormatLibrary, EntryPoint = nameof(avformat_alloc_context), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr avformat_alloc_context_osx();

        [DllImport(MacAVFormatLibrary, EntryPoint = nameof(avformat_free_context), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avformat_free_context_osx(IntPtr s);

        [DllImport(MacAVFormatLibrary, EntryPoint = nameof(avformat_open_input), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR avformat_open_input_osx(ref IntPtr ps, string url, IntPtr fmt, ref IntPtr options);

        [DllImport(MacAVFormatLibrary, EntryPoint = nameof(avformat_close_input), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avformat_close_input_osx(ref IntPtr s);

        [DllImport(MacAVFormatLibrary, EntryPoint = nameof(avformat_find_stream_info), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR avformat_find_stream_info_osx(IntPtr ic, ref IntPtr options);

        [DllImport(MacAVFormatLibrary, EntryPoint = nameof(av_dump_format), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_dump_format_osx(IntPtr ic, int index, string url, int is_output);

        [DllImport(MacAVFormatLibrary, EntryPoint = nameof(av_read_frame), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR av_read_frame_osx(IntPtr s, IntPtr pkt);

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
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region Public API

        /// <summary>
        /// Initialize libavformat and register all the muxers, demuxers and protocols.
        /// If you do not call this function, then you can select exactly which formats you want to support.
        /// </summary>
        public static void av_register_all()
        {
            av_register_all_impl();
        }

        /// <summary>
        /// Allocates an AVFormatContext. Use avformat_free_context() to free it.
        /// </summary>
        /// <returns>(AVFormatContext) A handle to an AVFormatContext object.</returns>
        public static IntPtr avformat_alloc_context()
        {
            return avformat_alloc_context_impl();
        }

        /// <summary>
        /// Free an AVFormatContext created by avformat_alloc_context() and all its streams.
        /// </summary>
        /// <param name="s">(AVFormatContext) The context to free.</param>
        public static void avformat_free_context(IntPtr s)
        {
            avformat_free_context_impl(s);
        }

        /// <summary>
        /// Open an input stream and read the header.
        /// The codecs are not opened. The stream must be closed with avformat_close_input().
        /// </summary>
        /// <param name="ps">(AVFormatContext) Pointer to user-supplied AVFormatContext (allocated by avformat_alloc_context).
        /// May be a pointer to NULL, in which case an AVFormatContext is allocated by this function and written into ps.
        /// Note that a user-supplied AVFormatContext will be freed on failure.</param>
        /// <param name="url">URL of the stream to open.</param>
        /// <param name="fmt">(AVInputFormat) If non-NULL, this parameter forces a specific input format.
        /// Otherwise the format is autodetected.</param>
        /// <param name="options">(AVDictionary) A dictionary filled with AVFormatContext and demuxer-private options.
        /// On return this parameter will be destroyed and replaced with a dict containing options that were not found. May be NULL.</param>
        /// <returns>0 on success, a negative AVERROR on failure.</returns>
        public static AVERROR avformat_open_input(ref IntPtr ps, string url, IntPtr fmt, ref IntPtr options)
        {
            return avformat_open_input_impl(ref ps, url, fmt, ref options);
        }

        /// <summary>
        /// Close an opened input AVFormatContext.
        /// Free it and all its contents and set *s to NULL.
        /// </summary>
        /// <param name="s">(AVFormatContext) The AVFormatContext to free.</param>
        public static void avformat_close_input(ref IntPtr s)
        {
            avformat_close_input_impl(ref s);
        }

        /// <summary>
        /// Read packets of a media file to get stream information.
        /// This is useful for file formats with no headers such as MPEG. This function also computes the real framerate in case of
        /// MPEG-2 repeat frame mode. The logical file position is not changed by this function; examined packets may be buffered
        /// for later processing.
        /// </summary>
        /// <param name="ic">(AVFormatContext) media file handle</param>
        /// <param name="options">(AVDictionary) If non-NULL, an ic.nb_streams long array of pointers to dictionaries, where i-th member contains
        /// options for codec corresponding to i-th stream. On return each dictionary will be filled with options that were not found.</param>
        /// <returns>>=0 if OK, AVERROR_xxx on error</returns>
        public static AVERROR avformat_find_stream_info(IntPtr ic, ref IntPtr options)
        {
            return avformat_find_stream_info_impl(ic, ref options);
        }

        /// <summary>
        /// Print detailed information about the input or output format, such as duration, bitrate, streams, container, programs, metadata,
        /// side data, codec and time base.
        /// </summary>
        /// <param name="ic">(AVFormatContext) the context to analyze</param>
        /// <param name="index">index of the stream to dump information about</param>
        /// <param name="url">the URL to print, such as source or destination file</param>
        /// <param name="is_output">Select whether the specified context is an input(0) or output(1)</param>
        public static void av_dump_format(IntPtr ic, int index, string url, int is_output)
        {
            av_dump_format_impl(ic, index, url, is_output);
        }

        /// <summary>
        /// Return the next frame of a stream.
        /// This function returns what is stored in the file, and does not validate
        /// that what is there are valid frames for the decoder. It will split what is
        /// stored in the file into frames and return one for each call. It will not
        /// omit invalid data between valid frames so as to give the decoder the maximum
        /// information possible for decoding.
        ///
        /// If pkt->buf is NULL, then the packet is valid until the next
        /// av_read_frame() or until avformat_close_input(). Otherwise the packet
        /// is valid indefinitely. In both cases the packet must be freed with
        /// av_packet_unref when it is no longer needed. For video, the packet contains
        /// exactly one frame. For audio, it contains an integer number of frames if each
        /// frame has a known fixed size (e.g. PCM or ADPCM data). If the audio frames
        /// have a variable size (e.g. MPEG audio), then it contains one frame.
        /// pkt->pts, pkt->dts and pkt->duration are always set to correct
        /// values in AVStream
        /// </summary>
        /// <param name="s">(AVFormatContext) AVFormatContext</param>
        /// <param name="pkt">(AVPacket) AVPacket</param>
        /// <returns>0 if OK, &lt; 0 on error or end of file</returns>
        public static AVERROR av_read_frame(IntPtr s, IntPtr pkt)
        {
            return av_read_frame_impl(s, pkt);
        }

        #endregion
    }
}
