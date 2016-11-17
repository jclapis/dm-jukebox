using System;

namespace DMJukebox.Interop
{
    /// <summary>
    /// encoding support
    /// These flags can be passed in AVCodecContext.flags before initialization.
    /// Note: Not everything is supported yet.
    /// </summary>
    [Flags]
    internal enum AV_CODEC_FLAG : uint
    {
        /// <summary>
        /// Allow decoders to produce frames with data planes that are not aligned
        /// to CPU requirements (e.g. due to cropping).
        /// </summary>
        AV_CODEC_FLAG_UNALIGNED = 0x1,

        /// <summary>
        /// Use fixed qscale.
        /// </summary>
        AV_CODEC_FLAG_QSCALE = 0x2,

        /// <summary>
        /// 4 MV per MB allowed / advanced prediction for H.263.
        /// </summary>
        AV_CODEC_FLAG_4MV = 0x4,

        /// <summary>
        /// Output even those frames that might be corrupted.
        /// </summary>
        AV_CODEC_FLAG_OUTPUT_CORRUPT = 0x8,

        /// <summary>
        /// Use qpel MC.
        /// </summary>
        AV_CODEC_FLAG_QPEL = 0x10,

        /// <summary>
        /// Use internal 2pass ratecontrol in first pass mode.
        /// </summary>
        AV_CODEC_FLAG_PASS1 = 0x200,

        /// <summary>
        /// Use internal 2pass ratecontrol in second pass mode.
        /// </summary>
        AV_CODEC_FLAG_PASS2 = 0x400,

        /// <summary>
        /// loop filter.
        /// </summary>
        AV_CODEC_FLAG_LOOP_FILTER = 0x800,

        /// <summary>
        /// Only decode/encode grayscale.
        /// </summary>
        AV_CODEC_FLAG_GRAY = 0x2000,

        /// <summary>
        /// error[?] variables will be set during encoding.
        /// </summary>
        AV_CODEC_FLAG_PSNR = 0x8000,

        /// <summary>
        /// Input bitstream might be truncated at a random location
        /// instead of only at frame boundaries.
        /// </summary>
        AV_CODEC_FLAG_TRUNCATED = 0x10000,

        /// <summary>
        /// Use interlaced DCT.
        /// </summary>
        AV_CODEC_FLAG_INTERLACED_DCT = 0x40000,

        /// <summary>
        /// Force low delay.
        /// </summary>
        AV_CODEC_FLAG_LOW_DELAY = 0x80000,

        /// <summary>
        /// Place global headers in extradata instead of every keyframe.
        /// </summary>
        AV_CODEC_FLAG_GLOBAL_HEADER = 0x400000,

        /// <summary>
        /// Use only bitexact stuff (except (I)DCT).
        /// </summary>
        AV_CODEC_FLAG_BITEXACT = 0x800000,

        /// <summary>
        /// H.263 advanced intra coding / MPEG-4 AC prediction
        /// </summary>
        AV_CODEC_FLAG_AC_PRED = 0x1000000,

        /// <summary>
        /// interlaced motion estimation
        /// </summary>
        AV_CODEC_FLAG_INTERLACED_ME = 0x20000000,
        AV_CODEC_FLAG_CLOSED_GOP = 0x80000000
    }
}
