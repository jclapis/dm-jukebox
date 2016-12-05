/*
 * Copyright (c) 2016 Joe Clapis.
 */

using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace DMJukebox
{
    /// <summary>
    /// DiscordPlaybackBuffer is a thread-safe circular buffer that takes incoming aggregated playback data, buffers it, and
    /// writes it to the SoundChannelAreas for the local audio playback system.
    /// </summary>
    internal class DiscordPlaybackBuffer
    {
        public const int BufferSize = AudioTrackManager.NumberOfPlaybackSamplesPerFrame * 20;

        private readonly float[] InternalBuffer;

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
        /// Creates a new DiscordPlaybackBuffer instance.
        /// </summary>
        public DiscordPlaybackBuffer()
        {
            InternalBuffer = new float[BufferSize];
            Lock = new object();
            ReadNotifier = new AutoResetEvent(false);
            WriteNotifier = new AutoResetEvent(false);
        }

        public void AddPlaybackData(float[] PlaybackData, int NumberOfSamplesToWrite)
        {
            NumberOfSamplesToWrite *= 2; // Since this data is interleaved, we really want to write twice as much.

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
            if (headroom >= NumberOfSamplesToWrite)
            {
                Buffer.BlockCopy(PlaybackData, 0, InternalBuffer, CurrentWritePosition * sizeof(float), NumberOfSamplesToWrite * sizeof(float));
                CurrentWritePosition += NumberOfSamplesToWrite;
                if (CurrentWritePosition == BufferSize)
                {
                    CurrentWritePosition = 0;
                }
            }
            // The second block handles the case where we have too much incoming data and do have to wrap around.
            else
            {
                int overflow = NumberOfSamplesToWrite - headroom;
                Buffer.BlockCopy(PlaybackData, 0, InternalBuffer, CurrentWritePosition * sizeof(float), headroom * sizeof(float));
                Buffer.BlockCopy(PlaybackData, headroom * sizeof(float), InternalBuffer, 0, overflow * sizeof(float));
                CurrentWritePosition = overflow;
                if (CurrentWritePosition == BufferSize)
                {
                    CurrentWritePosition = 0;
                }
            }

            lock (Lock)
            {
                AvailableData += NumberOfSamplesToWrite;
            }
            ReadNotifier.Set();
        }

        unsafe public void WritePlaybackDataToAudioBuffer(IntPtr AudioBuffer, int NumberOfSamplesToWrite)
        {
            NumberOfSamplesToWrite *= 2; // Since this data is interleaved, we really want to write twice as much.

            // This loop will cycle until there's enough data available to write all of the samples into the output buffer.
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

            int headroom = BufferSize - CurrentReadPosition;
            if (headroom >= NumberOfSamplesToWrite)
            {
                Marshal.Copy(InternalBuffer, CurrentReadPosition, AudioBuffer, NumberOfSamplesToWrite);
                CurrentReadPosition += NumberOfSamplesToWrite;
            }
            else
            {
                int overflow = NumberOfSamplesToWrite - headroom;
                Marshal.Copy(InternalBuffer, CurrentReadPosition, AudioBuffer, headroom);
                Marshal.Copy(InternalBuffer, 0, AudioBuffer + headroom * sizeof(float), overflow);
                CurrentReadPosition = overflow;
            }

            lock (Lock)
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
