using DiscordJukebox.Interop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace DiscordJukebox
{
    internal class AudioFrame : IDisposable
    {
        private readonly IntPtr PacketBufferPtr;

        private readonly IntPtr FrameExtendedData;

        private readonly IntPtr PacketPtr;

        private readonly IntPtr FramePtr;

        public ReadOnlyCollection<IntPtr> FrameBuffers { get; }

        public int NumberOfChannels { get; }

        public int SamplesPerFrame { get; }

        public int ChannelBufferSize { get; }

        public AVSampleFormat SampleFormat { get; }

        public int BufferLength { get; private set; }

        public AudioFrame(int NumberOfChannels, int SamplesPerFrame, AVSampleFormat SampleFormat)
        {
            // Initialization stuff
            List<IntPtr> frameBuffers = new List<IntPtr>();
            this.NumberOfChannels = NumberOfChannels;
            this.SamplesPerFrame = SamplesPerFrame;
            this.SampleFormat = SampleFormat;
            ChannelBufferSize = GetChannelBufferSize(SamplesPerFrame, SampleFormat);

            // Set up the packet
            AVPacket packet = new AVPacket();
            int bufferSize = AVCodecInterop.AVCODEC_MAX_AUDIO_FRAME_SIZE + AVCodecInterop.AV_INPUT_BUFFER_PADDING_SIZE;
            PacketBufferPtr = Marshal.AllocHGlobal(bufferSize);
            packet.data = PacketBufferPtr;
            packet.size = bufferSize;
            PacketPtr = Marshal.AllocHGlobal(Marshal.SizeOf<AVPacket>());
            Marshal.StructureToPtr(packet, PacketPtr, true);

            // Set up the input frame and its buffers
            FramePtr = AVUtilInterop.av_frame_alloc();
            AVFrame frame = Marshal.PtrToStructure<AVFrame>(FramePtr);
            FrameExtendedData = Marshal.AllocHGlobal(IntPtr.Size * NumberOfChannels);
            for(int i = 0; i < NumberOfChannels; i++)
            {
                IntPtr frameBuffer = Marshal.AllocHGlobal(ChannelBufferSize);
                frameBuffers.Add(frameBuffer);
                Marshal.WriteIntPtr(FrameExtendedData, IntPtr.Size * i, frameBuffer);
            }
            Marshal.StructureToPtr(frame, FramePtr, false);
            FrameBuffers = new ReadOnlyCollection<IntPtr>(frameBuffers);

            // Set up the output frame and its buffers
            AVFrame targetFrame = new AVFrame();
            targetFrame.channels = 2;
            targetFrame.channel_layout = AV_CH_LAYOUT.AV_CH_LAYOUT_STEREO;
            targetFrame.sample_rate = 48000;
            targetFrame.format = AVSampleFormat.AV_SAMPLE_FMT_FLTP;
        }

        public void ReadFrame(IntPtr CodecContext)
        {
            int result = AVCodecInterop.avcodec_send_packet(CodecContext, PacketPtr);
            if (result != 0)
            {
                throw new Exception($"Error reading audio packet: {result}");
            }

            result = AVCodecInterop.avcodec_receive_frame(CodecContext, FramePtr);
            if (result != 0)
            {
                throw new Exception($"Error receiving decoded audio frame: {result}");
            }

            // This is cheating, but copying the entire AVFrame over is inefficient when we just care
            // about the linesize.
            IntPtr linesizePtr = FramePtr + IntPtr.Size * 8;
            BufferLength = Marshal.ReadInt32(linesizePtr);
        }

        private static int GetChannelBufferSize(int SamplesPerFrame, AVSampleFormat SampleFormat)
        {
            switch(SampleFormat)
            {
                case AVSampleFormat.AV_SAMPLE_FMT_U8P:
                    return SamplesPerFrame;

                case AVSampleFormat.AV_SAMPLE_FMT_S16P:
                    return SamplesPerFrame * sizeof(short);

                case AVSampleFormat.AV_SAMPLE_FMT_S32P:
                    return SamplesPerFrame * sizeof(int);

                case AVSampleFormat.AV_SAMPLE_FMT_FLTP:
                    return SamplesPerFrame * sizeof(float);

                case AVSampleFormat.AV_SAMPLE_FMT_DBLP:
                    return SamplesPerFrame * sizeof(double);

                case AVSampleFormat.AV_SAMPLE_FMT_U8:
                case AVSampleFormat.AV_SAMPLE_FMT_S16:
                case AVSampleFormat.AV_SAMPLE_FMT_S32:
                case AVSampleFormat.AV_SAMPLE_FMT_FLT:
                case AVSampleFormat.AV_SAMPLE_FMT_DBL:
                    throw new Exception("Packed audio isn't supported yet");

                case AVSampleFormat.AV_SAMPLE_FMT_NONE:
                case AVSampleFormat.AV_SAMPLE_FMT_NB:
                    throw new Exception($"Unexpected sample format {SampleFormat}");

                default:
                    throw new Exception($"Sample format {SampleFormat} was unhandled.");
            }
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

                IntPtr framePtr = FramePtr;
                AVUtilInterop.av_frame_free(ref framePtr);
                Marshal.FreeHGlobal(FrameExtendedData);
                foreach(IntPtr buffer in FrameBuffers)
                {
                    Marshal.FreeHGlobal(buffer);
                }
                Marshal.FreeHGlobal(PacketPtr);
                Marshal.FreeHGlobal(PacketBufferPtr);

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~AudioFrame()
        {
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
