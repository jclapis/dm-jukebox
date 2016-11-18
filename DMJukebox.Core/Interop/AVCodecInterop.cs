using System;
using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a utility class that holds the P/Invoke wrappers for libavcodec.
    /// </summary>
    internal static class AVCodecInterop
    {
        /// <summary>
        /// The DLL for Windows
        /// </summary>
        private const string WindowsAVCodecLibrary = "avcodec-57.dll";

        /// <summary>
        /// The SO for Linux
        /// </summary>
        private const string LinuxAVCodecLibrary = "avcodec-57.so";

        /// <summary>
        /// The Dylib for OSX
        /// </summary>
        private const string MacAVCodecLibrary = "avcodec-57.dylib";

        // These regions contain the DllImport function definitions for each OS. Since we can't really set
        // the path of DllImport dynamically (and loading them dynamically using LoadLibrary / dlopen is complicated
        // to manage cross-platform), we have to pre-define them based on the names of the libraries above.

        #region Windows Functions

        [DllImport(WindowsAVCodecLibrary, EntryPoint = nameof(avcodec_find_decoder), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr avcodec_find_decoder_windows(AVCodecID id);

        [DllImport(WindowsAVCodecLibrary, EntryPoint = nameof(avcodec_open2), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR avcodec_open2_windows(IntPtr avctx, IntPtr codec, ref IntPtr options);
        
        [DllImport(WindowsAVCodecLibrary, EntryPoint = nameof(avcodec_send_packet), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR avcodec_send_packet_windows(IntPtr avctx, IntPtr avpkt);

        [DllImport(WindowsAVCodecLibrary, EntryPoint = nameof(avcodec_receive_frame), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR avcodec_receive_frame_windows(IntPtr avctx, IntPtr frame);

        [DllImport(WindowsAVCodecLibrary, EntryPoint = nameof(av_packet_alloc), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr av_packet_alloc_windows();

        [DllImport(WindowsAVCodecLibrary, EntryPoint = nameof(av_packet_free), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_packet_free_windows(ref IntPtr pkt);

        [DllImport(WindowsAVCodecLibrary, EntryPoint = nameof(av_new_packet), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR av_new_packet_windows(IntPtr pkt, int size);

        [DllImport(WindowsAVCodecLibrary, EntryPoint = nameof(av_init_packet), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_init_packet_windows(IntPtr pkt);

        [DllImport(WindowsAVCodecLibrary, EntryPoint = nameof(av_packet_unref), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_packet_unref_windows(IntPtr pkt);

        #endregion

        #region Linux Functions

        [DllImport(LinuxAVCodecLibrary, EntryPoint = nameof(avcodec_find_decoder), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr avcodec_find_decoder_linux(AVCodecID id);

        [DllImport(LinuxAVCodecLibrary, EntryPoint = nameof(avcodec_open2), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR avcodec_open2_linux(IntPtr avctx, IntPtr codec, ref IntPtr options);

        [DllImport(LinuxAVCodecLibrary, EntryPoint = nameof(avcodec_send_packet), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR avcodec_send_packet_linux(IntPtr avctx, IntPtr avpkt);

        [DllImport(LinuxAVCodecLibrary, EntryPoint = nameof(avcodec_receive_frame), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR avcodec_receive_frame_linux(IntPtr avctx, IntPtr frame);

        [DllImport(LinuxAVCodecLibrary, EntryPoint = nameof(av_packet_alloc), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr av_packet_alloc_linux();

        [DllImport(LinuxAVCodecLibrary, EntryPoint = nameof(av_packet_free), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_packet_free_linux(ref IntPtr pkt);

        [DllImport(LinuxAVCodecLibrary, EntryPoint = nameof(av_new_packet), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR av_new_packet_linux(IntPtr pkt, int size);

        [DllImport(LinuxAVCodecLibrary, EntryPoint = nameof(av_init_packet), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_init_packet_linux(IntPtr pkt);

        [DllImport(LinuxAVCodecLibrary, EntryPoint = nameof(av_packet_unref), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_packet_unref_linux(IntPtr pkt);

        #endregion

        #region OSX Functions

        [DllImport(MacAVCodecLibrary, EntryPoint = nameof(avcodec_find_decoder), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr avcodec_find_decoder_osx(AVCodecID id);

        [DllImport(MacAVCodecLibrary, EntryPoint = nameof(avcodec_open2), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR avcodec_open2_osx(IntPtr avctx, IntPtr codec, ref IntPtr options);

        [DllImport(MacAVCodecLibrary, EntryPoint = nameof(avcodec_send_packet), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR avcodec_send_packet_osx(IntPtr avctx, IntPtr avpkt);

        [DllImport(MacAVCodecLibrary, EntryPoint = nameof(avcodec_receive_frame), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR avcodec_receive_frame_osx(IntPtr avctx, IntPtr frame);

        [DllImport(MacAVCodecLibrary, EntryPoint = nameof(av_packet_alloc), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr av_packet_alloc_osx();

        [DllImport(MacAVCodecLibrary, EntryPoint = nameof(av_packet_free), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_packet_free_osx(ref IntPtr pkt);

        [DllImport(MacAVCodecLibrary, EntryPoint = nameof(av_new_packet), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern AVERROR av_new_packet_osx(IntPtr pkt, int size);

        [DllImport(MacAVCodecLibrary, EntryPoint = nameof(av_init_packet), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_init_packet_osx(IntPtr pkt);

        [DllImport(MacAVCodecLibrary, EntryPoint = nameof(av_packet_unref), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_packet_unref_osx(IntPtr pkt);

        #endregion

        #region Delegates and Platform-Dependent Loading

        // These delegates all represent the function signatures for the libavcodec methods I need to call.

        private delegate IntPtr avcodec_find_decoder_delegate(AVCodecID id);
        private delegate AVERROR avcodec_open2_delegate(IntPtr avctx, IntPtr codec, ref IntPtr options);
        private delegate AVERROR avcodec_send_packet_delegate(IntPtr avctx, IntPtr avpkt);
        private delegate AVERROR avcodec_receive_frame_delegate(IntPtr avctx, IntPtr frame);
        private delegate IntPtr av_packet_alloc_delegate();
        private delegate void av_packet_free_delegate(ref IntPtr pkt);
        private delegate AVERROR av_new_packet_delegate(IntPtr pkt, int size);
        private delegate void av_init_packet_delegate(IntPtr pkt);
        private delegate void av_packet_unref_delegate(IntPtr pkt);

        // These fields represent function pointers towards each of the extern functions. They get set
        // to the proper platform-specific functions by the static constructor. For example, if this is
        // running on a Windows machine, each of these pointers will point to the various avcodec_XXX_windows
        // extern functions listed above.

        private static avcodec_find_decoder_delegate avcodec_find_decoder_impl;
        private static avcodec_open2_delegate avcodec_open2_impl;
        private static avcodec_send_packet_delegate avcodec_send_packet_impl;
        private static avcodec_receive_frame_delegate avcodec_receive_frame_impl;
        private static av_packet_alloc_delegate av_packet_alloc_impl;
        private static av_packet_free_delegate av_packet_free_impl;
        private static av_new_packet_delegate av_new_packet_func;
        private static av_init_packet_delegate av_init_packet_impl;
        private static av_packet_unref_delegate av_packet_unref_impl;

        /// <summary>
        /// The static constructor figures out which library to use for P/Invoke based
        /// on the current OS platform.
        /// </summary>
        static AVCodecInterop()
        {
            NativePathFinder.AddNativeLibraryPathToEnvironmentVariable();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                avcodec_find_decoder_impl = avcodec_find_decoder_windows;
                avcodec_open2_impl = avcodec_open2_windows;
                avcodec_send_packet_impl = avcodec_send_packet_windows;
                avcodec_receive_frame_impl = avcodec_receive_frame_windows;
                av_packet_alloc_impl = av_packet_alloc_windows;
                av_packet_free_impl = av_packet_free_windows;
                av_new_packet_func = av_new_packet_windows;
                av_init_packet_impl = av_init_packet_windows;
                av_packet_unref_impl = av_packet_unref_windows;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                avcodec_find_decoder_impl = avcodec_find_decoder_linux;
                avcodec_open2_impl = avcodec_open2_linux;
                avcodec_send_packet_impl = avcodec_send_packet_linux;
                avcodec_receive_frame_impl = avcodec_receive_frame_linux;
                av_packet_alloc_impl = av_packet_alloc_linux;
                av_packet_free_impl = av_packet_free_linux;
                av_new_packet_func = av_new_packet_linux;
                av_init_packet_impl = av_init_packet_linux;
                av_packet_unref_impl = av_packet_unref_linux;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                avcodec_find_decoder_impl = avcodec_find_decoder_osx;
                avcodec_open2_impl = avcodec_open2_osx;
                avcodec_send_packet_impl = avcodec_send_packet_osx;
                avcodec_receive_frame_impl = avcodec_receive_frame_osx;
                av_packet_alloc_impl = av_packet_alloc_osx;
                av_packet_free_impl = av_packet_free_osx;
                av_new_packet_func = av_new_packet_osx;
                av_init_packet_impl = av_init_packet_osx;
                av_packet_unref_impl = av_packet_unref_osx;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region Public API

        /// <summary>
        /// Find a registered decoder with a matching codec ID.
        /// </summary>
        /// <param name="id">AVCodecID of the requested decoder</param>
        /// <returns>(AVCodec) A decoder if one was found, NULL otherwise.</returns>
        public static IntPtr avcodec_find_decoder(AVCodecID id)
        {
            return avcodec_find_decoder_impl(id);
        }

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
        public static AVERROR avcodec_open2(IntPtr avctx, IntPtr codec, ref IntPtr options)
        {
            return avcodec_open2_impl(avctx, codec, ref options);
        }

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
        public static AVERROR avcodec_send_packet(IntPtr avctx, IntPtr avpkt)
        {
            return avcodec_send_packet_impl(avctx, avpkt);
        }

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
        public static AVERROR avcodec_receive_frame(IntPtr avctx, IntPtr frame)
        {
            return avcodec_receive_frame_impl(avctx, frame);
        }

        /// <summary>
        /// Allocate an AVPacket and set its fields to default values. The resulting
        /// struct must be freed using av_packet_free().
        /// 
        /// This only allocates the AVPacket itself, not the data buffers. Those
        /// must be allocated through other means such as av_new_packet.
        /// </summary>
        /// <returns>(AVPacket) An AVPacket filled with default values or NULL on failure.</returns>
        public static IntPtr av_packet_alloc()
        {
            return av_packet_alloc_impl();
        }

        /// <summary>
        /// Free the packet, if the packet is reference counted, it will be
        /// unreferenced first.
        /// 
        /// Passing NULL is a no-op.
        /// </summary>
        /// <param name="pkt">(AVPacket) packet to be freed. The pointer will be set to NULL.</param>
        public static void av_packet_free(ref IntPtr pkt)
        {
            av_packet_free_impl(ref pkt);
        }

        /// <summary>
        /// Allocate the payload of a packet and initialize its fields with
        /// default values.
        /// </summary>
        /// <param name="pkt">(AVPacket) packet</param>
        /// <param name="size">wanted payload size</param>
        /// <returns>0 if OK, AVERROR_xxx otherwise</returns>
        public static AVERROR av_new_packet(IntPtr pkt, int size)
        {
            return av_new_packet_func(pkt, size);
        }
        
        public static void av_init_packet(IntPtr pkt)
        {
            av_init_packet_impl(pkt);
        }

        /// <summary>
        /// Wipe the packet.
        ///
        /// Unreference the buffer referenced by the packet and reset the
        /// remaining packet fields to their default values.
        /// </summary>
        /// <param name="pkt">The packet to be unreferenced.</param>
        public static void av_packet_unref(IntPtr pkt)
        {
            av_packet_unref_impl(pkt);
        }

        #endregion
    }
}
