using System;
using System.Runtime.InteropServices;

namespace DiscordJukebox
{
    internal class CircularBuffer
    {
        private readonly float[] LeftChannelBuffer;

        private readonly float[] RightChannelBuffer;

        private int CurrentWritePosition;

        private int CurrentReadPosition;

        private readonly int BufferSize;

        public int AvailableData { get; private set; }

        public CircularBuffer(int BufferSize)
        {
            this.BufferSize = BufferSize;
            LeftChannelBuffer = new float[BufferSize];
            RightChannelBuffer = new float[BufferSize];
        }

        public void Write(IntPtr LeftChannelData, IntPtr RightChannelData, int NumberOfSamples)
        {
            if(AvailableData + NumberOfSamples > BufferSize)
            {
                throw new Exception("Circular buffer overflow, this should never happen but it did. Disaster.");
            }

            int headroom = BufferSize - CurrentWritePosition;
            if (headroom >= NumberOfSamples)
            {
                Marshal.Copy(LeftChannelData, LeftChannelBuffer, CurrentWritePosition, NumberOfSamples);
                Marshal.Copy(RightChannelData, RightChannelBuffer, CurrentWritePosition, NumberOfSamples);
                CurrentWritePosition += NumberOfSamples;
            }
            else
            {
                int overflow = NumberOfSamples - headroom;
                Marshal.Copy(LeftChannelData, LeftChannelBuffer, CurrentWritePosition, headroom);
                Marshal.Copy(LeftChannelData + headroom * sizeof(float), LeftChannelBuffer, 0, overflow);
                Marshal.Copy(RightChannelData, RightChannelBuffer, CurrentWritePosition, headroom);
                Marshal.Copy(RightChannelData + headroom * sizeof(float), RightChannelBuffer, 0, overflow);
                CurrentWritePosition = overflow;
            }

            AvailableData += NumberOfSamples;
        }

        public void Read(float[] LeftChannel, float[] RightChannel, int Size)
        {
            if(Size > AvailableData)
            {
                throw new Exception("Circular buffer underflow, this should never happen but it did. Disaster.");
            }
            
            int headroom = BufferSize - CurrentReadPosition;
            if (headroom >= Size)
            {
                Buffer.BlockCopy(LeftChannelBuffer, CurrentReadPosition * sizeof(float), LeftChannel, 0, Size * sizeof(float));
                Buffer.BlockCopy(RightChannelBuffer, CurrentReadPosition * sizeof(float), RightChannel, 0, Size * sizeof(float));
                CurrentReadPosition += Size;
            }
            else
            {
                int overflow = Size - headroom;
                Buffer.BlockCopy(LeftChannelBuffer, CurrentReadPosition * sizeof(float), LeftChannel, 0, headroom * sizeof(float));
                Buffer.BlockCopy(LeftChannelBuffer, 0, LeftChannel, headroom * sizeof(float), overflow * sizeof(float));
                Buffer.BlockCopy(RightChannelBuffer, CurrentReadPosition * sizeof(float), RightChannel, 0, headroom * sizeof(float));
                Buffer.BlockCopy(RightChannelBuffer, 0, RightChannel, headroom * sizeof(float), overflow * sizeof(float));
                CurrentReadPosition = overflow;
            }

            AvailableData -= Size;
        }

    }
}
