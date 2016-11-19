/* 
 * This file contains a C# implementation of the AV_CODEC_CAP enum
 * as defined in avcodec.h of the libavcodec project, for interop use.
 * It isn't technically an enum in ffmpeg, just a bunch of macros.
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

namespace DMJukebox.Interop
{
    /// <summary>
    /// codec capabilities
    /// </summary>
    [Flags]
    internal enum AV_CODEC_CAP : uint
    {
        /// <summary>
        /// Decoder can use draw_horiz_band callback.
        /// </summary>
        AV_CODEC_CAP_DRAW_HORIZ_BAND = 0x1,

        /// <summary>
        /// Codec uses get_buffer() for allocating buffers and supports custom allocators.
        /// If not set, it might not use get_buffer() at all or use operations that
        /// assume the buffer was allocated by avcodec_default_get_buffer.
        /// </summary>
        AV_CODEC_CAP_DR1 = 0x2,

        AV_CODEC_CAP_TRUNCATED = 0x8,

        /// <summary>
        /// Encoder or decoder requires flushing with NULL input at the end in order to
        /// give the complete and correct output.
        ///
        /// NOTE: If this flag is not set, the codec is guaranteed to never be fed with
        ///       with NULL data. The user can still send NULL data to the public encode
        ///       or decode function, but libavcodec will not pass it along to the codec
        ///       unless this flag is set.
        ///
        /// Decoders:
        /// The decoder has a non-zero delay and needs to be fed with avpkt->data=NULL,
        /// avpkt->size=0 at the end to get the delayed data until the decoder no longer
        /// returns frames.
        ///
        /// Encoders:
        /// The encoder needs to be fed with NULL data at the end of encoding until the
        /// encoder no longer returns data.
        ///
        /// NOTE: For encoders implementing the AVCodec.encode2() function, setting this
        ///       flag also means that the encoder must set the pts and duration for
        ///       each output packet. If this flag is not set, the pts and duration will
        ///       be determined by libavcodec from the input frame.
        /// </summary>
        AV_CODEC_CAP_DELAY = 0x20,

        /// <summary>
        /// Codec can be fed a final frame with a smaller size.
        /// This can be used to prevent truncation of the last audio samples.
        /// </summary>
        AV_CODEC_CAP_SMALL_LAST_FRAME = 0x40,

        /// <summary>
        /// Codec can export data for HW decoding (VDPAU).
        /// </summary>
        AV_CODEC_CAP_HWACCEL_VDPAU = 0x80,

        /// <summary>
        /// Codec can output multiple frames per AVPacket
        /// Normally demuxers return one frame at a time, demuxers which do not do
        /// are connected to a parser to split what they return into proper frames.
        /// This flag is reserved to the very rare category of codecs which have a
        /// bitstream that cannot be split into frames without timeconsuming
        /// operations like full decoding. Demuxers carrying such bitstreams thus
        /// may return multiple frames in a packet. This has many disadvantages like
        /// prohibiting stream copy in many cases thus it should only be considered
        /// as a last resort.
        /// </summary>
        AV_CODEC_CAP_SUBFRAMES = 0x100,

        /// <summary>
        /// Codec is experimental and is thus avoided in favor of non experimental
        /// encoders
        /// </summary>
        AV_CODEC_CAP_EXPERIMENTAL = 0x200,

        /// <summary>
        /// Codec should fill in channel configuration and samplerate instead of container
        /// </summary>
        AV_CODEC_CAP_CHANNEL_CONF = 0x400,

        /// <summary>
        /// Codec supports frame-level multithreading.
        /// </summary>
        AV_CODEC_CAP_FRAME_THREADS = 0x1000,

        /// <summary>
        /// Codec supports slice-based (or partition-based) multithreading.
        /// </summary>
        AV_CODEC_CAP_SLICE_THREADS = 0x2000,

        /// <summary>
        /// Codec supports changed parameters at any point.
        /// </summary>
        AV_CODEC_CAP_PARAM_CHANGE = 0x4000,

        /// <summary>
        /// Codec supports avctx->thread_count == 0 (auto).
        /// </summary>
        AV_CODEC_CAP_AUTO_THREADS = 0x8000,

        /// <summary>
        /// Audio encoder supports receiving a different number of samples in each call.
        /// </summary>
        AV_CODEC_CAP_VARIABLE_FRAME_SIZE = 0x10000,

        /// <summary>
        /// Decoder is not a preferred choice for probing.
        /// This indicates that the decoder is not a good choice for probing.
        /// It could for example be an expensive to spin up hardware decoder,
        /// or it could simply not provide a lot of useful information about
        /// the stream.
        /// A decoder marked with this flag should only be used as last resort
        /// choice for probing.
        /// </summary>
        AV_CODEC_CAP_AVOID_PROBING = 0x20000,

        /// <summary>
        /// Codec is intra only.
        /// </summary>
        AV_CODEC_CAP_INTRA_ONLY = 0x40000000,

        /// <summary>
        /// Codec is lossless.
        /// </summary>
        AV_CODEC_CAP_LOSSLESS = 0x80000000
    }
}
