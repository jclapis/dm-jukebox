using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DiscordJukebox.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void draw_horiz_band(IntPtr s, IntPtr src,
        [MarshalAs(UnmanagedType.LPArray, SizeConst = 8)] int[] offset,
        int y, int type, int height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate AVPixelFormat get_format(IntPtr s, IntPtr fmt);

    /// <summary>
    /// main external API structure.
    /// New fields can be added to the end with minor version bumps.
    /// Removal, reordering and changes to existing fields require a major
    /// version bump.
    /// Please use AVOptions(av_opt* / av_set/get*()) to access these fields from user
    /// applications.
    /// The name string for AVOptions options matches the associated command line
    /// parameter name and can be found in libavcodec/options_table.h
    /// The AVOption/command line parameter names differ in some cases from the C
    /// structure field names for historic reasons or brevity.
    /// sizeof(AVCodecContext) must not be used outside libav*.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct AVCodecContext
    {
        /// <summary>
        /// information on struct for av_log
        /// - set by avcodec_alloc_context3
        /// </summary>
        public IntPtr av_class;

        public int log_level_offset;

        /// <summary>
        /// see AVMEDIA_TYPE_xxx
        /// </summary>
        public AVMediaType codec_type;

        public IntPtr codec;

        /// <summary>
        /// @deprecated this field is not used for anything in libavcodec
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string codec_name;

        /// <summary>
        /// see AV_CODEC_ID_xxx
        /// </summary>
        public AVCodecID codec_id;

        /// <summary>
        /// fourcc (LSB first, so "ABCD" -> ('D'<<24) + ('C'<<16) + ('B'<<8) + 'A').
        /// This is used to work around some encoder bugs.
        /// A demuxer should set this to what is stored in the field used to identify the codec.
        /// If there are multiple such fields in a container then the demuxer should choose the one
        /// which maximizes the information about the used codec.
        /// If the codec tag field in a container is larger than 32 bits then the demuxer should
        /// remap the longer ID to 32 bits with a table or other structure. Alternatively a new

        /// extra_codec_tag + size could be added but for this a clear advantage must be demonstrated
        /// first.
        /// - encoding: Set by user, if not then the default based on codec_id will be used.
        /// - decoding: Set by user, will be converted to uppercase by libavcodec during init.
        /// </summary>
        public uint codec_tag;

        /// <summary>
        /// this field is unused
        /// </summary>
        public uint stream_codec_tag;

        public IntPtr priv_data;

        /// <summary>
        /// Private context used for internal data.
        /// Unlike priv_data, this is not codec-specific.It is used in general
        /// libavcodec functions.
        /// </summary>
        public IntPtr @internal;

        /// <summary>
        /// Private data of the user, can be used to carry app specific stuff.
        /// - encoding: Set by user.
        /// - decoding: Set by user.
        /// </summary>
        public IntPtr opaque;

        /// <summary>
        /// the average bitrate
        /// - encoding: Set by user; unused for constant quantizer encoding.
        /// - decoding: Set by user, may be overwritten by libavcodec
        ///             if this info is available in the stream
        /// </summary>
        public long bit_rate;

        /// <summary>
        /// number of bits the bitstream is allowed to diverge from the reference.
        ///           the reference can be CBR(for CBR pass1) or VBR(for pass2)
        /// - encoding: Set by user; unused for constant quantizer encoding.
        /// - decoding: unused
        /// </summary>
        public int bit_rate_tolerance;

        /// <summary>
        /// Global quality for codecs which cannot change it per frame.
        /// This should be proportional to MPEG-1/2/4 qscale.
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public int global_quality;

        /// <summary>
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public int compression_level;

        /// <summary>
        /// AV_CODEC_FLAG_*.
        /// - encoding: Set by user.
        /// - decoding: Set by user.
        /// </summary>
        public AV_CODEC_FLAG flags;

        /// <summary>
        /// AV_CODEC_FLAG2_*
        /// - encoding: Set by user.
        /// - decoding: Set by user.
        /// </summary>
        public AV_CODEC_FLAG2 flags2;

        /// <summary>
        /// some codecs need / can use extradata like Huffman tables.
        /// MJPEG: Huffman tables
        /// rv10: additional flags
        /// MPEG-4: global headers(they can be in the bitstream or here)
        /// The allocated memory should be AV_INPUT_BUFFER_PADDING_SIZE bytes larger
        /// than extradata_size to avoid problems if it is read with the bitstream reader.
        /// The bytewise contents of extradata must not depend on the architecture or CPU endianness.
        /// - encoding: Set/allocated/freed by libavcodec.
        /// - decoding: Set/allocated/freed by user.
        /// </summary>
        public IntPtr extradata;

        public int extradata_size;

        /// <summary>
        /// This is the fundamental unit of time (in seconds) in terms
        /// of which frame timestamps are represented.For fixed-fps content,
        ///timebase should be 1 / framerate and timestamp increments should be
        /// identically 1.
        /// This often, but not always is the inverse of the frame rate or field rate
        /// for video. 1 / time_base is not the average frame rate if the frame rate is not
        /// constant.
        /// Like containers, elementary streams also can store timestamps, 1 / time_base
        /// is the unit in which these timestamps are specified.
        /// As example of such codec time base see ISO / IEC 14496 - 2:2001(E)
        /// vop_time_increment_resolution and fixed_vop_rate
        /// (fixed_vop_rate == 0 implies that it is different from the framerate)
        /// - encoding: MUST be set by user.
        /// - decoding: the use of this field for decoding is deprecated.
        ///             Use framerate instead.
        /// </summary>
        public AVRational time_base;

        /// <summary>
        /// For some codecs, the time base is closer to the field rate than the frame rate.
        /// Most notably, H.264 and MPEG-2 specify time_base as half of frame duration
        /// if no telecine is used...
        /// Set to time_base ticks per frame.Default 1, e.g., H.264/MPEG-2 set it to 2.
        /// </summary>
        public int ticks_per_frame;

        /// <summary>
        /// Codec delay.
        /// Encoding: Number of frames delay there will be from the encoder input to
        ///           the decoder output. (we assume the decoder matches the spec)
        /// Decoding: Number of frames delay in addition to what a standard decoder
        ///           as specified in the spec would produce.
        /// Video:
        ///   Number of frames the decoded output will be delayed relative to the
        ///   encoded input.
        /// Audio:
        ///   For encoding, this field is unused (see initial_padding).
        ///   For decoding, this is the number of samples the decoder needs to
        ///   output before the decoder's output is valid. When seeking, you should
        ///   start decoding this many samples prior to your desired seek point.
        /// - encoding: Set by libavcodec.
        /// - decoding: Set by libavcodec.
        /// </summary>
        public int delay;

        /// <summary>
        /// picture width / height.
        /// @note Those fields may not match the values of the last
        /// AVFrame output by avcodec_decode_video2 due frame
        /// reordering.
        /// - encoding: MUST be set by user.
        /// - decoding: May be set by the user before opening the decoder if known e.g.
        ///             from the container. Some decoders will require the dimensions
        ///             to be set by the caller. During decoding, the decoder may
        ///             overwrite those values as required while parsing the data.
        /// </summary>
        public int width;

        public int height;

        /// <summary>
        /// Bitstream width / height, may be different from width/height e.g. when
        /// the decoded frame is cropped before being output or lowres is enabled.
        /// @note Those field may not match the value of the last
        /// AVFrame output by avcodec_receive_frame() due frame
        /// reordering.
        /// - encoding: unused
        /// - decoding: May be set by the user before opening the decoder if known
        ///             e.g. from the container. During decoding, the decoder may
        ///             overwrite those values as required while parsing the data.
        /// </summary>
        public int coded_width;

        public int coded_height;

        /// <summary>
        /// the number of pictures in a group of pictures, or 0 for intra_only
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public int gop_size;

        /// <summary>
        /// Pixel format, see AV_PIX_FMT_xxx.
        /// May be set by the demuxer if known from headers.
        /// May be overridden by the decoder if it knows better.
        /// @note This field may not match the value of the last
        /// AVFrame output by avcodec_receive_frame() due frame
        /// reordering.
        /// - encoding: Set by user.
        /// - decoding: Set by user if known, overridden by libavcodec while
        ///             parsing the data.
        /// </summary>
        public AVPixelFormat pix_fmt;

        /// <summary>
        /// This option does nothing
        /// @deprecated use codec private options instead
        /// </summary>
        public int me_method;

        /// <summary>
        /// If non NULL, 'draw_horiz_band' is called by the libavcodec
        /// decoder to draw a horizontal band.It improves cache usage.Not
        /// all codecs can do that.You must check the codec capabilities
        /// beforehand.
        /// When multithreading is used, it may be called from multiple threads
        /// at the same time; threads might draw different parts of the same AVFrame,
        /// or multiple AVFrames, and there is no guarantee that slices will be drawn
        /// in order.
        /// The function is also used by hardware acceleration APIs.
        /// It is called at least once during frame decoding to pass
        /// the data needed for hardware render.
        /// In that mode instead of pixel data, AVFrame points to
        /// a structure specific to the acceleration API. The application
        /// reads the structure and can change some fields to indicate progress
        /// or mark state.
        /// - encoding: unused
        /// - decoding: Set by user.
        /// @param height the height of the slice
        /// @param y the y position of the slice
        /// @param type 1->top field, 2->bottom field, 3->frame
        /// @param offset offset into the AVFrame.data from which the slice should be read
        /// </summary>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public draw_horiz_band draw_horiz_band;

        /// <summary>
        /// callback to negotiate the pixelFormat
        /// @param fmt is the list of formats which are supported by the codec,
        /// it is terminated by -1 as 0 is a valid format, the formats are ordered by quality.
        /// The first is always the native one.
        /// @note The callback may be called again immediately if initialization for
        /// the selected (hardware-accelerated) pixel format failed.
        /// @warning Behavior is undefined if the callback returns a value not
        /// in the fmt list of formats.
        /// @return the chosen format
        /// - encoding: unused
        /// - decoding: Set by user, if not set the native format will be chosen.
        /// </summary>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public get_format get_format;

        // TODO: THIS IS WHERE YOU LEFT OFF, avcodec.h line 1935 - max_b_frames
    }
}
