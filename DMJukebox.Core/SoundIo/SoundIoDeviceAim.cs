/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a C# implementation of the SoundIoDeviceAim enum in libsoundio.
    /// It describes whether a device is meant for input or for output. If a 
    /// device supports both, then libsoundio will create two separate devices
    /// with the same ID - one for input, one for output.
    /// </summary>
    /// <remarks>
    /// This enum is defined in soundio.h of libsoundio.
    /// For more information, please see the documentation at
    /// http://libsound.io/doc-1.1.0/soundio_8h.html
    /// or the source code at https://github.com/andrewrk/libsoundio.
    /// </remarks>
    internal enum SoundIoDeviceAim
    {
        /// <summary>
        /// This is an input (recording) device
        /// </summary>
        SoundIoDeviceAimInput,

        /// <summary>
        /// This is an output (playback) device
        /// </summary>
        SoundIoDeviceAimOutput
    }
}
