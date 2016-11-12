using System;
using System.Runtime.InteropServices;

namespace DiscordJukebox.Interop
{
    internal static class AVCodecInterop
    {
        private const string AVCodecDll = "avcodec-57.dll";

        public const int AV_INPUT_BUFFER_PADDING_SIZE = 32;

        public const int AVCODEC_MAX_AUDIO_FRAME_SIZE = 192000;

        /// <summary>
        /// Find a registered decoder with a matching codec ID.
        /// </summary>
        /// <param name="id">AVCodecID of the requested decoder</param>
        /// <returns>(AVCodec) A decoder if one was found, NULL otherwise.</returns>
        [DllImport(AVCodecDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr avcodec_find_decoder(AVCodecID id);

        /// <summary>
        /// Initialize the AVCodecContext to use the given AVCodec. Prior to using this
        /// function the context has to be allocated with avcodec_alloc_context3().
        ///
        /// The functions avcodec_find_decoder_by_name(), avcodec_find_encoder_by_name(),
        /// avcodec_find_decoder() and avcodec_find_encoder() provide an easy way for
        /// retrieving a codec.
        ///
        /// This function is not thread safe!
        ///
        /// Always call this function before using decoding routines(such as
        /// <see cref="avcodec_receive_frame"/>()).
        /// </summary>
        /// <param name="avctx">(AVCodecContext) The context to initialize.</param>
        /// <param name="codec">(AVCodec) The codec to open this context for. If a non-NULL codec has been
        /// previously passed to avcodec_alloc_context3() or
        /// for this context, then this parameter MUST be either NULL or
        /// equal to the previously passed codec.</param>
        /// <param name="options">(AVDictionary) A dictionary filled with AVCodecContext and codec-private options.
        /// On return this object will be filled with options that were not found.</param>
        /// <returns>zero on success, a negative value on error</returns>
        [DllImport(AVCodecDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR avcodec_open2(IntPtr avctx, IntPtr codec, ref IntPtr options);

        /// <summary>
        /// Supply raw packet data as input to a decoder.
        ///
        /// Internally, this call will copy relevant AVCodecContext fields, which can
        /// influence decoding per-packet, and apply them when the packet is actually
        /// decoded. (For example AVCodecContext.skip_frame, which might direct the
        /// decoder to drop the frame contained by the packet sent with this function.)
        ///
        /// The input buffer, avpkt->data must be AV_INPUT_BUFFER_PADDING_SIZE
        /// larger than the actual read bytes because some optimized bitstream
        /// readers read 32 or 64 bits at once and could read over the end.
        ///
        /// Do not mix this API with the legacy API (like avcodec_decode_video2())
        /// on the same AVCodecContext. It will return unexpected results now
        /// or in future libavcodec versions.
        ///
        /// The AVCodecContext MUST have been opened with @ref avcodec_open2()
        /// before packets may be fed to the decoder.
        /// </summary>
        /// <param name="avctx">(AVCodecContext) codec context</param>
        /// <param name="avpkt">(AVPacket) The input AVPacket. Usually, this will be a single video
        /// frame, or several complete audio frames.
        /// Ownership of the packet remains with the caller, and the
        /// decoder will not write to the packet. The decoder may create
        /// a reference to the packet data (or copy it if the packet is
        /// not reference-counted).
        /// Unlike with older APIs, the packet is always fully consumed,
        /// and if it contains multiple frames (e.g. some audio codecs),
        /// will require you to call avcodec_receive_frame() multiple
        /// times afterwards before you can send a new packet.
        /// It can be NULL (or an AVPacket with data set to NULL and
        /// size set to 0); in this case, it is considered a flush
        /// packet, which signals the end of the stream. Sending the
        /// first flush packet will return success. Subsequent ones are
        /// unnecessary and will return AVERROR_EOF. If the decoder
        /// still has frames buffered, it will return them after sending
        /// a flush packet.</param>
        /// <returns>
        /// 0 on success, otherwise negative error code:
        /// AVERROR(EAGAIN): input is not accepted right now - the packet must be
        /// resent after trying to read output
        /// AVERROR_EOF: the decoder has been flushed, and no new packets can
        /// be sent to it (also returned if more than 1 flush packet is sent)
        /// AVERROR(EINVAL): codec not opened, it is an encoder, or requires flush
        /// AVERROR(ENOMEM): failed to add packet to internal queue, or similar
        /// other errors: legitimate decoding errors</returns>
        [DllImport(AVCodecDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR avcodec_send_packet(IntPtr avctx, IntPtr avpkt);

        /// <summary>
        /// Return decoded output data from a decoder.
        /// </summary>
        /// <param name="avctx">(AVCodecContext) codec context</param>
        /// <param name="frame">(AVFrame) This will be set to a reference-counted video or audio
        /// frame (depending on the decoder type) allocated by the
        /// decoder. Note that the function will always call
        /// av_frame_unref(frame) before doing anything else.</param>
        /// <returns>
        /// 0: success, a frame was returned
        /// AVERROR(EAGAIN): output is not available right now - user must try to send new input
        /// AVERROR_EOF: the decoder has been fully flushed, and there will be no more output frames
        /// AVERROR(EINVAL): codec not opened, or it is an encoder
        /// other negative values: legitimate decoding errors</returns>
        [DllImport(AVCodecDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR avcodec_receive_frame(IntPtr avctx, IntPtr frame);

        /// <summary>
        /// Allocate an AVPacket and set its fields to default values. The resulting
        /// struct must be freed using av_packet_free().
        /// 
        /// This only allocates the AVPacket itself, not the data buffers. Those
        /// must be allocated through other means such as av_new_packet.
        /// </summary>
        /// <returns>(AVPacket) An AVPacket filled with default values or NULL on failure.</returns>
        [DllImport(AVCodecDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr av_packet_alloc();

        /// <summary>
        /// Free the packet, if the packet is reference counted, it will be
        /// unreferenced first.
        /// 
        /// Passing NULL is a no-op.
        /// </summary>
        /// <param name="pkt">(AVPacket) packet to be freed. The pointer will be set to NULL.</param>
        [DllImport(AVCodecDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_packet_free(ref IntPtr pkt);

        /// <summary>
        /// Allocate the payload of a packet and initialize its fields with
        /// default values.
        /// </summary>
        /// <param name="pkt">(AVPacket) packet</param>
        /// <param name="size">wanted payload size</param>
        /// <returns>0 if OK, AVERROR_xxx otherwise</returns>
        [DllImport(AVCodecDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR av_new_packet(IntPtr pkt, int size);

        [DllImport(AVCodecDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_init_packet(IntPtr pkt);
    }
}
