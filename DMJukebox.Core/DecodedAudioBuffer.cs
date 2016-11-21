/*
 * Copyright (c) 2016 Joe Clapis.
 */

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DMJukebox
{
    /// <summary>
    /// DecodedAudioBuffer is a circular buffer for storing decoded data from an audio track and merging it into the
    /// aggregated playback buffer.
    /// </summary>
    internal class DecodedAudioBuffer
    {
        /// <summary>
        /// Buffer that stores the data for the left audio channel
        /// </summary>
        private readonly float[] InternalLeftChannelBuffer;

        /// <summary>
        /// Buffer that stores the data for the right audio channel
        /// </summary>
        private readonly float[] InternalRightChannelBuffer;

        /// <summary>
        /// The position in the buffer where the next incoming round of data will be written to
        /// </summary>
        private int CurrentWritePosition;

        /// <summary>
        /// The position in the buffer where the player will read from
        /// </summary>
        private int CurrentReadPosition;

        /// <summary>
        /// The size of the left and right buffers
        /// </summary>
        private readonly int BufferSize;

        /// <summary>
        /// The number of samples that are stored in the buffer, waiting to be read.
        /// </summary>
        public int AvailableData { get; private set; }

        /// <summary>
        /// Creates a new DecodedAudioBuffer instance.
        /// </summary>
        /// <param name="BufferSize">The number of samples for each channel to store.</param>
        public DecodedAudioBuffer(int BufferSize)
        {
            this.BufferSize = BufferSize;
            InternalLeftChannelBuffer = new float[BufferSize];
            InternalRightChannelBuffer = new float[BufferSize];
        }

        /// <summary>
        /// Writes incoming data from the decoder to the buffer.
        /// </summary>
        /// <param name="IncomingLeftChannelData">Pointer to the new decoded left channel data from the output frame</param>
        /// <param name="IncomingRightChannelData">Pointer to the new decoded right channel data from the output frame</param>
        /// <param name="NumberOfSamplesToWrite">The number of samples to write into this buffer from the unmanaged buffers</param>
        public void AddIncomingData(IntPtr IncomingLeftChannelData, IntPtr IncomingRightChannelData, int NumberOfSamplesToWrite)
        {
            // This shouldn't ever be a thing, if it happens then it means we didn't make the buffer big enough (which implies
            // that the frame_size provided by ffmpeg was a lie).
            if (AvailableData + NumberOfSamplesToWrite > BufferSize)
            {
                throw new Exception("Circular buffer overflow, this should never happen but it did. Disaster.");
            }

            // If we don't need to do a wrap-around and can just write straight into the buffer from the current position,
            // all we need to do is copy from the source to the target.
            int headroom = BufferSize - CurrentWritePosition;
            if (headroom >= NumberOfSamplesToWrite)
            {
                Marshal.Copy(IncomingLeftChannelData, InternalLeftChannelBuffer, CurrentWritePosition, NumberOfSamplesToWrite);
                Marshal.Copy(IncomingRightChannelData, InternalRightChannelBuffer, CurrentWritePosition, NumberOfSamplesToWrite);
                CurrentWritePosition += NumberOfSamplesToWrite;
            }

            // Otherwise, we have to do a partial copy until we hit the end of the buffer, then wrap around from the start.
            else
            {
                int overflow = NumberOfSamplesToWrite - headroom;
                Marshal.Copy(IncomingLeftChannelData, InternalLeftChannelBuffer, CurrentWritePosition, headroom);
                Marshal.Copy(IncomingLeftChannelData + headroom * sizeof(float), InternalLeftChannelBuffer, 0, overflow);
                Marshal.Copy(IncomingRightChannelData, InternalRightChannelBuffer, CurrentWritePosition, headroom);
                Marshal.Copy(IncomingRightChannelData + headroom * sizeof(float), InternalRightChannelBuffer, 0, overflow);
                CurrentWritePosition = overflow;
            }

            AvailableData += NumberOfSamplesToWrite;
        }

        /// <summary>
        /// Writes stored, decoded audio data into the aggregated buffer for playback. This will combine the data from the stream
        /// that owns this buffer into the playback buffer and takes care of stream-specific volume control.
        /// </summary>
        /// <param name="PlaybackBuffer">The playback buffer</param>
        /// <param name="NumberOfSamplesToWrite">The number of decoded samples to write into the playback buffers</param>
        /// <param name="Volume">The volume control to apply to this data (0.0 = muted, 1.0 = full volume)</param>
        /// <param name="OverwriteExistingData">True to replace whatever's in the playback buffer with the decoded data
        /// in this buffer, false to append this data to whatever's already inside the playback buffer. This is usually set to true
        /// for the first stream in a playback loop iteration, to overwrite the old stale data from the previous loop. After that it's
        /// set to false.</param>
        /// <remarks>
        /// This might seem like a weird way to do things (having the buffer do the mixing and volume control) instead of a class
        /// specifically designated to do that, but doing it this way eliminates unnecessary copying and makes the volume control
        /// happen as late as possible during the pipeline (which makes it feel responsive instead of laggy). At this point it's
        /// all about performance.
        /// </remarks>
        public void WriteDataIntoPlaybackBuffer(float[] PlaybackBuffer, int NumberOfSamplesToWrite, float Volume, bool OverwriteExistingData)
        {
            // This shouldn't ever be a thing because the player should always confirm that this buffer has enough data in it to fill the playback buffers.
            // If it doesn't, it should continuously read from the stream and store the decoded data here until it can cover the playback buffers.
            if (NumberOfSamplesToWrite > AvailableData)
            {
                throw new Exception("Circular buffer underflow, this should never happen but it did. Disaster.");
            }

            // This isn't a mass copy like AddIncomingData is, we have to iterate through the buffers piece-by-piece in order to
            // apply volume control and clamp it.
            for (int i = 0; i < NumberOfSamplesToWrite; i++)
            {
                int leftPlaybackIndex = i * 2;
                int rightPlaybackIndex = i * 2 + 1;
                float newLeftValue = InternalLeftChannelBuffer[CurrentReadPosition] * Volume;
                float newRightValue = InternalRightChannelBuffer[CurrentReadPosition] * Volume;
                if (!OverwriteExistingData)
                {
                    // If this is the first stream, then whatever is in the buffer is considered old and gets overwritten.
                    // If it isn't the first stream, then the new value gets added to the old value.
                    newLeftValue += PlaybackBuffer[leftPlaybackIndex];
                    newRightValue += PlaybackBuffer[rightPlaybackIndex];
                }

                // The playback buffer has to be interleaved to make life easier for Opus support, which is why
                // there's only one overall buffer instead of one per channel.
                PlaybackBuffer[leftPlaybackIndex] = Clamp(newLeftValue);
                PlaybackBuffer[rightPlaybackIndex] = Clamp(newRightValue);

                // Reset the current read position to the start of the buffer once we hit the end.
                CurrentReadPosition++;
                if (CurrentReadPosition == BufferSize)
                {
                    CurrentReadPosition = 0;
                }
            }

            AvailableData -= NumberOfSamplesToWrite;
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

        /// <summary>
        /// Clamps a sample from -1.0 to 1.0 so it's within the valid range of floating-point audio.
        /// </summary>
        /// <param name="Value">The value to clamp</param>
        /// <returns>The clamped value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float Clamp(float Value)
        {
            if(Value < -1.0f)
            {
                return -1.0f;
            }
            if(Value > 1.0f)
            {
                return 1.0f;
            }
            return Value;
        }

    }
}
