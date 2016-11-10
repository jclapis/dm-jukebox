using DiscordJukebox.Interop;
using System;
using System.Runtime.InteropServices;

namespace DiscordJukebox
{
    internal class AudioStream
    {
        private IntPtr StreamPtr;

        private AVStream Stream;

        private AVCodecContext CodecContext;

        private AVCodec Codec;

        public string CodecName
        {
            get
            {
                return Codec.long_name;
            }
        }

        public int NumberOfChannels
        {
            get
            {
                return CodecContext.channels;
            }
        }

        public long Bitrate
        {
            get
            {
                return CodecContext.bit_rate;
            }
        }

        public int SamplesPerFrame
        {
            get
            {
                return CodecContext.frame_size;
            }
        }

        public long Duration
        {
            get
            {
                return Stream.duration;
            }
        }

        public AudioStream(IntPtr StreamPtr, AVStream Stream, AVCodecContext CodecContext)
        {
            this.StreamPtr = StreamPtr;
            this.Stream = Stream;
            this.CodecContext = CodecContext;
            
            IntPtr codecPtr = AVCodecInterface.avcodec_find_decoder(CodecContext.codec_id);
            if (codecPtr == IntPtr.Zero)
            {
                throw new Exception($"Error loading audio codec: finding the decoder for codec ID {CodecContext.codec_id} failed.");
            }
            Codec = Marshal.PtrToStructure<AVCodec>(codecPtr);
            IntPtr options = IntPtr.Zero;
            int openResult = AVCodecInterface.avcodec_open2(Stream.codec, codecPtr, ref options);
            if (openResult != 0)
            {
                throw new Exception($"Error loading audio codec: opening codec failed with {openResult}.");
            }
        }

    }
}
