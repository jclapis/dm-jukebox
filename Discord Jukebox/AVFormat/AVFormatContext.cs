using System;
using System.Runtime.InteropServices;

namespace DiscordJukebox.Interop
{
    /// <summary>
    /// Callback used by devices to communicate with application.
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate int av_format_control_message(IntPtr s, int type, IntPtr data, UIntPtr data_size);

    /// <summary>
    /// A callback for opening new IO streams.
    /// Whenever a muxer or a demuxer needs to open an IO stream (typically from
    /// avformat_open_input() for demuxers, but for certain formats can happen at
    /// other times as well), it will call this callback to obtain an IO context.
    /// </summary>
    /// <param name="s">the format context</param>
    /// <param name="pb">on success, the newly opened IO context should be returned here</param>
    /// <param name="url">url the url to open</param>
    /// <param name="flags">flags a combination of AVIO_FLAG_*</param>
    /// <param name="options">options a dictionary of additional options, with the same
    /// semantics as in avio_open2()</param>
    /// <returns>0 on success, a negative AVERROR code on failure</returns>
    /// <remarks>Certain muxers and demuxers do nesting, i.e. they open one or more
    /// additional internal format contexts.Thus the AVFormatContext pointer
    /// passed to this callback may be different from the one facing the caller.
    /// It will, however, have the same 'opaque' field.</remarks>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate int io_open_Delegate(IntPtr s, ref IntPtr pb, string url, int flags, ref IntPtr options);

    /// <summary>
    /// A callback for closing the streams opened with AVFormatContext.io_open().
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void io_close_Delegate(IntPtr s, IntPtr pb);

