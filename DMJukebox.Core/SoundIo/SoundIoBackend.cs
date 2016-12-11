/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a C# implementation of the SoundIoBackend enum in libsoundio.
    /// It defines the different audio I/O backends that the system supports.
    /// </summary>
    /// <remarks>
    /// This enum is defined in soundio.h of libsoundio.
    /// For more information, please see the documentation at
    /// http://libsound.io/doc-1.1.0/soundio_8h.html
    /// or the source code at https://github.com/andrewrk/libsoundio.
    /// </remarks>
    internal enum SoundIoBackend
    {
        SoundIoBackendNone,
        SoundIoBackendJack,
        SoundIoBackendPulseAudio,
        SoundIoBackendAlsa,
        SoundIoBackendCoreAudio,
        SoundIoBackendWasapi,
        SoundIoBackendDummy
    }
}
