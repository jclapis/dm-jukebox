/*
 * Copyright (c) 2016 Joe Clapis.
 */

using DMJukebox.Interop;
using System;
using System.Runtime.InteropServices;

namespace DMJukebox
{
    /// <summary>
    /// LocalSoundPlayer is the class that deals with playing sound to the local machine's audio output.
    /// </summary>
    internal class LocalSoundPlayer : IDisposable
    {
        /// <summary>
        /// libsoundio suggests giving the stream a unique name to help identify it among others,
        /// and I'm pretty sure this is unique.
        /// </summary>
        private const string StreamName = "DM Jukebox Local Audio Playback";

        /// <summary>
        /// This is the SoundIo context object that libsoundio uses.
        /// </summary>
        private readonly IntPtr SoundIoPtr;

        /// <summary>
        /// This is the SoundIoDevice (the audio playback device).
        /// </summary>
        private readonly IntPtr SoundIoDevicePtr;

        /// <summary>
        /// This is the SoundIoOutStream (the stream for writing data to the speakers).
        /// </summary>
        private readonly IntPtr SoundIoOutStreamPtr;

        /// <summary>
        /// This delegate gets passed to libsoundio as the callback for sending audio to the speakers.
        /// </summary>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        private readonly write_callback WriteSoundDelegate;

        /// <summary>
        /// This delegate is the callback that libsoundio calls when there's a buffer underflow detected.
        /// </summary>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        private readonly underflow_callback HandleUnderflowDelegate;

        /// <summary>
        /// This is a flag to keep track of whether or not this instance is current playing. 
        /// </summary>
        private bool IsPlaying;

        /// <summary>
        /// This is the buffer used to store data that's ready for playback, so the sound thread can 
        /// read from it and play it.
        /// </summary>
        private readonly LocalPlaybackBuffer Buffer;

        /// <summary>
        /// This is the index of the SoundIoChannelArea that corresponds to the left channel
        /// (basically, this is the index of the output buffer to write left channel data to).
        /// </summary>
        private readonly int LeftChannelId;

        /// <summary>
        /// This is the index of the SoundIoChannelArea that corresponds to the right channel
        /// (basically, this is the index of the output buffer to write right channel data to).
        /// </summary>
        private readonly int RightChannelId;

        /// <summary>
        /// Creates a new LocalSoundPlayer instance.
        /// </summary>
        public LocalSoundPlayer()
        {
            Buffer = new LocalPlaybackBuffer();
            WriteSoundDelegate = WriteSound;
            HandleUnderflowDelegate = HandleUnderflow;
            
            // Create the SoundIo context.
            SoundIoPtr = SoundIoInterop.soundio_create();
            SoundIoError result = SoundIoInterop.soundio_connect(SoundIoPtr);
            if (result != SoundIoError.SoundIoErrorNone)
            {
                throw new Exception($"Connecting local audio to the backend failed: {result}");
            }
            SoundIoInterop.soundio_flush_events(SoundIoPtr);

            // Get the default playback device and figure out where the left and right channels are.
            int defaultDeviceIndex = SoundIoInterop.soundio_default_output_device_index(SoundIoPtr);
            SoundIoDevicePtr = SoundIoInterop.soundio_get_output_device(SoundIoPtr, defaultDeviceIndex);
            SoundIoDevice device = Marshal.PtrToStructure<SoundIoDevice>(SoundIoDevicePtr);
            IntPtr defaultLayout = device.layouts; // We just want the first layout here, so we don't have to turn it into an array.
            LeftChannelId = SoundIoInterop.soundio_channel_layout_find_channel(defaultLayout, SoundIoChannelId.SoundIoChannelIdFrontLeft);
            RightChannelId = SoundIoInterop.soundio_channel_layout_find_channel(defaultLayout, SoundIoChannelId.SoundIoChannelIdFrontRight);
            if(LeftChannelId == -1 || RightChannelId == -1)
            {
                SoundIoChannelLayout layout = Marshal.PtrToStructure<SoundIoChannelLayout>(defaultLayout);
                throw new Exception($"Local sound device {device.name} doesn't support stereo playback, couldn't find the front left or front right channels. Layout was {layout.name}.");
            }
            
            // Create the output stream.
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
            if(IsPlaying)
            {
                return;
            }

            SoundIoError result = SoundIoInterop.soundio_outstream_open(SoundIoOutStreamPtr);
            if (result != SoundIoError.SoundIoErrorNone)
            {
                throw new Exception($"Opening the local sound stream failed: {result}");
            }
            result = SoundIoInterop.soundio_outstream_start(SoundIoOutStreamPtr);
            if (result != SoundIoError.SoundIoErrorNone)
            {
                throw new Exception($"Starting local sound playback failed: {result}");
            }
            IsPlaying = true;
        }

        public void Stop()
        {
            /*SoundIoError result = SoundIoInterop.soundio_outstream_pause(SoundIoOutStreamPtr, true);
            if (result != SoundIoError.SoundIoErrorNone)
            {
                throw new Exception($"Pausing local sound playback failed: {result}");
            }*/
            SoundIoInterop.soundio_outstream_destroy(SoundIoOutStreamPtr);
        }

        unsafe private void WriteSound(IntPtr StreamPtr, int MinFrameCount, int MaxFrameCount)
        {
            IntPtr soundAreas = IntPtr.Zero;
            int frameCount = MaxFrameCount;
            SoundIoError result = SoundIoInterop.soundio_outstream_begin_write(StreamPtr, ref soundAreas, ref frameCount);
            if (result != SoundIoError.SoundIoErrorNone)
            {
                throw new Exception($"Writing to the local sound driver failed on begin: {result}");
            }
            
            SoundIoChannelArea* areas = (SoundIoChannelArea*)soundAreas.ToPointer();
            float* leftChannelArea = areas[LeftChannelId].ptr;
            float* rightChannelArea = areas[RightChannelId].ptr;
            int stepSize = areas[LeftChannelId].step / sizeof(float);
            Buffer.WritePlaybackDataToSoundAreas(leftChannelArea, rightChannelArea, frameCount, stepSize);

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

        public void WriteData(float[] LeftMergeBuffer, float[] RightMergeBuffer, int NumberOfSamples)
        {
            Buffer.AddPlaybackData(LeftMergeBuffer, RightMergeBuffer, NumberOfSamples);
        }

        public void ResetBuffer()
        {
            Buffer.Reset();
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
