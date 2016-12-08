/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

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
    /// This is a C# implementation of the AVCodecContext struct in FFmpeg.
    /// It describes many of the details for a particular audio or video stream
    /// contained within a media file. I only use it for audio tracks, of course.
    /// Note that this isn't a complete implementation; the actual struct in
    /// FFmpeg is about 2000 lines long and I only need part of it for this
    /// project.
    /// </summary>
    /// <remarks>
    /// This struct is defined in avcodec.h of the libavcodec project.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct AVCodecContext
    {
        public IntPtr av_class;

        public int log_level_offset;
        
        public AVMediaType codec_type;

        public IntPtr codec;
        
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string codec_name;
        
        public AVCodecID codec_id;
        
        public uint codec_tag;
        
        public uint stream_codec_tag;

        public IntPtr priv_data;
        
        public IntPtr @internal;
        
        public IntPtr opaque;
        
        public long bit_rate;
        
        public int bit_rate_tolerance;
        
        public int global_quality;
        
        public int compression_level;
        
        public AV_CODEC_FLAG flags;
        
        public AV_CODEC_FLAG2 flags2;
        
        public IntPtr extradata;

        public int extradata_size;
        
        public AVRational time_base;
        
        public int ticks_per_frame;
        
        public int delay;
        
        public int width;

        public int height;
        
        public int coded_width;

        public int coded_height;
        
        public int gop_size;
        
        public AVPixelFormat pix_fmt;
        
        public int me_method;
        
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public draw_horiz_band draw_horiz_band;
        
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public get_format get_format;
        
        public int max_b_frames;
        
        public float b_quant_factor;
        
        public int rc_strategy;
        
        public int b_frame_strategy;
        
        public float b_quant_offset;
        
        public int has_b_frames;
        
        public int mpeg_quant;
        
        public float i_quant_factor;
        
        public float i_quant_offset;
        
        public float lumi_masking;
        
        public float temporal_cplx_masking;
        
        public float spatial_cplx_masking;
        
        public float p_masking;
        
        public float dark_masking;
        
        public int slice_count;
        
        public int prediction_method;
        
        public IntPtr slice_offset;
        
        public AVRational sample_aspect_ratio;
        
        public int me_cmp;
        
        public int me_sub_cmp;
        
        public int mb_cmp;
        
        public int ildct_cmp;
        
        public int dia_size;
        
        public int last_predictor_count;
        
        public int pre_me;
        
        public int me_pre_cmp;
        
        public int pre_dia_size;
        
        public int me_subpel_quality;
        
        public int dtg_active_format;
        
        public int me_range;
        
        public int intra_quant_bias;
        
        public int inter_quant_bias;
        
        public SLICE_FLAG slice_flags;
        
        public int xvmc_acceleration;
        
        public FF_MB_DECISION mb_decision;
        
        public IntPtr intra_matrix;
        
        public IntPtr inter_matrix;
        
        public int scenechange_threshold;
        
        public int noise_reduction;
        
        public int me_threshold;
        
        public int mb_threshold;
        
        public int intra_dc_precision;
        
        public int skip_top;
        
        public int skip_bottom;
        
        public float border_masking;
        
        public int mb_lmin;
        
        public int mb_lmax;
        
        public int me_penalty_compensation;
        
        public int bidir_refine;
        
        public int brd_scale;
        
        public int keyint_min;
        
        public int refs;
        
        public int chromaoffset;
        
        public int scenechange_factor;
        
        public int mv0_threshold;
        
        public int b_sensitivity;
        
        public AVColorPrimaries color_primaries;
        
        public AVColorTransferCharacteristic color_trc;
        
        public AVColorSpace colorspace;
        
        public AVColorRange color_range;
        
        public AVChromaLocation chroma_sample_location;
        
        public int slices;
        
        public AVFieldOrder field_order;
        
        public int sample_rate;
        
        public int channels;
        
        public AVSampleFormat sample_fmt;
        
        public int frame_size;
        
        public int frame_number;
        
        public int block_align;
        
        public int cutoff;
        
        public AV_CH_LAYOUT channel_layout;

        // Left off at avcodec.h line 2468.
        // If I need more stuff from it, I'll finish implementing it later
        // but right now this is sufficient.
    }
}
