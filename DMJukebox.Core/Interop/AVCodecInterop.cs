/* ========================================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * ====================================================================== */

using System;
using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a utility class that holds the P/Invoke wrappers for libavcodec.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
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
        private static extern AVERROR avcodec_receive_frame_windows(IntPtr avctx, IntPtr frame);

        [DllImport(WindowsAVCodecLibrary, EntryPoint = nameof(av_packet_alloc), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr av_packet_alloc_windows();

        [DllImport(WindowsAVCodecLibrary, EntryPoint = nameof(av_packet_free), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_packet_free_windows(ref IntPtr pkt);

        [DllImport(WindowsAVCodecLibrary, EntryPoint = nameof(av_new_packet), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR av_new_packet_windows(IntPtr pkt, int size);

        [DllImport(WindowsAVCodecLibrary, EntryPoint = nameof(av_init_packet), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_init_packet_windows(IntPtr pkt);

        [DllImport(WindowsAVCodecLibrary, EntryPoint = nameof(av_packet_unref), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_packet_unref_windows(IntPtr pkt);

        [DllImport(WindowsAVCodecLibrary, EntryPoint = nameof(avcodec_flush_buffers), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void avcodec_flush_buffers_windows(IntPtr avctx);

        [DllImport(WindowsAVCodecLibrary, EntryPoint = nameof(av_get_exact_bits_per_sample), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int av_get_exact_bits_per_sample_windows(AVCodecID codec_id);

        #endregion

        #region Linux Functions

        [DllImport(LinuxAVCodecLibrary, EntryPoint = nameof(avcodec_find_decoder), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr avcodec_find_decoder_linux(AVCodecID id);

        [DllImport(LinuxAVCodecLibrary, EntryPoint = nameof(avcodec_open2), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR avcodec_open2_linux(IntPtr avctx, IntPtr codec, ref IntPtr options);

        [DllImport(LinuxAVCodecLibrary, EntryPoint = nameof(avcodec_send_packet), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR avcodec_send_packet_linux(IntPtr avctx, IntPtr avpkt);

        [DllImport(LinuxAVCodecLibrary, EntryPoint = nameof(avcodec_receive_frame), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR avcodec_receive_frame_linux(IntPtr avctx, IntPtr frame);

        [DllImport(LinuxAVCodecLibrary, EntryPoint = nameof(av_packet_alloc), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr av_packet_alloc_linux();

        [DllImport(LinuxAVCodecLibrary, EntryPoint = nameof(av_packet_free), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_packet_free_linux(ref IntPtr pkt);

        [DllImport(LinuxAVCodecLibrary, EntryPoint = nameof(av_new_packet), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR av_new_packet_linux(IntPtr pkt, int size);

        [DllImport(LinuxAVCodecLibrary, EntryPoint = nameof(av_init_packet), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_init_packet_linux(IntPtr pkt);

        [DllImport(LinuxAVCodecLibrary, EntryPoint = nameof(av_packet_unref), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_packet_unref_linux(IntPtr pkt);

        [DllImport(LinuxAVCodecLibrary, EntryPoint = nameof(avcodec_flush_buffers), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void avcodec_flush_buffers_linux(IntPtr avctx);

        [DllImport(LinuxAVCodecLibrary, EntryPoint = nameof(av_get_exact_bits_per_sample), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int av_get_exact_bits_per_sample_linux(AVCodecID codec_id);

        #endregion

        #region OSX Functions

        [DllImport(MacAVCodecLibrary, EntryPoint = nameof(avcodec_find_decoder), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr avcodec_find_decoder_osx(AVCodecID id);

        [DllImport(MacAVCodecLibrary, EntryPoint = nameof(avcodec_open2), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR avcodec_open2_osx(IntPtr avctx, IntPtr codec, ref IntPtr options);

        [DllImport(MacAVCodecLibrary, EntryPoint = nameof(avcodec_send_packet), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR avcodec_send_packet_osx(IntPtr avctx, IntPtr avpkt);

        [DllImport(MacAVCodecLibrary, EntryPoint = nameof(avcodec_receive_frame), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR avcodec_receive_frame_osx(IntPtr avctx, IntPtr frame);

        [DllImport(MacAVCodecLibrary, EntryPoint = nameof(av_packet_alloc), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr av_packet_alloc_osx();

        [DllImport(MacAVCodecLibrary, EntryPoint = nameof(av_packet_free), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_packet_free_osx(ref IntPtr pkt);

        [DllImport(MacAVCodecLibrary, EntryPoint = nameof(av_new_packet), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern AVERROR av_new_packet_osx(IntPtr pkt, int size);

        [DllImport(MacAVCodecLibrary, EntryPoint = nameof(av_init_packet), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_init_packet_osx(IntPtr pkt);

        [DllImport(MacAVCodecLibrary, EntryPoint = nameof(av_packet_unref), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void av_packet_unref_osx(IntPtr pkt);

        [DllImport(MacAVCodecLibrary, EntryPoint = nameof(avcodec_flush_buffers), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void avcodec_flush_buffers_osx(IntPtr avctx);

        [DllImport(MacAVCodecLibrary, EntryPoint = nameof(av_get_exact_bits_per_sample), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int av_get_exact_bits_per_sample_osx(AVCodecID codec_id);

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
        private delegate void avcodec_flush_buffers_delegate(IntPtr avctx);
        private delegate int av_get_exact_bits_per_sample_delegate(AVCodecID codec_id);

        // These fields represent function pointers towards each of the extern functions. They get set
        // to the proper platform-specific functions by the static constructor. For example, if this is
        // running on a Windows machine, each of these pointers will point to the various XXX_windows
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
        private static avcodec_flush_buffers_delegate avcodec_flush_buffers_impl;
        private static av_get_exact_bits_per_sample_delegate av_get_exact_bits_per_sample_impl;

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
                avcodec_flush_buffers_impl = avcodec_flush_buffers_windows;
                av_get_exact_bits_per_sample_impl = av_get_exact_bits_per_sample_windows;
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
                avcodec_flush_buffers_impl = avcodec_flush_buffers_linux;
                av_get_exact_bits_per_sample_impl = av_get_exact_bits_per_sample_linux;
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
                avcodec_flush_buffers_impl = avcodec_flush_buffers_osx;
                av_get_exact_bits_per_sample_impl = av_get_exact_bits_per_sample_osx;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region Public API
        
        /// <summary>
        /// Returns information about the decoder for the given codec type.
        /// </summary>
        /// <param name="id">The codec to get the decoder for</param>
        /// <returns>(<see cref="AVCodec"/>*) The decoder for the codec type</returns>
        public static IntPtr avcodec_find_decoder(AVCodecID id)
        {
            return avcodec_find_decoder_impl(id);
        }
        
        /// <summary>
        /// Sets up an <see cref="AVCodecContext"/> with support for the given <see cref="AVCodec"/>.
        /// </summary>
        /// <param name="avctx">(<see cref="AVCodecContext"/>*) The context to prepare</param>
        /// <param name="codec">(<see cref="AVCodec"/>*) The codec to prepare the context with</param>
        /// <param name="options">(<see cref="AVDictionary"/>*) Options for the assignment, or
        /// <see cref="IntPtr.Zero"/> for the default options.</param>
        /// <returns><see cref="AVERROR.AVERROR_SUCCESS"/> on a success, or an error code on a failure.</returns>
        public static AVERROR avcodec_open2(IntPtr avctx, IntPtr codec, ref IntPtr options)
        {
            return avcodec_open2_impl(avctx, codec, ref options);
        }

        /// <summary>
        /// Sends a packet full of encoded audio data from an audio stream to the decoder for decoding.
        /// </summary>
        /// <param name="avctx">(<see cref="AVCodecContext"/>*) The decoder to decode the packet with</param>
        /// <param name="avpkt">(<see cref="AVPacket"/>*) The input packet with the encoded audio data</param>
        /// <returns><see cref="AVERROR.AVERROR_SUCCESS"/> on a success, or an error code on a failure.</returns>
        public static AVERROR avcodec_send_packet(IntPtr avctx, IntPtr avpkt)
        {
            return avcodec_send_packet_impl(avctx, avpkt);
        }

        /// <summary>
        /// Retrieves raw, decoded audio from the decoder. Call this after <see cref="avcodec_send_packet(IntPtr, IntPtr)"/>
        /// to get the decoded input data.
        /// </summary>
        /// <param name="avctx">(<see cref="AVCodecContext"/>*) The decoder holding the decoded data</param>
        /// <param name="frame">(<see cref="AVFrame"/>*) The output frame to hold the decoded data</param>
        /// <returns><see cref="AVERROR.AVERROR_SUCCESS"/> on a success, or an error code on a failure.</returns>
        public static AVERROR avcodec_receive_frame(IntPtr avctx, IntPtr frame)
        {
            return avcodec_receive_frame_impl(avctx, frame);
        }
        
        /// <summary>
        /// Allocates an <see cref="AVPacket"/> and sets all of its fields to the default values.
        /// This doesn't allocate any data buffers though, those have to be done with
        /// <see cref="av_new_packet(IntPtr, int)"/>.
        /// </summary>
        /// <returns>(<see cref="AVPacket"/>*) The new packet</returns>
        public static IntPtr av_packet_alloc()
        {
            return av_packet_alloc_impl();
        }
        
        /// <summary>
        /// Deletes and frees an <see cref="AVPacket"/>. The pointer to it in <paramref name="pkt"/>
        /// will be set to <see cref="IntPtr.Zero"/>.
        /// </summary>
        /// <param name="pkt">(<see cref="AVPacket"/>*) The packet to free</param>
        public static void av_packet_free(ref IntPtr pkt)
        {
            av_packet_free_impl(ref pkt);
        }

        /// <summary>
        /// Reinitializes an <see cref="AVPacket"/>, setting its fields to the default values
        /// and allocating its data buffer with the given size.
        /// </summary>
        /// <param name="pkt">(<see cref="AVPacket"/>*) The packet to reset</param>
        /// <param name="size">The size of the buffer to allocate, in bytes</param>
        /// <returns><see cref="AVERROR.AVERROR_SUCCESS"/> on a success, or an error code on a failure.</returns>
        public static AVERROR av_new_packet(IntPtr pkt, int size)
        {
            return av_new_packet_func(pkt, size);
        }
        
        /// <summary>
        /// Reinitializes an <see cref="AVPacket"/>, setting its fields to the default values
        /// but leaving the data buffer alone.
        /// </summary>
        /// <param name="pkt">(<see cref="AVPacket"/>*) The packet to reset</param>
        public static void av_init_packet(IntPtr pkt)
        {
            av_init_packet_impl(pkt);
        }
        
        /// <summary>
        /// Wipes an <see cref="AVPacket"/>, resetting the fields to their default values
        /// and decrementing the reference counter on the data buffer.
        /// </summary>
        /// <param name="pkt">(<see cref="AVPacket"/>*) The packet to wipe</param>
        public static void av_packet_unref(IntPtr pkt)
        {
            av_packet_unref_impl(pkt);
        }
        
        /// <summary>
        /// Wipes and resets the internal state of an <see cref="AVCodecContext"/> and clears
        /// out its internal buffers. This should be used after seeking around in a stream,
        /// such as restarting it from the beginning.
        /// </summary>
        /// <param name="avctx">(<see cref="AVCodecContext"/>*) The context to reset</param>
        public static void avcodec_flush_buffers(IntPtr avctx)
        {
            avcodec_flush_buffers_impl(avctx);
        }
        
        /// <summary>
        /// Returns the precise number of bits that an audio sample will contain, based on the
        /// codec it was encoded with. If the number is unknown, this will return zero.
        /// </summary>
        /// <param name="codec_id">The codec that the sample was encoded with</param>
        /// <returns>The number of bits that a frame of audio encoded with the given codec
        /// contains, or zero if the amount isn't known.</returns>
        public static int av_get_exact_bits_per_sample(AVCodecID codec_id)
        {
            return av_get_exact_bits_per_sample_impl(codec_id);
        }

        #endregion
    }
}
