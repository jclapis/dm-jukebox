using DiscordJukebox.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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

        public LocalSoundPlayer()
        {
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

        private void WriteSound(IntPtr StreamPtr, int MinFrameCount, int MaxFrameCount)
        {

        }

        private void HandleUnderflow(IntPtr StreamPtr)
        {

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
