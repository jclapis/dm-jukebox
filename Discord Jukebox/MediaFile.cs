using DiscordJukebox.Interop;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DiscordJukebox
{
    internal class MediaFile : IDisposable
    {
        public string FilePath { get; }

        private IntPtr FormatContextPtr;

        private IntPtr PacketBufferPtr;

        private IntPtr PacketPtr;

        private IntPtr FramePtr;

        public AudioStream AudioStream { get; }

        public MediaFile(string FilePath)
        {
            this.FilePath = FilePath;
            FormatContextPtr = AVFormatInterface.avformat_alloc_context();
            IntPtr options = IntPtr.Zero;
            int result = AVFormatInterface.avformat_open_input(ref FormatContextPtr, FilePath, IntPtr.Zero, ref options);
            if (result != 0)
            {
                throw new Exception($"Opening the file failed with code {result}");
            }

            options = IntPtr.Zero;
            result = AVFormatInterface.avformat_find_stream_info(FormatContextPtr, ref options);
            if (result != 0)
            {
                throw new Exception($"Reading the file's stream info failed with code {result}");
            }

            AVFormatContext format = (AVFormatContext)Marshal.PtrToStructure(FormatContextPtr, typeof(AVFormatContext));
            AudioStream = FindAudioStream(format);

            AVPacket packet = new AVPacket();
            int bufferSize = AVCodecInterface.AVCODEC_MAX_AUDIO_FRAME_SIZE + AVCodecInterface.AV_INPUT_BUFFER_PADDING_SIZE;
            PacketBufferPtr = Marshal.AllocHGlobal(bufferSize);
            packet.data = PacketBufferPtr;
            packet.size = bufferSize;
            PacketPtr = Marshal.AllocHGlobal(Marshal.SizeOf<AVPacket>());
            Marshal.StructureToPtr(packet, PacketPtr, true);
            FramePtr = AVUtilInterface.av_frame_alloc();
        }

        public AVFrame GetNextFrame()
        {
            int result = AVFormatInterface.av_read_frame(FormatContextPtr, PacketPtr);
            if (result != 0)
            {
                throw new Exception($"Reading the next frame from {Path.GetFileName(FilePath)} failed with code {result}");
            }
            return AudioStream.GetNextFrame(PacketPtr, FramePtr);
        }

        private static AudioStream FindAudioStream(AVFormatContext FormatContext)
        {
            for (int i = 0; i < FormatContext.nb_streams; i++)
            {
                IntPtr streamPtr = Marshal.ReadIntPtr(FormatContext.streams + i);
                AVStream stream = Marshal.PtrToStructure<AVStream>(streamPtr);
                AVCodecContext codecContext = Marshal.PtrToStructure<AVCodecContext>(stream.codec);
                if (codecContext.codec_type == AVMediaType.AVMEDIA_TYPE_AUDIO)
                {
                    return new AudioStream(streamPtr, stream, codecContext);
                }
            }

            return null;
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
                
                Marshal.FreeHGlobal(PacketBufferPtr);
                Marshal.FreeHGlobal(PacketPtr);
                AVUtilInterface.av_frame_free(ref FramePtr);
                AVFormatInterface.avformat_free_context(FormatContextPtr);

                disposedValue = true;
            }
        }
        
        ~MediaFile()
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
