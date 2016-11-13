﻿using DiscordJukebox.Interop;
using System;
using System.Runtime.InteropServices;

namespace DiscordJukebox
{
    internal class AudioFrame : IDisposable
    {
        private IntPtr PacketPtr;

        private IntPtr InputFramePtr;

        private IntPtr OutputFramePtr;

        private IntPtr SwrContextPtr;

        unsafe public AudioFrame(AVSampleFormat SampleFormat, int SamplesPerChannel, AV_CH_LAYOUT ChannelLayout, int SampleRate)
        {
            // Set up the packet
            PacketPtr = AVCodecInterop.av_packet_alloc();
            AVERROR result = AVCodecInterop.av_new_packet(PacketPtr, AVCodecInterop.AVCODEC_MAX_AUDIO_FRAME_SIZE + AVCodecInterop.AV_INPUT_BUFFER_PADDING_SIZE);
            if(result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Packet allocation failed: {result}");
            }

            // Set up the input frame and its buffers
            InputFramePtr = AVUtilInterop.av_frame_alloc();
            AVFrame* inputFrame =(AVFrame*)InputFramePtr.ToPointer();
            inputFrame->format = SampleFormat;
            inputFrame->nb_samples = SamplesPerChannel;
            inputFrame->channel_layout = ChannelLayout;
            inputFrame->sample_rate = SampleRate;
            //result = AVUtilInterop.av_frame_get_buffer(InputFramePtr, 0);
            if(result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Input frame buffer allocation failed: {result}");
            }

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

            // Set up the buffers for the output frame
            long delay = SWResampleInterop.swr_get_delay(SwrContextPtr, 48000);
            int outSamples = (int)delay + (SamplesPerChannel * 48000 / SampleRate) + 3;
            outputFrame->nb_samples = outSamples;
            result = AVUtilInterop.av_frame_get_buffer(OutputFramePtr, 0);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Output frame buffer allocation failed: {result}");
            }
        }

        unsafe public bool ReadFrame(IntPtr FormatContext, IntPtr CodecContext)
        {
            AVFrame* i = (AVFrame*)InputFramePtr.ToPointer();
            AVPacket* p = (AVPacket*)PacketPtr.ToPointer();
            AVFrame* f = (AVFrame*)OutputFramePtr.ToPointer();

            AVERROR result = AVFormatInterop.av_read_frame(FormatContext, PacketPtr);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                if(result == AVERROR.AVERROR_EOF)
                {
                    return true;
                }
                throw new Exception($"Error reading audio packet: {result}");
            }

            result = AVCodecInterop.avcodec_send_packet(CodecContext, PacketPtr);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Error decoding audio packet: {result}");
            }


            result = AVCodecInterop.avcodec_receive_frame(CodecContext, InputFramePtr);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Error receiving decoded audio frame: {result}");
            }

            result = SWResampleInterop.swr_convert_frame(SwrContextPtr, OutputFramePtr, InputFramePtr);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Resampling audio frame failed: {result}");
            }
            while(f->nb_samples > 0)
            {
                result = SWResampleInterop.swr_convert_frame(SwrContextPtr, OutputFramePtr, IntPtr.Zero);
                if (result != AVERROR.AVERROR_SUCCESS)
                {
                    throw new Exception($"Resampling audio frame failed: {result}");
                }
            }

            


            //AVFrame frame = Marshal.PtrToStructure<AVFrame>(OutputFramePtr);
            // This is cheating, but copying the entire AVFrame over is inefficient when we just care
            // about the linesize.
            IntPtr linesizePtr = OutputFramePtr + IntPtr.Size * 8;
            //BufferLength = Marshal.ReadInt32(linesizePtr);

            return false;
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                SWResampleInterop.swr_free(ref SwrContextPtr);
                AVUtilInterop.av_frame_free(ref InputFramePtr);
                AVUtilInterop.av_frame_free(ref OutputFramePtr);
                AVCodecInterop.av_packet_free(ref PacketPtr);

                disposedValue = true;
            }
        }
        
        ~AudioFrame()
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
