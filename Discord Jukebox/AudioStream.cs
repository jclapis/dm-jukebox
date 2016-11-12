using DiscordJukebox.Interop;
using System;
using System.Runtime.InteropServices;

namespace DiscordJukebox
{
    internal class AudioStream : IDisposable
    {
        private readonly IntPtr FormatContextPtr;

        private readonly AVStream Stream;

        private readonly AVCodecContext CodecContext;

        private readonly AudioFrame Frame;

        public float Volume { get; set; }

        public bool Loop { get; set; }

        public string CodecName { get; }

        public int NumberOfChannels { get; }

        public long Bitrate { get; }

        public int SamplesPerFrame { get; }

        public TimeSpan Duration { get; }

        public AudioStream(string FilePath)
        {
            // Create the FormatContext
            FormatContextPtr = AVFormatInterop.avformat_alloc_context();
            IntPtr options = IntPtr.Zero;
            AVERROR result = AVFormatInterop.avformat_open_input(ref FormatContextPtr, FilePath, IntPtr.Zero, ref options);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Opening the file failed with code {result}");
            }

            // Read the info for the streams in the file
            options = IntPtr.Zero;
            result = AVFormatInterop.avformat_find_stream_info(FormatContextPtr, ref options);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Reading the file's stream info failed with code {result}");
            }

            // Find the first audio stream
            AVFormatContext formatContext = Marshal.PtrToStructure<AVFormatContext>(FormatContextPtr);
            bool foundStream = false;
            for (int i = 0; i < formatContext.nb_streams; i++)
            {
                IntPtr streamPtr = Marshal.ReadIntPtr(formatContext.streams, IntPtr.Size * i);
                Stream = Marshal.PtrToStructure<AVStream>(streamPtr);
                CodecContext = Marshal.PtrToStructure<AVCodecContext>(Stream.codec);
                if (CodecContext.codec_type == AVMediaType.AVMEDIA_TYPE_AUDIO)
                {
                    foundStream = true;
                    break;
                }
            }
            if(!foundStream)
            {
                throw new Exception("No audio streams detected in the file.");
            }

            // Prepare the decoder
            IntPtr codecPtr = AVCodecInterop.avcodec_find_decoder(CodecContext.codec_id);
            if (codecPtr == IntPtr.Zero)
            {
                throw new Exception($"Error loading audio codec: finding the decoder for codec ID {CodecContext.codec_id} failed.");
            }
            AVCodec codec = Marshal.PtrToStructure<AVCodec>(codecPtr);
            options = IntPtr.Zero;
            result = AVCodecInterop.avcodec_open2(Stream.codec, codecPtr, ref options);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Error loading audio codec: opening codec failed with {result}.");
            }

            // Create the audio frame for getting decoded data
            Frame = new AudioFrame(CodecContext.sample_fmt, CodecContext.frame_size, CodecContext.channel_layout);

            CodecName = codec.long_name;
            NumberOfChannels = CodecContext.channels;
            Bitrate = CodecContext.bit_rate;
            SamplesPerFrame = CodecContext.frame_size;

            double timeBaseInSeconds = Stream.time_base.num / (double)Stream.time_base.den;
            double durationInSeconds = Stream.duration * timeBaseInSeconds;
            Duration = TimeSpan.FromSeconds(durationInSeconds);
        }

        public AudioFrame GetNextFrame()
        {
            bool endOfFile = Frame.ReadFrame(FormatContextPtr, Stream.codec);
            if(endOfFile)
            {
                return null;
            }
            return Frame;
        }

        private unsafe void SetVolume()
        {

        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Frame.Dispose();
                }

                AVFormatInterop.avformat_free_context(FormatContextPtr);

                disposedValue = true;
            }
        }

        ~AudioStream()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
