/*
 * Copyright (c) 2016 Joe Clapis.
 */

using System;
using System.Threading;

namespace DMJukebox
{
    internal class ThreadSafeCircularBuffer
    {
        public const int BufferSize = 4800;

        private readonly float[] LeftChannelBuffer;

        private readonly float[] RightChannelBuffer;

        private readonly object Lock;

        private int CurrentWritePosition;

        private int CurrentReadPosition;

        public int AvailableData { get; private set; }

        private readonly AutoResetEvent ReadNotifier;

        private readonly AutoResetEvent WriteNotifier;

        public ThreadSafeCircularBuffer()
        {
            LeftChannelBuffer = new float[BufferSize];
            RightChannelBuffer = new float[BufferSize];
            Lock = new object();
            ReadNotifier = new AutoResetEvent(false);
            WriteNotifier = new AutoResetEvent(false);
        }

        public void Write(float[] LeftChannelData, float[] RightChannelData, int Size)
        {
            bool isNotReady;
            lock (Lock)
            {
                isNotReady = AvailableData + Size > BufferSize;
            }
            while (isNotReady)
            {
                WriteNotifier.WaitOne();
                lock (Lock)
                {
                    isNotReady = AvailableData + Size > BufferSize;
                }
            }

            int headroom = BufferSize - CurrentWritePosition;
            if (headroom >= Size)
            {
                Buffer.BlockCopy(LeftChannelData, 0, LeftChannelBuffer, CurrentWritePosition * sizeof(float), Size * sizeof(float));
                Buffer.BlockCopy(RightChannelData, 0, RightChannelBuffer, CurrentWritePosition * sizeof(float), Size * sizeof(float));
                CurrentWritePosition += Size;
            }
            else
            {
                int overflow = Size - headroom;
                Buffer.BlockCopy(LeftChannelData, 0, LeftChannelBuffer, CurrentWritePosition * sizeof(float), headroom * sizeof(float));
                Buffer.BlockCopy(LeftChannelData, headroom * sizeof(float), LeftChannelBuffer, 0, overflow * sizeof(float));
                Buffer.BlockCopy(RightChannelData, 0, RightChannelBuffer, CurrentWritePosition * sizeof(float), headroom * sizeof(float));
                Buffer.BlockCopy(RightChannelData, headroom * sizeof(float), RightChannelBuffer, 0, overflow * sizeof(float));
                CurrentWritePosition = overflow;
            }

            lock(Lock)
            {
                AvailableData += Size;
            }
            ReadNotifier.Set();
        }

        public void Read(float[] LeftChannel, float[] RightChannel, int Size)
        {
            bool isNotReady;
            lock (Lock)
            {
                isNotReady = Size > AvailableData;
            }
            while (isNotReady)
            {
                ReadNotifier.WaitOne();
                lock (Lock)
                {
                    isNotReady = Size > AvailableData;
                }
            }
            
            int headroom = BufferSize - CurrentReadPosition;
            if(headroom >= Size)
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

            lock(Lock)
            {
                AvailableData -= Size;
            }
            WriteNotifier.Set();

        }

        public void Reset()
        {
            Array.Clear(LeftChannelBuffer, 0, BufferSize);
            Array.Clear(RightChannelBuffer, 0, BufferSize);
            CurrentReadPosition = 0;
            CurrentWritePosition = 0;
            AvailableData = 0;
        }

    }
}
