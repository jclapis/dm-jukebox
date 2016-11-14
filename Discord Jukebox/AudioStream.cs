﻿using DiscordJukebox.Interop;
using System;
using System.Runtime.InteropServices;

namespace DiscordJukebox
{
    internal class AudioStream : IDisposable
    {
        private readonly IntPtr FormatContextPtr;

        private IntPtr PacketPtr;

        private IntPtr InputFramePtr;

        private IntPtr OutputFramePtr;

        private IntPtr SwrContextPtr;

        private readonly AVStream Stream;

        private readonly AVCodecContext CodecContext;

        private readonly float[] LeftChannelBuffer;

        private readonly float[] RightChannelBuffer;

        public float Volume { get; set; }

        public bool Loop { get; set; }

        public string CodecName { get; }

        public int NumberOfChannels { get; }

        public long Bitrate { get; }

        public int SamplesPerFrame { get; }

        public TimeSpan Duration { get; }

        unsafe public AudioStream(string FilePath)
        {
            Volume = 1;

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
            
            // Set up the packet - no need for doing the buffers, because they get reset
            // on each new av_read_frame() call
            PacketPtr = AVCodecInterop.av_packet_alloc();

            // Set up the input frame (not the buffers, since they get reset every time we
            // read a new frame from the input file)
            InputFramePtr = AVUtilInterop.av_frame_alloc();
            AVFrame* inputFrame = (AVFrame*)InputFramePtr.ToPointer();
            inputFrame->format = CodecContext.sample_fmt;
            inputFrame->nb_samples = CodecContext.frame_size;
            inputFrame->channel_layout = CodecContext.channel_layout;
            inputFrame->sample_rate = CodecContext.sample_rate;

            // Set up the output frame
            OutputFramePtr = AVUtilInterop.av_frame_alloc();
            AVFrame* outputFrame = (AVFrame*)OutputFramePtr.ToPointer();
            outputFrame->channel_layout = AV_CH_LAYOUT.AV_CH_LAYOUT_STEREO;
            outputFrame->sample_rate = 48000;
            outputFrame->format = AVSampleFormat.AV_SAMPLE_FMT_FLTP;

            // Set up the swresample context
            SwrContextPtr = SWResampleInterop.swr_alloc();
            result = SWResampleInterop.swr_config_frame(SwrContextPtr, OutputFramePtr, InputFramePtr);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Resampling context configuration failed: {result}");
            }
            result = SWResampleInterop.swr_init(SwrContextPtr);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Resampling context initialization failed: {result}");
            }

            // Set up the buffers for the output frame, which are persistent
            long delay = SWResampleInterop.swr_get_delay(SwrContextPtr, 48000);
            int outSamples = (int)delay + (CodecContext.frame_size * 48000 / CodecContext.sample_rate) + 3;
            outputFrame->nb_samples = outSamples;
            result = AVUtilInterop.av_frame_get_buffer(OutputFramePtr, 0);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Output frame buffer allocation failed: {result}");
            }

            // Set up the output capture buffers for playback
            LeftChannelBuffer = new float[outputFrame->linesize[0]];
            RightChannelBuffer = new float[outputFrame->linesize[0]];
            
            CodecName = codec.long_name;
            NumberOfChannels = CodecContext.channels;
            Bitrate = CodecContext.bit_rate;
            SamplesPerFrame = CodecContext.frame_size;

            double timeBaseInSeconds = Stream.time_base.num / (double)Stream.time_base.den;
            double durationInSeconds = Stream.duration * timeBaseInSeconds;
            Duration = TimeSpan.FromSeconds(durationInSeconds);
        }

        unsafe public AudioFrame GetNextFrame()
        {
            AVFrame* f = (AVFrame*)OutputFramePtr.ToPointer();
            float* leftChannel = (float*)f->data0;
            float* rightChannel = (float*)f->data1;

            // Read the next packet from the file
            AVERROR result = AVFormatInterop.av_read_frame(FormatContextPtr, PacketPtr);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                if (result == AVERROR.AVERROR_EOF)
                {
                    return null;
                }
                throw new Exception($"Error reading audio packet: {result}");
            }

            // Send the packet over to the decoder
            result = AVCodecInterop.avcodec_send_packet(Stream.codec, PacketPtr);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Error decoding audio packet: {result}");
            }

            // Get the decoded raw data from the decoder
            result = AVCodecInterop.avcodec_receive_frame(Stream.codec, InputFramePtr);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Error receiving decoded audio frame: {result}");
            }

            // Resample it to the Discord requirements (48 kHz, 2 channel stereo)
            int currentIndex = 0;
            result = SWResampleInterop.swr_convert_frame(SwrContextPtr, OutputFramePtr, InputFramePtr);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Resampling audio frame failed: {result}");
            }
            while (f->nb_samples > 0)
            {
                // Copy the new data into the managed buffers
                for(int i = 0; i < f->nb_samples; i++)
                {
                    LeftChannelBuffer[currentIndex + i] = leftChannel[i] * Volume;
                    RightChannelBuffer[currentIndex + i] = rightChannel[i] * Volume;
                }
                //Marshal.Copy((IntPtr)f->data0, LeftChannelBuffer, currentIndex, f->nb_samples);
                //Marshal.Copy((IntPtr)f->data1, RightChannelBuffer, currentIndex, f->nb_samples);
                currentIndex += f->nb_samples;

                // Keep the cycle going until we've exhausted the swrcontext buffer
                result = SWResampleInterop.swr_convert_frame(SwrContextPtr, OutputFramePtr, IntPtr.Zero);
                if (result != AVERROR.AVERROR_SUCCESS)
                {
                    throw new Exception($"Resampling audio frame failed: {result}");
                }
            }

            // Clean up the packet and input frame to make sure their buffers are released,
            // since ffmpeg insists on reallocating them each time. Note that the output frame
            // doesn't need to be unref'd, because it's only used by swresample which doesn't
            // allocate a new buffer for the frame each time swr_convert_frame is called.
            AVCodecInterop.av_packet_unref(PacketPtr);
            AVUtilInterop.av_frame_unref(InputFramePtr);

            float[] leftChannelData = new float[currentIndex];
            float[] rightChannelData = new float[currentIndex];
            Buffer.BlockCopy(LeftChannelBuffer, 0, leftChannelData, 0, leftChannelData.Length * sizeof(float));
            Buffer.BlockCopy(RightChannelBuffer, 0, rightChannelData, 0, rightChannelData.Length * sizeof(float));

            AudioFrame frame = new AudioFrame(leftChannelData, rightChannelData);
            return frame;
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                SWResampleInterop.swr_free(ref SwrContextPtr);
                AVUtilInterop.av_frame_free(ref InputFramePtr);
                AVUtilInterop.av_frame_free(ref OutputFramePtr);
                AVCodecInterop.av_packet_free(ref PacketPtr);
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
