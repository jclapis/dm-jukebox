using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DiscordJukebox
{
    internal class AudioStreamBuffer
    {
        private readonly float[] InternalLeftChannelBuffer;

        private readonly float[] InternalRightChannelBuffer;

        private int CurrentWritePosition;

        private int CurrentReadPosition;

        private readonly int BufferSize;

        public int AvailableData { get; private set; }

        public AudioStreamBuffer(int BufferSize)
        {
            this.BufferSize = BufferSize;
            InternalLeftChannelBuffer = new float[BufferSize];
            InternalRightChannelBuffer = new float[BufferSize];
        }

        public void AddIncomingData(IntPtr IncomingLeftChannelData, IntPtr IncomingRightChannelData, int NumberOfSamplesToWrite)
        {
            if (AvailableData + NumberOfSamplesToWrite > BufferSize)
            {
                throw new Exception("Circular buffer overflow, this should never happen but it did. Disaster.");
            }

            int headroom = BufferSize - CurrentWritePosition;
            if (headroom >= NumberOfSamplesToWrite)
            {
                Marshal.Copy(IncomingLeftChannelData, InternalLeftChannelBuffer, CurrentWritePosition, NumberOfSamplesToWrite);
                Marshal.Copy(IncomingRightChannelData, InternalRightChannelBuffer, CurrentWritePosition, NumberOfSamplesToWrite);
                CurrentWritePosition += NumberOfSamplesToWrite;
            }
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

        public void WriteDataIntoMergeBuffers(float[] LeftChannelMergeBuffer, float[] RightChannelMergeBuffer, int NumberOfBytesToRead, float Volume, bool OverwriteExistingData)
        {
            if (NumberOfBytesToRead > AvailableData)
            {
                throw new Exception("Circular buffer underflow, this should never happen but it did. Disaster.");
            }

            for (int i = 0; i < NumberOfBytesToRead; i++)
            {
                float newLeftValue = InternalLeftChannelBuffer[CurrentReadPosition] * Volume;
                float newRightValue = InternalRightChannelBuffer[CurrentReadPosition] * Volume;
                if (!OverwriteExistingData)
                {
                    // If this is the first stream, then whatever is in the buffer is considered old and gets overwritten.
                    // If it isn't the first stream, then the new value gets added to the old value.
                    newLeftValue += LeftChannelMergeBuffer[i];
                    newRightValue += RightChannelMergeBuffer[i];
                }
                LeftChannelMergeBuffer[i] = Clamp(newLeftValue);
                RightChannelMergeBuffer[i] = Clamp(newRightValue);

                // Reset the current read position once we hit the end of the buffer.
                CurrentReadPosition++;
                if (CurrentReadPosition == BufferSize)
                {
                    CurrentReadPosition = 0;
                }
            }

            AvailableData -= NumberOfBytesToRead;
        }

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
