using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DiscordJukebox.Interop
{
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
        /// seconds.Only set this value if you know none of the individual stream
        /// durations and also do not set any of them.This is deduced from the
        /// AVStream values if not set.
        /// Demuxing only, set by libavformat.
        /// </summary>
        public long duration;

        /// <summary>
        /// Total stream bitrate in bit/s, 0 if not
        /// available.Never set it directly if the file_size and the
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

        #endregion
    }

}
