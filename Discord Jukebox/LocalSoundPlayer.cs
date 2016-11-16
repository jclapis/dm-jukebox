using DiscordJukebox.Interop;
using System;
using System.Runtime.InteropServices;

namespace DiscordJukebox
{
    internal class LocalSoundPlayer : IDisposable
    {
        private const string StreamName = "Discord Jukebox Dev Stream";

        private readonly IntPtr SoundIoPtr;

        private readonly IntPtr SoundIoDevicePtr;

        private readonly IntPtr SoundIoOutStreamPtr;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        private readonly write_callback WriteSoundDelegate;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        private readonly underflow_callback HandleUnderflowDelegate;

        private bool Started;

        private readonly ThreadSafeCircularBuffer Buffer;

        private readonly int LeftChannelId;

        private readonly int RightChannelId;

        private readonly float[] LeftChannelData;

        private readonly float[] RightChannelData;

        public LocalSoundPlayer()
        {
            Buffer = new ThreadSafeCircularBuffer();
            LeftChannelData = new float[ThreadSafeCircularBuffer.BufferSize];
            RightChannelData = new float[ThreadSafeCircularBuffer.BufferSize];
            WriteSoundDelegate = WriteSound;
            HandleUnderflowDelegate = HandleUnderflow;
            
            SoundIoPtr = SoundIoInterop.soundio_create();
            SoundIoError result = SoundIoInterop.soundio_connect(SoundIoPtr);
            if (result != SoundIoError.SoundIoErrorNone)
            {
                throw new Exception($"Connecting local audio to the backend failed: {result}");
            }

            SoundIoInterop.soundio_flush_events(SoundIoPtr);
            int defaultDeviceIndex = SoundIoInterop.soundio_default_output_device_index(SoundIoPtr);
            SoundIoDevicePtr = SoundIoInterop.soundio_get_output_device(SoundIoPtr, defaultDeviceIndex);
            SoundIoDevice device = Marshal.PtrToStructure<SoundIoDevice>(SoundIoDevicePtr);
            IntPtr defaultLayout = device.layouts;
            LeftChannelId = SoundIoInterop.soundio_channel_layout_find_channel(defaultLayout, SoundIoChannelId.SoundIoChannelIdFrontLeft);
            RightChannelId = SoundIoInterop.soundio_channel_layout_find_channel(defaultLayout, SoundIoChannelId.SoundIoChannelIdFrontRight);
            if(LeftChannelId == -1 || RightChannelId == -1)
            {
                SoundIoChannelLayout layout = Marshal.PtrToStructure<SoundIoChannelLayout>(defaultLayout);
                throw new Exception($"Local sound device {device.name} doesn't support stereo playback, couldn't find the front left or front right channels. Layout was {layout.name}.");
            }
            
            SoundIoOutStreamPtr = SoundIoInterop.soundio_outstream_create(SoundIoDevicePtr);
            SoundIoOutStream stream = Marshal.PtrToStructure<SoundIoOutStream>(SoundIoOutStreamPtr);
            stream.write_callback = WriteSoundDelegate;
            stream.underflow_callback = HandleUnderflowDelegate;
            stream.name = StreamName;
            stream.software_latency = 0;
            stream.sample_rate = 48000;
            stream.format = SoundIoFormat.SoundIoFormatFloat32LE;
            Marshal.StructureToPtr(stream, SoundIoOutStreamPtr, false);

            result = SoundIoInterop.soundio_outstream_open(SoundIoOutStreamPtr);
            if (result != SoundIoError.SoundIoErrorNone)
            {
                throw new Exception($"Opening the local sound stream failed: {result}");
            }
        }

        public void Start()
        {
            SoundIoError result;
            if (!Started)
            {
                result = SoundIoInterop.soundio_outstream_start(SoundIoOutStreamPtr);
                if (result != SoundIoError.SoundIoErrorNone)
                {
                    throw new Exception($"Starting local sound playback failed: {result}");
                }
                Started = true;
            }
            result = SoundIoInterop.soundio_outstream_pause(SoundIoOutStreamPtr, false);
            if (result != SoundIoError.SoundIoErrorNone)
            {
                throw new Exception($"Resuming local sound playback failed: {result}");
            }
        }

        public void Pause()
        {
            SoundIoError result = SoundIoInterop.soundio_outstream_pause(SoundIoOutStreamPtr, true);
            if (result != SoundIoError.SoundIoErrorNone)
            {
                throw new Exception($"Pausing local sound playback failed: {result}");
            }
        }

        unsafe private void WriteSound(IntPtr StreamPtr, int MinFrameCount, int MaxFrameCount)
        {
            IntPtr soundAreas = IntPtr.Zero;
            SoundIoOutStream stream = Marshal.PtrToStructure<SoundIoOutStream>(StreamPtr);

            int frameCount = MaxFrameCount;
            SoundIoError result = SoundIoInterop.soundio_outstream_begin_write(StreamPtr, ref soundAreas, ref frameCount);
            if (result != SoundIoError.SoundIoErrorNone)
            {
                throw new Exception($"Writing to the local sound driver failed on begin: {result}");
            }
            
            Buffer.Read(LeftChannelData, RightChannelData, MaxFrameCount);
            SoundIoChannelArea* areas = (SoundIoChannelArea*)soundAreas.ToPointer();
            float* leftChannelArea = areas[LeftChannelId].ptr;
            float* rightChannelArea = areas[RightChannelId].ptr;
            int stepSize = areas[LeftChannelId].step / sizeof(float);
            for(int currentFrame = 0; currentFrame < MaxFrameCount; currentFrame++)
            {
                int areaIndex = stepSize * currentFrame;
                leftChannelArea[areaIndex] = LeftChannelData[currentFrame];
                rightChannelArea[areaIndex] = RightChannelData[currentFrame];
            }

            result = SoundIoInterop.soundio_outstream_end_write(StreamPtr);
            if (result != SoundIoError.SoundIoErrorNone)
            {
                throw new Exception($"Writing to the local sound driver failed on end: {result}");
            }
        }

        private void HandleUnderflow(IntPtr StreamPtr)
        {
            System.Diagnostics.Debug.WriteLine("Underflow detected.");
        }

        public void WriteData(float[] LeftMuxBuffer, float[] RightMuxBuffer, int NumberOfSamples)
        {
            Buffer.Write(LeftMuxBuffer, RightMuxBuffer, NumberOfSamples);
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                SoundIoInterop.soundio_outstream_destroy(SoundIoOutStreamPtr);
                SoundIoInterop.soundio_device_unref(SoundIoDevicePtr);
                SoundIoInterop.soundio_destroy(SoundIoPtr);

                disposedValue = true;
            }
        }

        ~LocalSoundPlayer()
        {
            Dispose(false);
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
