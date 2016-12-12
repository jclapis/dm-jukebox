/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace DMJukebox
{
    /// <summary>
    /// DiscordPlaybackBuffer is a thread-safe circular buffer that takes incoming aggregated playback data, buffers it, and
    /// writes it to the output buffer that will ultimately be sent to Discord.
    /// </summary>
    internal class DiscordPlaybackBuffer
    {
        /// <summary>
        /// The size of the internal buffer
        /// </summary>
        private readonly int BufferSize;

        /// <summary>
        /// The internal buffer for storing playback data
        /// </summary>
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
        private int AvailableData;

        /// <summary>
        /// This is a flag that lets <see cref="WritePlaybackDataToAudioBuffer(IntPtr, int)"/> know
        /// when the reader thread is actually closing instead of trying to read data, so it should
        /// just break out and return without sending anything to the output buffers.
        /// </summary>
        private bool IsResetting;

        /// <summary>
        /// Creates a new DiscordPlaybackBuffer instance.
        /// </summary>
        public DiscordPlaybackBuffer(int BufferSize)
        {
            this.BufferSize = BufferSize;
            InternalBuffer = new float[BufferSize];
            Lock = new object();
            ReadNotifier = new AutoResetEvent(false);
            WriteNotifier = new AutoResetEvent(false);
        }

        /// <summary>
        /// Adds playback data ready for streaming into the buffer.
        /// </summary>
        /// <param name="PlaybackData">The playback audio to add, in interleaved (packed) format</param>
        /// <param name="NumberOfSamplesToWrite">The number of samples to write into this buffer</param>
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

        /// <summary>
        /// Writes buffered playback audio into the output buffer for Discord.
        /// </summary>
        /// <param name="AudioBuffer">The output buffer which will be sent to Discord</param>
        /// <param name="NumberOfSamplesToWrite">The number of samples to write from the
        /// internal buffer to the Discord output</param>
        public void WritePlaybackDataToAudioBuffer(IntPtr AudioBuffer, int NumberOfSamplesToWrite)
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
                if(IsResetting)
                {
                    IsResetting = false;
                    return;
                }
                lock (Lock)
                {
                    isNotReady = NumberOfSamplesToWrite > AvailableData;
                }
            }

            // Do a straight copy if there's enough contiguous data to write all at once.
            int headroom = BufferSize - CurrentReadPosition;
            if (headroom >= NumberOfSamplesToWrite)
            {
                Marshal.Copy(InternalBuffer, CurrentReadPosition, AudioBuffer, NumberOfSamplesToWrite);
                CurrentReadPosition += NumberOfSamplesToWrite;
                if(CurrentReadPosition == BufferSize)
                {
                    CurrentReadPosition = 0;
                }
            }

            // Otherwise, we have to break the operation into two writes because of wrap-around.
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
        /// Sometimes, the playback thread in <see cref="WritePlaybackDataToAudioBuffer(IntPtr, int)"/> 
        /// is stuck waiting for new data to come in (for <see cref="ReadNotifier"/> to get set) when 
        /// we're actually closing down the playback thread. Use this to set it and unblock the playback
        /// thread while setting the flag that tells it we're shutting down so it just returns.
        /// </summary>
        public void ReleasePlaybackWaiter()
        {
            IsResetting = true;
            ReadNotifier.Set();
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
