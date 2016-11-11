using System;
using System.Runtime.InteropServices;

namespace DiscordJukebox.Interop
{
    /// <summary>
    /// Stream structure.
    /// New fields can be added to the end with minor version bumps.
    /// Removal, reordering and changes to existing fields require a major
    /// version bump.
    /// sizeof(AVStream) must not be used outside libav*.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct AVStream
    {
        /// <summary>
        /// stream index in AVFormatContext
        /// </summary>
        public int index;

        /// <summary>
        /// Format-specific stream ID.
        /// decoding: set by libavformat
        /// encoding: set by the user, replaced by libavformat if left unset
        /// </summary>
        public int id;

        /// <summary>
        /// @deprecated use the codecpar struct instead
        /// </summary>
        public IntPtr codec;

        public IntPtr priv_data;

        /// <summary>
        /// @deprecated this field is unused
        /// </summary>
        public AVFrac pts;

        /// <summary>
        /// This is the fundamental unit of time (in seconds) in terms
        /// of which frame timestamps are represented.
        /// decoding: set by libavformat
        /// encoding: May be set by the caller before avformat_write_header() to
        ///           provide a hint to the muxer about the desired timebase. In
        ///           avformat_write_header(), the muxer will overwrite this field
        ///           with the timebase that will actually be used for the timestamps
        ///           written into the file(which may or may not be related to the
        ///           user-provided one, depending on the format).
        /// </summary>
        public AVRational time_base;

        /// <summary>
        /// Decoding: pts of the first frame of the stream in presentation order, in stream time base.
        /// Only set this if you are absolutely 100% sure that the value you set
        /// it to really is the pts of the first frame.
        /// This may be undefined (AV_NOPTS_VALUE).
        /// @note The ASF header does NOT contain a correct start_time the ASF
        /// demuxer must NOT set this.
        /// </summary>
        public long start_time;

        /// <summary>
        /// Decoding: duration of the stream, in stream time base.
        /// If a source file does not specify a duration, but does specify
        /// a bitrate, this value will be estimated from bitrate and file size.
        /// </summary>
        public long duration;

        /// <summary>
        /// number of frames in this stream if known or 0
        /// </summary>
        public long nb_frames;

        /// <summary>
        /// AV_DISPOSITION_* bit field
        /// </summary>
        public AV_DISPOSITION disposition;

        /// <summary>
        /// Selects which packets can be discarded at will and do not need to be demuxed.
        /// </summary>
        public AVDiscard discard;

        /// <summary>
        /// sample aspect ratio (0 if unknown)
        /// - encoding: Set by user.
        /// - decoding: Set by libavformat.
        /// </summary>
        public AVRational sample_aspect_ratio;

        public IntPtr metadata;

        /// <summary>
        /// Average framerate
        /// - demuxing: May be set by libavformat when creating the stream or in
        ///             avformat_find_stream_info().
        /// - muxing: May be set by the caller before avformat_write_header().
        /// </summary>
        public AVRational avg_frame_rate;

        /// <summary>
        /// For streams with AV_DISPOSITION_ATTACHED_PIC disposition, this packet
        /// will contain the attached picture.
        /// decoding: set by libavformat, must not be modified by the caller.
        /// encoding: unused
        /// </summary>
        public AVPacket attached_pic;

        /// <summary>
        /// An array of side data that applies to the whole stream (i.e. the
        /// container does not allow it to change between packets).
        ///
        /// There may be no overlap between the side data in this array and side data
        /// in the packets. I.e. a given side data is either exported by the muxer
        /// (demuxing) / set by the caller(muxing) in this array, then it never
        /// appears in the packets, or the side data is exported / sent through
        /// the packets(always in the first packet where the value becomes known or
        /// changes), then it does not appear in this array.
        ///
        /// - demuxing: Set by libavformat when the stream is created.
        /// - muxing: May be set by the caller before avformat_write_header().
        ///
        /// Freed by libavformat in avformat_free_context().
        ///
        /// @see av_format_inject_global_side_data()
        /// </summary>
        public IntPtr side_data;

        public int nb_side_data;

        /// <summary>
        /// Flags for the user to detect events happening on the stream. Flags must
        /// be cleared by the user once the event has been handled.
        /// A combination of AVSTREAM_EVENT_FLAG_*.
        /// </summary>
        public AVSTREAM_EVENT_FLAG event_flags;

        // The rest of the fields are supposed to be internal to libavformat, so they aren't included here.
    }
}
