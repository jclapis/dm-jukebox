using System;
using System.Runtime.InteropServices;

namespace DiscordJukebox.Interop
{
    /// <summary>
    /// This is the wrapper class for the FFMPEG AVFormat library.
    /// </summary>
    internal static class AvFormatInterface
    {
        /// <summary>
        /// The filename of the AVFormat DLL.
        /// </summary>
        private const string AvFormatLibrary = "avformat-57.dll";

        /// <summary>
        /// Initialize libavformat and register all the muxers, demuxers and protocols.
        /// If you do not call this function, then you can select exactly which formats you want to support.
        /// </summary>
        [DllImport(AvFormatLibrary)]
        public static extern void av_register_all();

        /// <summary>
        /// Allocates an AVFormatContext. Use avformat_free_context() to free it.
        /// </summary>
        /// <returns>A handle to an AVFormatContext object.</returns>
        [DllImport(AvFormatLibrary)]
        public static extern IntPtr avformat_alloc_context();

        /// <summary>
        /// Free an AVFormatContext created by avformat_alloc_context() and all its streams.
        /// </summary>
        /// <param name="s">The context to free.</param>
        [DllImport(AvFormatLibrary)]
        public static extern void avformat_free_context(IntPtr s);

        /// <summary>
        /// Open an input stream and read the header.
        /// The codecs are not opened. The stream must be closed with avformat_close_input().
        /// </summary>
        /// <param name="ps">Pointer to user-supplied AVFormatContext (allocated by avformat_alloc_context).
        /// May be a pointer to NULL, in which case an AVFormatContext is allocated by this function and written into ps.
        /// Note that a user-supplied AVFormatContext will be freed on failure.</param>
        /// <param name="url">URL of the stream to open.</param>
        /// <param name="fmt">If non-NULL, this parameter forces a specific input format.
        /// Otherwise the format is autodetected.</param>
        /// <param name="options">A dictionary filled with AVFormatContext and demuxer-private options.
        /// On return this parameter will be destroyed and replaced with a dict containing options that were not found. May be NULL.</param>
        /// <returns>0 on success, a negative AVERROR on failure.</returns>
        [DllImport(AvFormatLibrary)]
        public static extern int avformat_open_input(ref IntPtr ps, string url, IntPtr fmt, ref IntPtr options);

        /// <summary>
        /// Close an opened input AVFormatContext.
        /// Free it and all its contents and set *s to NULL.
        /// </summary>
        /// <param name="s">The AVFormatContext to free.</param>
        [DllImport(AvFormatLibrary)]
        public static extern void avformat_close_input(ref IntPtr s);

        /// <summary>
        /// Read packets of a media file to get stream information.
        /// This is useful for file formats with no headers such as MPEG. This function also computes the real framerate in case of
        /// MPEG-2 repeat frame mode. The logical file position is not changed by this function; examined packets may be buffered
        /// for later processing.
        /// </summary>
        /// <param name="ic">media file handle</param>
        /// <param name="options">If non-NULL, an ic.nb_streams long array of pointers to dictionaries, where i-th member contains
        /// options for codec corresponding to i-th stream. On return each dictionary will be filled with options that were not
        /// found.</param>
        /// <returns>>=0 if OK, AVERROR_xxx on error</returns>
        [DllImport(AvFormatLibrary)]
        public static extern int avformat_find_stream_info(IntPtr ic, ref IntPtr options);

        /// <summary>
        /// Print detailed information about the input or output format, such as duration, bitrate, streams, container, programs, metadata,
        /// side data, codec and time base.
        /// </summary>
        /// <param name="ic">the context to analyze</param>
        /// <param name="index">index of the stream to dump information about</param>
        /// <param name="url">the URL to print, such as source or destination file</param>
        /// <param name="is_output">Select whether the specified context is an input(0) or output(1)</param>
        [DllImport(AvFormatLibrary)]
        public static extern void av_dump_format(IntPtr ic, int index, string url, int is_output);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int SetStdHandle(int device, IntPtr handle);

        [DllImport("avutil-55.dll")]
        public static extern void av_log_set_callback(
            [MarshalAs(UnmanagedType.FunctionPtr)] LogCallback callback);
    }
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public delegate void LogCallback(IntPtr avcl, int level, string fmt, IntPtr args);
}
