/*
 * Copyright (c) 2016 Joe Clapis.
 */

using System;
using System.Threading;

namespace DMJukebox
{
    /// <summary>
    /// LocalPlaybackBuffer is a thread-safe circular buffer that takes incoming aggregated playback data, buffers it, and
    /// writes it to 
    /// </summary>
    internal class LocalPlaybackBuffer
    {
        public const int BufferSize = 4800;

        private readonly float[] InternalLeftChannelBuffer;

        private readonly float[] InternalRightChannelBuffer;

        private readonly object Lock;

        private int CurrentWritePosition;

        private int CurrentReadPosition;

        private readonly AutoResetEvent ReadNotifier;

        private readonly AutoResetEvent WriteNotifier;

        public int AvailableData { get; private set; }

        public LocalPlaybackBuffer()
        {
            InternalLeftChannelBuffer = new float[BufferSize];
            InternalRightChannelBuffer = new float[BufferSize];
            Lock = new object();
            ReadNotifier = new AutoResetEvent(false);
            WriteNotifier = new AutoResetEvent(false);
        }

        public void AddPlaybackData(float[] LeftChannelPlaybackData, float[] RightChannelPlaybackData, int Size)
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
                Buffer.BlockCopy(LeftChannelPlaybackData, 0, InternalLeftChannelBuffer, CurrentWritePosition * sizeof(float), Size * sizeof(float));
                Buffer.BlockCopy(RightChannelPlaybackData, 0, InternalRightChannelBuffer, CurrentWritePosition * sizeof(float), Size * sizeof(float));
                CurrentWritePosition += Size;
            }
            else
            {
                int overflow = Size - headroom;
                Buffer.BlockCopy(LeftChannelPlaybackData, 0, InternalLeftChannelBuffer, CurrentWritePosition * sizeof(float), headroom * sizeof(float));
                Buffer.BlockCopy(LeftChannelPlaybackData, headroom * sizeof(float), InternalLeftChannelBuffer, 0, overflow * sizeof(float));
                Buffer.BlockCopy(RightChannelPlaybackData, 0, InternalRightChannelBuffer, CurrentWritePosition * sizeof(float), headroom * sizeof(float));
                Buffer.BlockCopy(RightChannelPlaybackData, headroom * sizeof(float), InternalRightChannelBuffer, 0, overflow * sizeof(float));
                CurrentWritePosition = overflow;
            }

            lock(Lock)
            {
                AvailableData += Size;
            }
            ReadNotifier.Set();
        }

        unsafe public void WritePlaybackDataToSoundAreas(float* LeftChannelArea, float* RightChannelArea, int NumberOfSamplesToWrite, int StepSize)
        {
            bool isNotReady;
            lock (Lock)
            {
                isNotReady = NumberOfSamplesToWrite > AvailableData;
            }
            while (isNotReady)
            {
                ReadNotifier.WaitOne();
                lock (Lock)
                {
                    isNotReady = NumberOfSamplesToWrite > AvailableData;
                }
            }
            
            for(int i = 0; i < NumberOfSamplesToWrite; i++)
            {
                // Because we have to space things out according to the step size, we can't just do a straight copy.
                int areaIndex = StepSize * i;
                LeftChannelArea[areaIndex] = InternalLeftChannelBuffer[CurrentReadPosition];
                RightChannelArea[areaIndex] = InternalRightChannelBuffer[CurrentReadPosition];

                // Reset the current read position to the start of the buffer once we hit the end.
                CurrentReadPosition++;
                if (CurrentReadPosition == BufferSize)
                {
                    CurrentReadPosition = 0;
                }
            }

            lock(Lock)
            {
                AvailableData -= NumberOfSamplesToWrite;
            }
            WriteNotifier.Set();

        }

        public void Reset()
        {
            Array.Clear(InternalLeftChannelBuffer, 0, BufferSize);
            Array.Clear(InternalRightChannelBuffer, 0, BufferSize);
            CurrentReadPosition = 0;
            CurrentWritePosition = 0;
            AvailableData = 0;
        }

    }
}
