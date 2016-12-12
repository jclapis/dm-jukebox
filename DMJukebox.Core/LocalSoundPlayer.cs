/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

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
        /// This is the <see cref="SoundIo"/> context object that libsoundio uses.
        /// </summary>
        private readonly IntPtr SoundIoPtr;

        /// <summary>
        /// This is the <see cref="SoundIoDevice"/> (the audio playback device).
        /// </summary>
        private readonly IntPtr SoundIoDevicePtr;

        /// <summary>
        /// This is the <see cref="SoundIoOutStream"/> (the stream for writing data to the speakers).
        /// </summary>
        private IntPtr SoundIoOutStreamPtr;

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
        }

        /// <summary>
        /// Begins the playback thread that writes output audio to the speakers.
        /// </summary>
        public void Start()
        {
            if(IsPlaying)
            {
                return;
            }

            // Create the output stream. This has to be done every time the player starts playback
            // because when it stops, we have to destroy the stream entirely.
            SoundIoOutStreamPtr = SoundIoInterop.soundio_outstream_create(SoundIoDevicePtr);
            SoundIoOutStream stream = Marshal.PtrToStructure<SoundIoOutStream>(SoundIoOutStreamPtr);
            stream.write_callback = WriteSoundDelegate;
            stream.underflow_callback = HandleUnderflowDelegate;
            stream.name = StreamName;
            stream.software_latency = 0;
            stream.sample_rate = 48000;
            stream.format = SoundIoFormat.SoundIoFormatFloat32LE;
            Marshal.StructureToPtr(stream, SoundIoOutStreamPtr, false);

            // Open the stream and start playback.
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

        /// <summary>
        /// Stops the playback thread.
        /// </summary>
        public void Stop()
        {
            // We have to destroy the output stream here because if we just pause, then there will be
            // residual data left over which causes a really obnoxious "burp" of the previous few frames
            // that were written. I couldn't get this to stop no matter how many buffer clears I put in
            // or how many sync techniques I tried, so at the end of day it's easiest to just kill the
            // entire output channel and start over from scratch when playback resumes.
            SoundIoInterop.soundio_outstream_destroy(SoundIoOutStreamPtr);
            SoundIoOutStreamPtr = IntPtr.Zero;
            Buffer.Reset();
            IsPlaying = false;
        }

        /// <summary>
        /// This is the callback that writes sound data out to the speakers.
        /// </summary>
        /// <param name="OutStream">The output stream (this is just SoundIoOutStreamPtr).</param>
        /// <param name="MinFrameCount">The smallest number of frames that can be written during this call</param>
        /// <param name="MaxFrameCount">The largets number of frames that can be written during this call</param>
        /// <remarks>
        /// So for WASAPI, the system will break unless you actually write MaxFrameCount number of frames. Anything
        /// less and it blows up. I figure since we're running a circular buffer behind the scenes and data comes
        /// in way faster than it goes out anyway, let's just write the full number of frames no matter what
        /// backend libsoundio winds up using.
        /// </remarks>
        unsafe private void WriteSound(IntPtr OutStream, int MinFrameCount, int MaxFrameCount)
        {
            IntPtr soundAreas = IntPtr.Zero;
            int frameCount = MaxFrameCount;

            // Start writing output to the speakers
            SoundIoError result = SoundIoInterop.soundio_outstream_begin_write(OutStream, ref soundAreas, ref frameCount);
            if (result != SoundIoError.SoundIoErrorNone)
            {
                throw new Exception($"Writing to the local sound driver failed on begin: {result}");
            }
            
            // Write the max number of allowed frames. Note that WritePlaybackDataToSoundAreas() will block
            // until there's enough data in the buffer to satisfy this entire write request.
            SoundIoChannelArea* areas = (SoundIoChannelArea*)soundAreas.ToPointer();
            float* leftChannelArea = areas[LeftChannelId].ptr;
            float* rightChannelArea = areas[RightChannelId].ptr;
            int stepSize = areas[LeftChannelId].step / sizeof(float);
            Buffer.WritePlaybackDataToSoundAreas(leftChannelArea, rightChannelArea, frameCount, stepSize);

            // Finish the write, and send the data out to the speakers.
            result = SoundIoInterop.soundio_outstream_end_write(OutStream);
            if (result != SoundIoError.SoundIoErrorNone)
            {
                throw new Exception($"Writing to the local sound driver failed on end: {result}");
            }
        }

        /// <summary>
        /// This gets called by libsoundio when the playback thread gets starved and there's not enough in the buffer
        /// to keep it going smoothly. This is very rare, it only happens when something clogs the decoding thread.
        /// </summary>
        /// <param name="StreamPtr">The output stream that suffered from an underflow</param>
        private void HandleUnderflow(IntPtr StreamPtr)
        {
            System.Diagnostics.Debug.WriteLine("Underflow detected.");
        }

        /// <summary>
        /// Adds samples of decoded, playback-ready data to this player so they can be sent out to the speakers.
        /// </summary>
        /// <param name="PlaybackData">The buffer with the data to write, in interleaved (packed) format</param>
        /// <param name="NumberOfSamplesToWrite">The number of samples from each channel to add to the player</param>
        public void AddPlaybackData(float[] PlaybackData, int NumberOfSamplesToWrite)
        {
            Buffer.AddPlaybackData(PlaybackData, NumberOfSamplesToWrite);
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

                Stop();
                if(SoundIoOutStreamPtr != IntPtr.Zero)
                {
                    SoundIoInterop.soundio_outstream_destroy(SoundIoOutStreamPtr);
                }
                if(SoundIoDevicePtr != IntPtr.Zero)
                {
                    SoundIoInterop.soundio_device_unref(SoundIoDevicePtr);
                }
                if(SoundIoPtr != IntPtr.Zero)
                {
                    SoundIoInterop.soundio_destroy(SoundIoPtr);
                }

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
