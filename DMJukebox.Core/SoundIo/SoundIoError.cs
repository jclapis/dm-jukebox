/* 
 * This file contains a C# implementation of the SoundIoError enum
 * as defined in soundio.h of the libsoundio project, for interop use.
 * 
 * For more information, please see the documentation at
 * http://libsound.io/doc-1.1.0/soundio_8h.html or the source code at
 * https://github.com/andrewrk/libsoundio.
 * 
 * Copyright (c) 2016 Joe Clapis.
 */

namespace DMJukebox.Interop
{
    internal enum SoundIoError
    {
        SoundIoErrorNone,
        SoundIoErrorNoMem,
        SoundIoErrorInitAudioBackend,
        SoundIoErrorSystemResources,
        SoundIoErrorOpeningDevice,
        SoundIoErrorNoSuchDevice,
        SoundIoErrorInvalid,
        SoundIoErrorBackendUnavailable,
        SoundIoErrorStreaming,
        SoundIoErrorIncompatibleDevice,
        SoundIoErrorNoSuchClient,
        SoundIoErrorIncompatibleBackend,
        SoundIoErrorBackendDisconnected,
        SoundIoErrorInterrupted,
        SoundIoErrorUnderflow,
        SoundIoErrorEncodingString
    }
}
