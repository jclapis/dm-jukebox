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
        /// <param name="PlaybackData">The incoming playback data</param>
        /// <param name="NumberOfSamplesToWrite">The number of samples to copy from the incoming data</param>
        public void AddPlaybackData(float[] PlaybackData, int NumberOfSamplesToWrite)
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

            // Once there's room available, copy the data! The first block handles the case where we don't need to wrap around to the beginning of the buffer.
            int headroom = BufferSize - CurrentWritePosition;
            if(headroom >= NumberOfSamplesToWrite)
            {
                for (int i = 0; i < NumberOfSamplesToWrite; i++)
                {
                    // The playback buffer is interleaved to make life easier for Opus support, so we have to
                    // uninterleave it here.
                    InternalLeftChannelBuffer[CurrentWritePosition] = PlaybackData[i * 2];
                    InternalRightChannelBuffer[CurrentWritePosition] = PlaybackData[i * 2 + 1];
                    CurrentWritePosition++;
                }
                if (CurrentWritePosition == BufferSize)
                {
                    CurrentWritePosition = 0;
                }
            }
            // The second block handles the case where we have too much incoming data and do have to wrap around.
            else
            {
                int overflow = NumberOfSamplesToWrite - headroom;
                for (int i = 0; i < headroom; i++)
                {
                    InternalLeftChannelBuffer[CurrentWritePosition] = PlaybackData[i * 2];
                    InternalRightChannelBuffer[CurrentWritePosition] = PlaybackData[i * 2 + 1];
                    CurrentWritePosition++;
                }
                CurrentWritePosition = 0;

                for (int i = 0; i < overflow; i++)
                {
                    int playbackIndex = (headroom + i) * 2;
                    InternalLeftChannelBuffer[CurrentWritePosition] = PlaybackData[playbackIndex];
                    InternalRightChannelBuffer[CurrentWritePosition] = PlaybackData[playbackIndex + 1];
                    CurrentWritePosition++;
                }
                if (CurrentWritePosition == BufferSize)
                {
                    CurrentWritePosition = 0;
                }
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
        /// Reset the buffer to an empty state. Note that this doesn't clear old data, but it will all
        /// get overwritten before playback anyway so it doesn't matter.
        /// </summary>
        public void Reset()
        {
            CurrentReadPosition = 0;
            CurrentWritePosition = 0;
            AvailableData = 0;
        }

    }
}
