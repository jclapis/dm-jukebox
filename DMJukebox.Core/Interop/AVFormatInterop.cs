using System;
using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is the wrapper class for the FFMPEG AVFormat library.
    /// </summary>
    public static class AVFormatInterop
    {
        /// <summary>
        /// The filename of the AVFormat DLL.
        /// </summary>
        private const string AvFormatDll = "avformat-57.dll";

        /// <summary>
        /// Initialize libavformat and register all the muxers, demuxers and protocols.
        /// If you do not call this function, then you can select exactly which formats you want to support.
        /// </summary>
        [DllImport(AvFormatDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_register_all();

        /// <summary>
        /// Allocates an AVFormatContext. Use avformat_free_context() to free it.
        /// </summary>
        /// <returns>(AVFormatContext) A handle to an AVFormatContext object.</returns>
        [DllImport(AvFormatDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr avformat_alloc_context();

        /// <summary>
        /// Free an AVFormatContext created by avformat_alloc_context() and all its streams.
        /// </summary>
        /// <param name="s">(AVFormatContext) The context to free.</param>
        [DllImport(AvFormatDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avformat_free_context(IntPtr s);

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
        [DllImport(AvFormatDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR avformat_open_input(ref IntPtr ps, string url, IntPtr fmt, ref IntPtr options);

        /// <summary>
        /// Close an opened input AVFormatContext.
        /// Free it and all its contents and set *s to NULL.
        /// </summary>
        /// <param name="s">(AVFormatContext) The AVFormatContext to free.</param>
        [DllImport(AvFormatDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void avformat_close_input(ref IntPtr s);

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
        [DllImport(AvFormatDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR avformat_find_stream_info(IntPtr ic, ref IntPtr options);

        /// <summary>
        /// Print detailed information about the input or output format, such as duration, bitrate, streams, container, programs, metadata,
        /// side data, codec and time base.
        /// </summary>
        /// <param name="ic">(AVFormatContext) the context to analyze</param>
        /// <param name="index">index of the stream to dump information about</param>
        /// <param name="url">the URL to print, such as source or destination file</param>
        /// <param name="is_output">Select whether the specified context is an input(0) or output(1)</param>
        [DllImport(AvFormatDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_dump_format(IntPtr ic, int index, string url, int is_output);

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
        [DllImport(AvFormatDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR av_read_frame(IntPtr s, IntPtr pkt);
    }
}
