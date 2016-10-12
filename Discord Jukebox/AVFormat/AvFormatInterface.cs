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
        public static extern int avformat_open_input(out IntPtr ps, string url, IntPtr fmt, ref IntPtr options);

        /// <summary>
        /// Close an opened input AVFormatContext.
        /// Free it and all its contents and set *s to NULL.
        /// </summary>
        /// <param name="s">The AVFormatContext to free.</param>
        [DllImport(AvFormatLibrary)]
        public static extern void avformat_close_input(ref IntPtr s);
    }

}
