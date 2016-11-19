/*
 * Copyright (c) 2016 Joe Clapis.
 */

using System;
using System.Threading;

namespace DMJukebox
{
    /// <summary>
    /// LocalPlaybackBuffer is a thread-safe circular buffer that takes incoming aggregated playback data, buffers it, and
    /// writes it to the SoundChannelAreas for the local audio playback system.
    /// </summary>
    internal class LocalPlaybackBuffer
    {
        /// <summary>
        /// The number of samples the buffer can hold. The smaller this is, the lower the latency between user interaction
        /// and playback results (such as setting the volume). It's set to 4800 by default which corresponds to 0.1 second
        /// of latency - this should be fine for most cases.
        /// </summary>
        public const int BufferSize = 4800;

        /// <summary>
        /// Buffer that stores the data for the left audio channel
        /// </summary>
        private readonly float[] InternalLeftChannelBuffer;

        /// <summary>
        /// Buffer that stores the data for the right audio channel
        /// </summary>
        private readonly float[] InternalRightChannelBuffer;

        /// <summary>
        /// Sync object for thread safety between the read and write threads
        /// </summary>
        private readonly object Lock;

        /// <summary>
        /// The position in the buffer where the next incoming round of data will be written to
        /// </summary>
        private int CurrentWritePosition;

        /// <summary>
        /// The position in the buffer where the player will read from
        /// </summary>
        private int CurrentReadPosition;

        /// <summary>
        /// A sync object for the read thread to wait on until there's enough data for it to read.
        /// </summary>
        private readonly AutoResetEvent ReadNotifier;

        /// <summary>
        /// A sync object for the write thread to wait on until the read thread has pulled out enough
        /// data that it has room to write new data into the buffer.
        /// </summary>
        private readonly AutoResetEvent WriteNotifier;

        /// <summary>
        /// The number of samples that are ready for reading
        /// </summary>
        public int AvailableData { get; private set; }

        /// <summary>
        /// Creates a new LocalPlaybackBuffer instance.
        /// </summary>
        public LocalPlaybackBuffer()
        {
            InternalLeftChannelBuffer = new float[BufferSize];
            InternalRightChannelBuffer = new float[BufferSize];
            Lock = new object();
            ReadNotifier = new AutoResetEvent(false);
            WriteNotifier = new AutoResetEvent(false);
        }

        /// <summary>
        /// Adds incoming aggregated playback data from the audio track decoding and mixing system into this buffer, making
        /// it ready for local playback.
        /// </summary>
        /// <param name="LeftChannelPlaybackData">The left channel of the incoming playback data</param>
        /// <param name="RightChannelPlaybackData">The right channel of the incoming playback data</param>
        /// <param name="NumberOfSamplesToWrite">The number of samples to copy from the incoming data</param>
        public void AddPlaybackData(float[] LeftChannelPlaybackData, float[] RightChannelPlaybackData, int NumberOfSamplesToWrite)
        {
            // This loop will cycle until there's enough free space to write all of the samples into the buffer.
            bool isNotReady;
            lock (Lock)
            {
                isNotReady = AvailableData + NumberOfSamplesToWrite > BufferSize;
            }
            while (isNotReady)
            {
                WriteNotifier.WaitOne();
                lock (Lock)
                {
                    isNotReady = AvailableData + NumberOfSamplesToWrite > BufferSize;
                }
            }

            // Once there's room available, copy the data! The first block handles the case where we don't need to wrap around to the beginning of the buffer,
            // the second block handles the case where we have too much incoming data and do have to wrap around.
            int headroom = BufferSize - CurrentWritePosition;
            if (headroom >= NumberOfSamplesToWrite)
            {
                Buffer.BlockCopy(LeftChannelPlaybackData, 0, InternalLeftChannelBuffer, CurrentWritePosition * sizeof(float), NumberOfSamplesToWrite * sizeof(float));
                Buffer.BlockCopy(RightChannelPlaybackData, 0, InternalRightChannelBuffer, CurrentWritePosition * sizeof(float), NumberOfSamplesToWrite * sizeof(float));
                CurrentWritePosition += NumberOfSamplesToWrite;
            }
            else
            {
                int overflow = NumberOfSamplesToWrite - headroom;
                Buffer.BlockCopy(LeftChannelPlaybackData, 0, InternalLeftChannelBuffer, CurrentWritePosition * sizeof(float), headroom * sizeof(float));
                Buffer.BlockCopy(LeftChannelPlaybackData, headroom * sizeof(float), InternalLeftChannelBuffer, 0, overflow * sizeof(float));
                Buffer.BlockCopy(RightChannelPlaybackData, 0, InternalRightChannelBuffer, CurrentWritePosition * sizeof(float), headroom * sizeof(float));
                Buffer.BlockCopy(RightChannelPlaybackData, headroom * sizeof(float), InternalRightChannelBuffer, 0, overflow * sizeof(float));
                CurrentWritePosition = overflow;
            }

            lock(Lock)
            {
                AvailableData += NumberOfSamplesToWrite;
            }
            ReadNotifier.Set();
        }

        /// <summary>
        /// Writes data from the playback buffers out to the sound areas, which will then be played on the local system's output.
        /// </summary>
        /// <param name="LeftChannelArea">The SoundChannelArea for the left channel</param>
        /// <param name="RightChannelArea">The SoundChannelArea for the right channel</param>
        /// <param name="NumberOfSamplesToWrite">The number of samples to write from the internal buffer into the sound areas</param>
        /// <param name="StepSize">The step size of the areas (how many bytes belong to each sample, per channel)</param>
        unsafe public void WritePlaybackDataToSoundAreas(float* LeftChannelArea, float* RightChannelArea, int NumberOfSamplesToWrite, int StepSize)
        {
            // This loop will cycle until there's enough data available to write all of the samples into the sound areas.
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

            // Because we have to space things out according to the step size, we can't just do a straight copy.
            for (int i = 0; i < NumberOfSamplesToWrite; i++)
            {
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

        /// <summary>
        /// Clears the buffer, resetting it to the empty state.
        /// </summary>
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
