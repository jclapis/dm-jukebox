using DMJukebox.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DMJukebox
{
    internal class DiscordPlayer : IDisposable
    {
        private readonly IntPtr OpusEncodeBuffer;

        private const int OpusEncodeBufferSize = 4800 * 2 * sizeof(float);

        private readonly IntPtr OpusEncoder;

        private ushort PacketNumber;

        private uint Ssrc;

        public DiscordPlayer()
        {
            OpusEncodeBuffer = Marshal.AllocHGlobal(OpusEncodeBufferSize);
            OpusErrorCode error;
            OpusEncoder = OpusInterop.opus_encoder_create(OpusSampleRate._48000, OpusChannelCount._2, OPUS_APPLICATION.OPUS_APPLICATION_AUDIO, out error);
            if(error != OpusErrorCode.OPUS_OK)
            {
                throw new Exception($"Failed to create Opus encoder: {error}");
            }
        }

        unsafe public void AddPlaybackData(float[] PlaybackData, int NumberOfSamplesToWrite)
        {
            int result;
            fixed (float* playbackPtr = &PlaybackData[0])
            {
                result = OpusInterop.opus_encode_float(OpusEncoder, (IntPtr)playbackPtr, NumberOfSamplesToWrite, OpusEncodeBuffer, OpusEncodeBufferSize);
            }
            if(result < 0)
            {
                OpusErrorCode error = (OpusErrorCode)result;
                throw new Exception($"Opus failed to encode data: {error}");
            }
        }

        private void CreateRtpHeader()
        {
            byte[] header = new byte[12];
            header[0] = 0x80;
            header[1] = 0x78;
            
            header[2] = (byte)(PacketNumber >> 8);
            header[3] = (byte)PacketNumber;


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

                OpusInterop.opus_encoder_destroy(OpusEncoder);
                Marshal.FreeHGlobal(OpusEncodeBuffer);

                disposedValue = true;
            }
        }
        
        ~DiscordPlayer()
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
