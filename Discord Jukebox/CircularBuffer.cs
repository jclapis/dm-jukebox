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
        private readonly float[] InternalBuffer;

        private readonly object Lock;

        private int CurrentWritePosition;

        private int CurrentReadPosition;

        public int ReadWriteDelta { get; private set; }

        private readonly AutoResetEvent ReadNotifier;

        private readonly AutoResetEvent WriteNotifier;

        public CircularBuffer()
        {
            InternalBuffer = new float[9600];
            Lock = new object();
            ReadNotifier = new AutoResetEvent(false);
            WriteNotifier = new AutoResetEvent(false);
        }

        public bool Write(float[] Data, int Size)
        {
            bool isNotReady;
            lock (Lock)
            {
                isNotReady = ReadWriteDelta + Size > InternalBuffer.Length;
            }
            while (isNotReady)
            {
                System.Diagnostics.Debug.WriteLine("Writing has to pause, too much stuff");
                WriteNotifier.WaitOne();
                lock (Lock)
                {
                    isNotReady = ReadWriteDelta + Size > InternalBuffer.Length;
                }
            }

            int headroom = InternalBuffer.Length - CurrentWritePosition;
            if (headroom >= Size)
            {
                Buffer.BlockCopy(Data, 0, InternalBuffer, CurrentWritePosition * sizeof(float), Size * sizeof(float));
                CurrentWritePosition += Size;
            }
            else
            {
                int overflow = Size - headroom;
                Buffer.BlockCopy(Data, 0, InternalBuffer, CurrentWritePosition * sizeof(float), headroom * sizeof(float));
                Buffer.BlockCopy(Data, headroom * sizeof(float), InternalBuffer, 0, overflow * sizeof(float));
                CurrentWritePosition = overflow;
            }

            lock(Lock)
            {
                ReadWriteDelta += Size;
            }
            ReadNotifier.Set();
            return true;
        }

        public float[] Read(int Size)
        {
            bool isNotReady;
            lock (Lock)
            {
                isNotReady = Size > ReadWriteDelta;
            }
            while (isNotReady)
            {
                System.Diagnostics.Debug.WriteLine("Reading has to pause, not enough stuff yet");
                ReadNotifier.WaitOne();
                lock (Lock)
                {
                    isNotReady = Size > ReadWriteDelta;
                }
            }

            float[] outBuffer = new float[Size];
            int headroom = InternalBuffer.Length - CurrentReadPosition;
            if(headroom >= Size)
            {
                Buffer.BlockCopy(InternalBuffer, CurrentReadPosition * sizeof(float), outBuffer, 0, Size * sizeof(float));
                CurrentReadPosition += Size;
            }
            else
            {
                int overflow = Size - headroom;
                Buffer.BlockCopy(InternalBuffer, CurrentReadPosition * sizeof(float), outBuffer, 0, headroom * sizeof(float));
                Buffer.BlockCopy(InternalBuffer, 0, outBuffer, headroom * sizeof(float), overflow * sizeof(float));
                CurrentReadPosition = overflow;
            }

            lock(Lock)
            {
                ReadWriteDelta -= Size;
            }
            WriteNotifier.Set();
            return outBuffer;

        }

    }
}
