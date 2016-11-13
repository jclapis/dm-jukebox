using DiscordJukebox.Interop;
using System;
using System.Runtime.InteropServices;

namespace DiscordJukebox
{
    internal class AudioFrame // : IDisposable
    {
        public float[] LeftChannel { get; }

        public float[] RightChannel { get; }

        public AudioFrame(float[] LeftChannel, float[] RightChannel)
        {
            this.LeftChannel = LeftChannel;
            this.RightChannel = RightChannel;
        }






























        /* private IntPtr PacketPtr;

        private IntPtr InputFramePtr;

        private IntPtr OutputFramePtr;

        private IntPtr SwrContextPtr;

        public float[] LeftChannelBuffer { get; }

        public float[] RightChannelBuffer { get; }

        public int NumberOfElements { get; private set; }

        unsafe public AudioFrame(AVSampleFormat SampleFormat, int SamplesPerChannel, AV_CH_LAYOUT ChannelLayout, int SampleRate)
        {
            // Set up the packet - no need for doing the buffers, because they get reset
            // on each new av_read_frame() call
            AVERROR result;
            PacketPtr = AVCodecInterop.av_packet_alloc();

            // Set up the input frame (not the buffers, since they get reset every time we
            // read a new frame from the input file)
            InputFramePtr = AVUtilInterop.av_frame_alloc();
            AVFrame* inputFrame =(AVFrame*)InputFramePtr.ToPointer();
            inputFrame->format = SampleFormat;
            inputFrame->nb_samples = SamplesPerChannel;
            inputFrame->channel_layout = ChannelLayout;
            inputFrame->sample_rate = SampleRate;

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
            int outSamples = (int)delay + (SamplesPerChannel * 48000 / SampleRate) + 3;
            outputFrame->nb_samples = outSamples;
            result = AVUtilInterop.av_frame_get_buffer(OutputFramePtr, 0);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Output frame buffer allocation failed: {result}");
            }

            // Set up the output capture buffers for playback
            LeftChannelBuffer = new float[outputFrame->linesize[0]];
            RightChannelBuffer = new float[outputFrame->linesize[0]];
        }

        unsafe public bool ReadFrame(IntPtr FormatContext, IntPtr CodecContext)
        {
            AVFrame* i = (AVFrame*)InputFramePtr.ToPointer();
            AVPacket* p = (AVPacket*)PacketPtr.ToPointer();
            AVFrame* f = (AVFrame*)OutputFramePtr.ToPointer();

            // Read the next packet from the file
            AVERROR result = AVFormatInterop.av_read_frame(FormatContext, PacketPtr);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                if(result == AVERROR.AVERROR_EOF)
                {
                    return true;
                }
                throw new Exception($"Error reading audio packet: {result}");
            }

            // Send the packet over to the decoder
            result = AVCodecInterop.avcodec_send_packet(CodecContext, PacketPtr);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Error decoding audio packet: {result}");
            }

            // Get the decoded raw data from the decoder
            result = AVCodecInterop.avcodec_receive_frame(CodecContext, InputFramePtr);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Error receiving decoded audio frame: {result}");
            }

            // Resample it to the Discord requirements (48 kHz, 2 channel stereo)
            result = SWResampleInterop.swr_convert_frame(SwrContextPtr, OutputFramePtr, InputFramePtr);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Resampling audio frame failed: {result}");
            }

            // Copy the data into the managed buffers
            NumberOfElements = 0;
            Marshal.Copy((IntPtr)f->data0, LeftChannelBuffer, NumberOfElements, f->nb_samples);
            Marshal.Copy((IntPtr)f->data1, RightChannelBuffer, NumberOfElements, f->nb_samples);
            NumberOfElements += f->nb_samples;

            // Get the next round of data, if there is one
            result = SWResampleInterop.swr_convert_frame(SwrContextPtr, OutputFramePtr, IntPtr.Zero);
            if (result != AVERROR.AVERROR_SUCCESS)
            {
                throw new Exception($"Resampling audio frame failed: {result}");
            }
            while (f->nb_samples > 0)
            {
                // Copy the new data into the buffer as well
                Marshal.Copy((IntPtr)f->data0, LeftChannelBuffer, NumberOfElements, f->nb_samples);
                Marshal.Copy((IntPtr)f->data1, RightChannelBuffer, NumberOfElements, f->nb_samples);
                NumberOfElements += f->nb_samples;

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
    }*/
    }
}
