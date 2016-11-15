using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordJukebox
{
    internal class CircularBuffer
    {
        public const int BufferSize = 4800;

        private readonly float[] LeftBuffer;

        private readonly float[] RightBuffer;

        private readonly object Lock;

        private int CurrentWritePosition;

        private int CurrentReadPosition;

        public int ReadWriteDelta { get; private set; }

        private readonly AutoResetEvent ReadNotifier;

        private readonly AutoResetEvent WriteNotifier;

        public CircularBuffer()
        {
            LeftBuffer = new float[BufferSize];
            RightBuffer = new float[BufferSize];
            Lock = new object();
            ReadNotifier = new AutoResetEvent(false);
            WriteNotifier = new AutoResetEvent(false);
        }

        public void Write(float[] LeftChannelData, float[] RightChannelData, int Size)
        {
            bool isNotReady;
            lock (Lock)
            {
                isNotReady = ReadWriteDelta + Size > BufferSize;
            }
            while (isNotReady)
            {
                //System.Diagnostics.Debug.WriteLine("Writing has to pause, too much stuff");
                WriteNotifier.WaitOne();
                lock (Lock)
                {
                    isNotReady = ReadWriteDelta + Size > BufferSize;
                }
            }

            int headroom = BufferSize - CurrentWritePosition;
            if (headroom >= Size)
            {
                Buffer.BlockCopy(LeftChannelData, 0, LeftBuffer, CurrentWritePosition * sizeof(float), Size * sizeof(float));
                Buffer.BlockCopy(RightChannelData, 0, RightBuffer, CurrentWritePosition * sizeof(float), Size * sizeof(float));
                CurrentWritePosition += Size;
            }
            else
            {
                int overflow = Size - headroom;
                Buffer.BlockCopy(LeftChannelData, 0, LeftBuffer, CurrentWritePosition * sizeof(float), headroom * sizeof(float));
                Buffer.BlockCopy(LeftChannelData, headroom * sizeof(float), LeftBuffer, 0, overflow * sizeof(float));
                Buffer.BlockCopy(RightChannelData, 0, RightBuffer, CurrentWritePosition * sizeof(float), headroom * sizeof(float));
                Buffer.BlockCopy(RightChannelData, headroom * sizeof(float), RightBuffer, 0, overflow * sizeof(float));
                CurrentWritePosition = overflow;
            }

            lock(Lock)
            {
                ReadWriteDelta += Size;
            }
            ReadNotifier.Set();
        }

        public void Read(float[] LeftChannel, float[] RightChannel, int Size)
        {
            bool isNotReady;
            lock (Lock)
            {
                isNotReady = Size > ReadWriteDelta;
            }
            while (isNotReady)
            {
                //System.Diagnostics.Debug.WriteLine("Reading has to pause, not enough stuff yet");
                ReadNotifier.WaitOne();
                lock (Lock)
                {
                    isNotReady = Size > ReadWriteDelta;
                }
            }
            
            int headroom = BufferSize - CurrentReadPosition;
            if(headroom >= Size)
            {
                Buffer.BlockCopy(LeftBuffer, CurrentReadPosition * sizeof(float), LeftChannel, 0, Size * sizeof(float));
                Buffer.BlockCopy(RightBuffer, CurrentReadPosition * sizeof(float), RightChannel, 0, Size * sizeof(float));
                CurrentReadPosition += Size;
            }
            else
            {
                int overflow = Size - headroom;
                Buffer.BlockCopy(LeftBuffer, CurrentReadPosition * sizeof(float), LeftChannel, 0, headroom * sizeof(float));
                Buffer.BlockCopy(LeftBuffer, 0, LeftChannel, headroom * sizeof(float), overflow * sizeof(float));
                Buffer.BlockCopy(RightBuffer, CurrentReadPosition * sizeof(float), RightChannel, 0, headroom * sizeof(float));
                Buffer.BlockCopy(RightBuffer, 0, RightChannel, headroom * sizeof(float), overflow * sizeof(float));
                CurrentReadPosition = overflow;
            }

            lock(Lock)
            {
                ReadWriteDelta -= Size;
            }
            WriteNotifier.Set();

        }

    }
}