    /// <summary>
    /// Called to open further IO contexts when needed for demuxing.
    /// This can be set by the user application to perform security checks on
    /// the URLs before opening them.
    /// The function should behave like avio_open2(), AVFormatContext is provided
    /// as contextual information and to reach AVFormatContext.opaque.
    /// If NULL then some simple checks are used together with avio_open2().
    /// Must not be accessed directly from outside avformat.
    /// Demuxing: Set by user.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="p"></param>
    /// <param name="url"></param>
    /// <param name="flags"></param>
    /// <param name="int_cb"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate int open_cb_Delegate(IntPtr s, ref IntPtr p, string url, int flags, IntPtr int_cb, ref IntPtr options);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct AVFormatContext
    {
        /// <summary>
        /// A class for logging and @ref avoptions. Set by avformat_alloc_context().
        /// Exports(de)muxer private options if they exist.
        /// </summary>
        public IntPtr av_class;

        /// <summary>
        /// The input container format.
        /// Demuxing only, set by avformat_open_input().
        /// </summary>
        public IntPtr iformat;

        /// <summary>
        /// The output container format.
        /// Muxing only, must be set by the caller before avformat_write_header().
        /// </summary>
        public IntPtr oformat;

        /// <summary>
        /// Format private data. This is an AVOptions-enabled struct
        /// if and only if iformat/oformat.priv_class is not NULL.
        /// - muxing: set by avformat_write_header()
        /// - demuxing: set by avformat_open_input()
        /// </summary>
        public IntPtr priv_data;

        /// <summary>
        /// I/O context.
        /// - demuxing: either set by the user before avformat_open_input() (then
        ///             the user must close it manually) or set by avformat_open_input().
        /// - muxing: set by the user before avformat_write_header(). The caller must
        ///           take care of closing / freeing the IO context.
        /// Do NOT set this field if AVFMT_NOFILE flag is set in
        /// iformat/oformat.flags.In such a case, the (de)muxer will handle
        /// I/O in some other way and this field will be NULL.
        /// </summary>
        public IntPtr pb;

        #region Stream Info

        /// <summary>
        /// Flags signalling stream properties. A combination of AVFMTCTX_*.
        /// Set by libavformat.
        /// </summary>
        public int ctx_flags;

        /// <summary>
        /// Number of elements in AVFormatContext.streams. 
        /// Set by avformat_new_stream(), must not be modified by any other code.
        /// </summary>
        public uint nb_streams;

        /// <summary>
        /// A list of all streams in the file. New streams are created with 
        /// avformat_new_stream().
        /// - demuxing: streams are created by libavformat in avformat_open_input().
        ///             If AVFMTCTX_NOHEADER is set in ctx_flags, then new streams may also
        ///             appear in av_read_frame().
        /// - muxing: streams are created by the user before avformat_write_header().
        /// Freed by libavformat in avformat_free_context().
        /// </summary>
        public IntPtr streams;

        /// <summary>
        /// input or output filename
        /// - demuxing: set by avformat_open_input()
        /// - muxing: may be set by the caller before avformat_write_header()
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string filename;

        /// <summary>
        /// Position of the first frame of the component, in
        /// AV_TIME_BASE fractional seconds.NEVER set this value directly:
        /// It is deduced from the AVStream values.
        /// Demuxing only, set by libavformat.
        /// </summary>
        public long start_time;

        /// <summary>
        /// Duration of the stream, in AV_TIME_BASE fractional
        /// seconds. Only set this value if you know none of the individual stream
        /// durations and also do not set any of them.This is deduced from the
        /// AVStream values if not set.
        /// Demuxing only, set by libavformat.
        /// </summary>
        public long duration;

        /// <summary>
        /// Total stream bitrate in bit/s, 0 if not
        /// available. Never set it directly if the file_size and the
        /// duration are known as FFmpeg can compute it automatically.
        /// </summary>
        public long bit_rate;
        
        public uint packet_size;

        public int max_delay;

        /// <summary>
        /// Flags modifying the (de)muxer behaviour. A combination of AVFMT_FLAG_*.
        /// Set by the user before avformat_open_input() / avformat_write_header().
        /// </summary>
        public AvFormatFlags flags;

        /// <summary>
        /// Maximum size of the data read from input for determining
        /// the input container format.
        /// Demuxing only, set by the caller before avformat_open_input().
        /// </summary>
        public long probesize;

        /// <summary>
        /// Maximum duration (in AV_TIME_BASE units) of the data read
        /// from input in avformat_find_stream_info().
        /// Demuxing only, set by the caller before avformat_find_stream_info().
        /// Can be set to 0 to let avformat choose using a heuristic.
        /// </summary>
        public long max_analyze_duration;

        public IntPtr key;

        public int keylen;

        public uint nb_programs;

        public IntPtr programs;

        /// <summary>
        /// Forced video codec_id.
        /// Demuxing: Set by user.
        /// </summary>
        public AVCodecID video_codec_id;

        /// <summary>
        /// Forced audio codec_id.
        /// Demuxing: Set by user.
        /// </summary>
        public AVCodecID audio_codec_id;

        /// <summary>
        /// Forced subtitle codec_id.
        /// Demuxing: Set by user.
        /// </summary>
        public AVCodecID subtitle_codec_id;

        /// <summary>
        /// Maximum amount of memory in bytes to use for the index of each stream.
        /// If the index exceeds this size, entries will be discarded as
        /// needed to maintain a smaller size.This can lead to slower or less
        /// accurate seeking (depends on demuxer).
        /// Demuxers for which a full in-memory index is mandatory will ignore
        /// this.
        /// - muxing: unused
        /// - demuxing: set by user
        /// </summary>
        public uint max_index_size;

        /// <summary>
        /// Maximum amount of memory in bytes to use for buffering frames
        /// obtained from realtime capture devices.
        /// </summary>
        public uint max_picture_buffer;

        /// <summary>
        /// Number of chapters in AVChapter array.
        /// When muxing, chapters are normally written in the file header,
        /// so nb_chapters should normally be initialized before write_header
        /// is called.Some muxers(e.g.mov and mkv) can also write chapters
        /// in the trailer.To write chapters in the trailer, nb_chapters
        /// must be zero when write_header is called and non-zero when
        /// write_trailer is called.
        /// - muxing: set by user
        /// - demuxing: set by libavformat
        /// </summary>
        public uint nb_chapters;

        public IntPtr chapters;

        /// <summary>
        /// Metadata that applies to the whole file.
        /// - demuxing: set by libavformat in avformat_open_input()
        /// - muxing: may be set by the caller before avformat_write_header()
        /// Freed by libavformat in avformat_free_context().
        /// </summary>
        public IntPtr metadata;

        /// <summary>
        /// Start time of the stream in real world time, in microseconds
        /// since the Unix epoch(00:00 1st January 1970). That is, pts=0 in the
        /// stream was captured at this real world time.
        /// muxing: Set by the caller before avformat_write_header(). If set to
        ///         either 0 or AV_NOPTS_VALUE, then the current wall-time will
        ///         be used.
        ///  - demuxing: Set by libavformat. AV_NOPTS_VALUE if unknown. Note that
        ///              the value may become known after some number of frames
        ///              have been received.
        /// </summary>
        public long start_time_realtime;

        /// <summary>
        /// The number of frames used for determining the framerate in
        /// avformat_find_stream_info().
        /// Demuxing only, set by the caller before avformat_find_stream_info().
        /// </summary>
        public int fps_probe_size;

        /// <summary>
        /// Error recognition; higher values will detect more errors but may
        /// misdetect some more or less valid parts as errors.
        /// Demuxing only, set by the caller before avformat_open_input().
        /// </summary>
        public int error_recognition;

        /// <summary>
        /// Custom interrupt callbacks for the I/O layer.
        /// demuxing: set by the user before avformat_open_input().
        /// muxing: set by the user before avformat_write_header()
        /// (mainly useful for AVFMT_NOFILE formats). The callback
        /// should also be passed to avio_open2() if it's used to
        /// open the file.
        /// </summary>
        public AVIOInterruptCB interrupt_callback;

        /// <summary>
        /// Flags to enable debugging.
        /// </summary>
        public int debug;

        /// <summary>
        /// Maximum buffering duration for interleaving.
        /// To ensure all the streams are interleaved correctly,
        /// av_interleaved_write_frame() will wait until it has at least one packet
        /// for each stream before actually writing any packets to the output file.
        /// When some streams are "sparse" (i.e.there are large gaps between
        /// successive packets), this can result in excessive buffering.
        /// This field specifies the maximum difference between the timestamps of the
        /// first and the last packet in the muxing queue, above which libavformat
        /// will output a packet regardless of whether it has queued a packet for all
        /// the streams.
        /// Muxing only, set by the caller before avformat_write_header().
        /// </summary>
        public long max_interleave_delta;

        /// <summary>
        /// Allow non-standard and experimental extension
        /// <see cref="AVCodecContext.strict_std_compliance"/>
        /// </summary>
        public int strict_std_compliance;

        /// <summary>
        /// Flags for the user to detect events happening on the file. Flags must
        /// be cleared by the user once the event has been handled.
        /// A combination of AVFMT_EVENT_FLAG_*.
        /// </summary>
        public int event_flags;

        /// <summary>
        /// Maximum number of packets to read while waiting for the first timestamp.
        /// Decoding only.
        /// </summary>
        public int max_ts_probe;

        /// <summary>
        /// Avoid negative timestamps during muxing.
        /// Any value of the AVFMT_AVOID_NEG_TS_* constants.
        /// Note, this only works when using av_interleaved_write_frame. (interleave_packet_per_dts is in use)
        /// - muxing: Set by user
        /// - demuxing: unused
        /// </summary>
        public AVFMT_AVOID_NEG_TS avoid_negative_ts;

        /// <summary>
        /// Transport stream id.
        /// This will be moved into demuxer private options.Thus no API/ABI compatibility
        /// </summary>
        public int ts_id;

        /// <summary>
        /// Audio preload in microseconds.
        /// Note, not all formats support this and unpredictable things may happen if it is used when not supported.
        /// - encoding: Set by user via AVOptions (NO direct access)
        /// - decoding: unused
        /// </summary>
        public int audio_preload;

        /// <summary>
        /// Max chunk time in microseconds.
        /// Note, not all formats support this and unpredictable things may happen if it is used when not supported.
        /// - encoding: Set by user via AVOptions (NO direct access)
        /// - decoding: unused
        /// </summary>
        public int max_chunk_duration;

        /// <summary>
        /// Max chunk size in bytes
        /// Note, not all formats support this and unpredictable things may happen if it is used when not supported.
        /// - encoding: Set by user via AVOptions (NO direct access)
        /// - decoding: unused
        /// </summary>
        public int max_chunk_size;

        /// <summary>
        /// forces the use of wallclock timestamps as pts/dts of packets
        /// This has undefined results in the presence of B frames.
        /// - encoding: unused
        /// - decoding: Set by user via AVOptions(NO direct access)
        /// </summary>
        public int use_wallclock_as_timestamps;

        /// <summary>
        /// avio flags, used to force AVIO_FLAG_DIRECT.
        /// - encoding: unused
        /// - decoding: Set by user via AVOptions (NO direct access)
        /// </summary>
        public int avio_flags;

        /// <summary>
        /// The duration field can be estimated through various ways, and this field can be used
        /// to know how the duration was estimated.
        /// - encoding: unused
        /// - decoding: Read by user via AVOptions (NO direct access)
        /// </summary>
        public AVDurationEstimationMethod duration_estimation_method;

        /// <summary>
        /// Skip initial bytes when opening stream
        /// - encoding: unused
        /// - decoding: Set by user via AVOptions (NO direct access)
        /// </summary>
        public long skip_initial_bytes;

        /// <summary>
        /// Correct single timestamp overflows
        /// - encoding: unused
        /// - decoding: Set by user via AVOptions (NO direct access)
        /// </summary>
        public uint correct_ts_overflow;

        /// <summary>
        /// Force seeking to any (also non key) frames.
        /// - encoding: unused
        /// - decoding: Set by user via AVOptions (NO direct access)
        /// </summary>
        public int seek2any;

        /// <summary>
        /// Flush the I/O context after each packet.
        /// - encoding: Set by user via AVOptions (NO direct access)
        /// - decoding: unused
        /// </summary>
        public int flush_packets;

        /// <summary>
        /// format probing score.
        /// The maximal score is AVPROBE_SCORE_MAX, its set when the demuxer probes
        /// the format.
        /// - encoding: unused
        /// - decoding: set by avformat, read by user via av_format_get_probe_score() (NO direct access)
        /// </summary>
        public int probe_score;

        /// <summary>
        /// number of bytes to read maximally to identify format.
        /// - encoding: unused
        /// - decoding: set by user through AVOPtions(NO direct access)
        /// </summary>
        public int format_probesize;

        /// <summary>
        /// ',' separated list of allowed decoders.
        /// If NULL then all are allowed
        /// - encoding: unused
        /// - decoding: set by user through AVOptions (NO direct access)
        /// </summary>
        public string codec_whitelist;

        /// <summary>
        /// ',' separated list of allowed demuxers.
        /// If NULL then all are allowed
        /// - encoding: unused
        /// - decoding: set by user through AVOptions (NO direct access)
        /// </summary>
        public string format_whitelist;

        /// <summary>
        /// An opaque field for libavformat internal usage.
        /// Must not be accessed in any way by callers.
        /// </summary>
        public IntPtr @internal;

        /// <summary>
        /// IO repositioned flag.
        /// This is set by avformat when the underlaying IO context read pointer
        /// is repositioned, for example when doing byte based seeking.
        /// Demuxers can use the flag to detect such changes.
        /// </summary>
        public int io_repositioned;

        /// <summary>
        /// Forced video codec.
        /// This allows forcing a specific decoder, even when there are multiple with
        /// the same codec_id.
        /// Demuxing: Set by user via av_format_set_video_codec (NO direct access).
        /// </summary>
        public IntPtr video_codec;

        /// <summary>
        /// Forced audio codec.
        /// This allows forcing a specific decoder, even when there are multiple with
        /// the same codec_id.
        /// Demuxing: Set by user via av_format_set_audio_codec (NO direct access).
        /// </summary>
        public IntPtr audio_codec;

        /// <summary>
        /// Forced subtitle codec.
        /// This allows forcing a specific decoder, even when there are multiple with
        /// the same codec_id.
        /// Demuxing: Set by user via av_format_set_subtitle_codec (NO direct access).
        /// </summary>
        public IntPtr subtitle_codec;

        /// <summary>
        /// Forced data codec.
        /// This allows forcing a specific decoder, even when there are multiple with
        /// the same codec_id.
        /// Demuxing: Set by user via av_format_set_data_codec (NO direct access).
        /// </summary>
        public IntPtr data_codec;

        /// <summary>
        /// Number of bytes to be written as padding in a metadata header.
        /// Demuxing: Unused.
        /// Muxing: Set by user via av_format_set_metadata_header_padding.
        /// </summary>
        public int metadata_header_padding;

        /// <summary>
        /// User data.
        /// This is a place for some private data of the user.
        /// </summary>
        public IntPtr opaque;

        /// <summary>
        /// Callback used by devices to communicate with application.
        /// </summary>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public av_format_control_message control_message_cb;

        /// <summary>
        /// Output timestamp offset, in microseconds.
        /// Muxing: set by user via AVOptions(NO direct access)
        /// </summary>
        public long output_ts_offset;

        /// <summary>
        /// dump format separator.
        /// can be ", " or "\n      " or anything else
        /// Code outside libavformat should access this field using AVOptions
        /// (NO direct access).
        /// - muxing: Set by user.
        /// - demuxing: Set by user.
        /// </summary>
        public IntPtr dump_separator;

        /// <summary>
        /// Forced Data codec_id.
        /// Demuxing: Set by user.
        /// </summary>
        public AVCodecID data_codec_id;

        /// <summary>
        /// Called to open further IO contexts when needed for demuxing.
        /// </summary>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public open_cb_Delegate open_cb;

        /// <summary>
        /// ',' separated list of allowed protocols.
        /// - encoding: unused
        /// - decoding: set by user through AVOptions (NO direct access)
        /// </summary>
        public string protocol_whitelist;

        /// <summary>
        /// A callback for opening new IO streams.
        /// </summary>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public io_open_Delegate io_open;

        /// <summary>
        /// A callback for closing the streams opened with AVFormatContext.io_open().
        /// </summary>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public io_close_Delegate io_close;

        /// <summary>
        /// ',' separated list of disallowed protocols.
        /// - encoding: unused
        /// - decoding: set by user through AVOptions(NO direct access)
        /// </summary>
        public string protocol_blacklist;

        #endregion
    }

}
