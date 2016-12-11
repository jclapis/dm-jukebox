/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a C# implementation of the SoundIoChannelArea struct in libsoundio.
    /// It is a buffer allocated by libsoundio that you can write a single channel
    /// of audio data into for speaker playback.
    /// </summary>
    /// <remarks>
    /// This struct is defined in soundio.h of libsoundio.
    /// For more information, please see the documentation at
    /// http://libsound.io/doc-1.1.0/soundio_8h.html
    /// or the source code at https://github.com/andrewrk/libsoundio.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    unsafe internal struct SoundIoChannelArea
    {
        /// <summary>
        /// The buffer for holding playback data.
        /// </summary>
        /// <remarks>
        /// In libsoundio this is defined as a char*, but because I'm always going
        /// to be writing data in float format, it's more convenient to consider
        /// it a float* here.
        /// </remarks>
        public float* ptr;
        
        /// <summary>
        /// The number of bytes between samples (this is read-only, so you must
        /// respect it while writing data to the buffer).
        /// </summary>
        public int step;
    }
}
