/* 
 * This file contains a partial C# implementation of the AVCodecContext struct
 * as defined in avcodec.h of the libavcodec project, for interop use.
 * 
 * Note that unlike everything else in the interop folders, this one isn't a full
 * binding because this struct is like a thousand lines long and I don't need
 * all of the fields to be populated. Thus it's only partially implemented.
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
        /// fourcc (LSB first, so "ABCD" -> ('D'&lt;&lt;24) + ('C'&lt;&lt;16) + ('B'&lt;&lt;8) + 'A').
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

        /// <summary>
        /// maximum number of B-frames between non-B-frames
        /// Note: The output will be delayed by max_b_frames+1 relative to the input.
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public int max_b_frames;

        /// <summary>
        /// qscale factor between IP and B-frames
        /// If &gt; 0 then the last P-frame quantizer will be used(q= lastp_q * factor + offset).
        /// If &lt; 0 then normal ratecontrol will be done (q= -normal_q * factor+offset).
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public float b_quant_factor;

        /// <summary>
        /// use codec private option instead
        /// </summary>
        public int rc_strategy;

        /// <summary>
        /// use encoder private options instead
        /// </summary>
        public int b_frame_strategy;

        /// <summary>
        /// qscale offset between IP and B-frames
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public float b_quant_offset;

        /// <summary>
        /// Size of the frame reordering buffer in the decoder.
        /// For MPEG-2 it is 1 IPB or 0 low delay IP.
        /// - encoding: Set by libavcodec.
        /// - decoding: Set by libavcodec.
        /// </summary>
        public int has_b_frames;

        /// <summary>
        /// use encoder private options instead
        /// </summary>
        public int mpeg_quant;

        /// <summary>
        /// qscale factor between P- and I-frames
        /// If &gt; 0 then the last P-frame quantizer will be used(q = lastp_q * factor + offset).
        /// If &lt; 0 then normal ratecontrol will be done (q= -normal_q * factor+offset).
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public float i_quant_factor;

        /// <summary>
        /// qscale offset between P and I-frames
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public float i_quant_offset;

        /// <summary>
        /// luminance masking (0-> disabled)
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public float lumi_masking;

        /// <summary>
        /// temporary complexity masking (0-> disabled)
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public float temporal_cplx_masking;

        /// <summary>
        /// spatial complexity masking (0-> disabled)
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public float spatial_cplx_masking;

        /// <summary>
        /// p block masking (0-> disabled)
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public float p_masking;

        /// <summary>
        /// darkness masking (0-> disabled)
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public float dark_masking;

        /// <summary>
        /// slice count
        /// - encoding: Set by libavcodec.
        /// - decoding: Set by user(or 0).
        /// </summary>
        public int slice_count;

        /// <summary>
        /// use encoder private options instead
        /// </summary>
        public int prediction_method;

        /// <summary>
        /// slice offsets in the frame in bytes
        /// - encoding: Set/allocated by libavcodec.
        /// - decoding: Set/allocated by user(or NULL).
        /// </summary>
        public IntPtr slice_offset;

        /// <summary>
        /// sample aspect ratio (0 if unknown)
        /// That is the width of a pixel divided by the height of the pixel.
        /// Numerator and denominator must be relatively prime and smaller than 256 for some video standards.
        /// - encoding: Set by user.
        /// - decoding: Set by libavcodec.
        /// </summary>
        public AVRational sample_aspect_ratio;

        /// <summary>
        /// motion estimation comparison function
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public int me_cmp;

        /// <summary>
        /// subpixel motion estimation comparison function
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public int me_sub_cmp;

        /// <summary>
        /// macroblock comparison function (not supported yet)
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public int mb_cmp;

        /// <summary>
        /// interlaced DCT comparison function
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public int ildct_cmp;

        /// <summary>
        /// ME diamond size & shape
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public int dia_size;

        /// <summary>
        /// amount of previous MV predictors (2a+1 x 2a+1 square)
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public int last_predictor_count;

        /// <summary>
        /// use encoder private options instead
        /// </summary>
        public int pre_me;

        /// <summary>
        /// motion estimation prepass comparison function
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public int me_pre_cmp;

        /// <summary>
        /// ME prepass diamond size & shape
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public int pre_dia_size;

        /// <summary>
        /// subpel ME quality
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public int me_subpel_quality;

        /// <summary>
        /// DTG active format information (additional aspect ratio
        /// information only used in DVB MPEG-2 transport streams)
        /// 0 if not set.
        ///
        /// - encoding: unused
        /// - decoding: Set by decoder.
        /// Deprecated in favor of AVSideData
        /// </summary>
        public int dtg_active_format;

        /// <summary>
        /// maximum motion estimation search range in subpel units
        /// If 0 then no limit.
        ///
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public int me_range;

        /// <summary>
        /// use encoder private option instead
        /// </summary>
        public int intra_quant_bias;

        /// <summary>
        /// use encoder private option instead
        /// </summary>
        public int inter_quant_bias;

        /// <summary>
        /// slice flags
        /// - encoding: unused
        /// - decoding: Set by user.
        /// </summary>
        public SLICE_FLAG slice_flags;

        /// <summary>
        /// XVideo Motion Acceleration
        /// - encoding: forbidden
        /// - decoding: set by decoder
        /// XvMC doesn't need it anymore.
        /// </summary>
        public int xvmc_acceleration;

        /// <summary>
        /// macroblock decision mode
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public FF_MB_DECISION mb_decision;

        /// <summary>
        /// custom intra quantization matrix
        /// - encoding: Set by user, can be NULL.
        /// - decoding: Set by libavcodec.
        /// </summary>
        public IntPtr intra_matrix;

        /// <summary>
        /// custom inter quantization matrix
        /// - encoding: Set by user, can be NULL.
        /// - decoding: Set by libavcodec.
        /// </summary>
        public IntPtr inter_matrix;

        /// <summary>
        /// use encoder private options instead
        /// </summary>
        public int scenechange_threshold;

        /// <summary>
        /// use encoder private options instead
        /// </summary>
        public int noise_reduction;

        /// <summary>
        /// this field is unused
        /// </summary>
        public int me_threshold;

        /// <summary>
        /// this field is unused
        /// </summary>
        public int mb_threshold;

        /// <summary>
        /// precision of the intra DC coefficient - 8
        /// - encoding: Set by user.
        /// - decoding: Set by libavcodec
        /// </summary>
        public int intra_dc_precision;

        /// <summary>
        /// Number of macroblock rows at the top which are skipped.
        /// - encoding: unused
        /// - decoding: Set by user.
        /// </summary>
        public int skip_top;

        /// <summary>
        /// Number of macroblock rows at the bottom which are skipped.
        /// - encoding: unused
        /// - decoding: Set by user.
        /// </summary>
        public int skip_bottom;

        /// <summary>
        /// use encoder private options instead
        /// </summary>
        public float border_masking;

        /// <summary>
        /// minimum MB Lagrange multiplier
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public int mb_lmin;

        /// <summary>
        /// maximum MB Lagrange multiplier
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public int mb_lmax;

        /// <summary>
        /// use encoder private options instead
        /// </summary>
        public int me_penalty_compensation;

        /// <summary>
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public int bidir_refine;

        /// <summary>
        /// use encoder private options instead
        /// </summary>
        public int brd_scale;

        /// <summary>
        /// minimum GOP size
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public int keyint_min;

        /// <summary>
        /// number of reference frames
        /// - encoding: Set by user.
        /// - decoding: Set by lavc.
        /// </summary>
        public int refs;

        /// <summary>
        /// use encoder private options instead
        /// </summary>
        public int chromaoffset;

        /// <summary>
        /// Multiplied by qscale for each frame and added to scene_change_score.
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public int scenechange_factor;

        /// <summary>
        /// Note: Value depends upon the compare function used for fullpel ME.
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public int mv0_threshold;

        /// <summary>
        /// use encoder private options instead
        /// </summary>
        public int b_sensitivity;

        /// <summary>
        /// Chromaticity coordinates of the source primaries.
        /// - encoding: Set by user
        /// - decoding: Set by libavcodec
        /// </summary>
        public AVColorPrimaries color_primaries;

        /// <summary>
        /// Color Transfer Characteristic.
        /// - encoding: Set by user
        /// - decoding: Set by libavcodec
        /// </summary>
        public AVColorTransferCharacteristic color_trc;

        /// <summary>
        /// YUV colorspace type.
        /// - encoding: Set by user
        /// - decoding: Set by libavcodec
        /// </summary>
        public AVColorSpace colorspace;

        /// <summary>
        /// MPEG vs JPEG YUV range.
        /// - encoding: Set by user
        /// - decoding: Set by libavcodec
        /// </summary>
        public AVColorRange color_range;

        /// <summary>
        /// This defines the location of chroma samples.
        /// - encoding: Set by user
        /// - decoding: Set by libavcodec
        /// </summary>
        public AVChromaLocation chroma_sample_location;

        /// <summary>
        /// Number of slices.
        /// Indicates number of picture subdivisions.Used for parallelized decoding.
        /// - encoding: Set by user
        /// - decoding: unused
        /// </summary>
        public int slices;

        /// <summary>
        /// Field order
        /// - encoding: set by libavcodec
        /// - decoding: Set by user.
        /// </summary>
        public AVFieldOrder field_order;

        /// <summary>
        /// samples per second
        /// </summary>
        public int sample_rate;

        /// <summary>
        /// number of audio channels
        /// </summary>
        public int channels;

        /// <summary>
        /// audio sample format
        /// - encoding: Set by user.
        /// - decoding: Set by libavcodec.
        /// </summary>
        public AVSampleFormat sample_fmt;

        /// <summary>
        /// Number of samples per channel in an audio frame.
        /// 
        /// - encoding: set by libavcodec in avcodec_open2(). Each submitted frame
        ///   except the last must contain exactly frame_size samples per channel.
        ///   May be 0 when the codec has AV_CODEC_CAP_VARIABLE_FRAME_SIZE set, then the
        ///   frame size is not restricted.
        /// - decoding: may be set by some decoders to indicate constant frame size
        /// </summary>
        public int frame_size;

        /// <summary>
        /// Frame counter, set by libavcodec.
        /// The counter is not incremented if encoding/decoding resulted in
        /// an error.
        ///
        /// - decoding: total number of frames returned from the decoder so far.
        /// - encoding: total number of frames passed to the encoder so far.
        /// </summary>
        public int frame_number;

        /// <summary>
        /// number of bytes per packet if constant and known or 0
        /// Used by some WAV based audio codecs.
        /// </summary>
        public int block_align;

        /// <summary>
        /// Audio cutoff bandwidth (0 means "automatic")
        /// - encoding: Set by user.
        /// - decoding: unused
        /// </summary>
        public int cutoff;

        /// <summary>
        /// Audio channel layout.
        /// - encoding: set by user.
        /// - decoding: set by user, may be overwritten by libavcodec.
        /// </summary>
        public AV_CH_LAYOUT channel_layout;

        // Left off at avcodec.h line 2468.
        // If I need more stuff from it, I'll finish implementing it later
        // but right now this is sufficient.
    }
}
