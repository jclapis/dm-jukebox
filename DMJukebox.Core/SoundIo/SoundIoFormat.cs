/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a C# implementation of the SoundIoFormat enum in libsoundio.
    /// It describes the binary format of an audio sample.
    /// </summary>
    /// <remarks>
    /// This enum is defined in soundio.h of libsoundio.
    /// For more information, please see the documentation at
    /// http://libsound.io/doc-1.1.0/soundio_8h.html
    /// or the source code at https://github.com/andrewrk/libsoundio.
    /// </remarks>
    internal enum SoundIoFormat
    {
        /// <summary>
        /// Unspecified / invalid
        /// </summary>
        SoundIoFormatInvalid,

        /// <summary>
        /// Signed 8-bit
        /// </summary>
        SoundIoFormatS8,

        /// <summary>
        /// Unsigned 8-bit
        /// </summary>
        SoundIoFormatU8,

        /// <summary>
        /// Signed 16-bit (little endian)
        /// </summary>
        SoundIoFormatS16LE,

        /// <summary>
        /// Signed 16-bit (big endian)
        /// </summary>
        SoundIoFormatS16BE,

        /// <summary>
        /// Unsigned 16-bit (little endian)
        /// </summary>
        SoundIoFormatU16LE,

        /// <summary>
        /// Unsigned 16-bit (big endian)
        /// </summary>
        SoundIoFormatU16BE,

        /// <summary>
        /// Signed 24-bit (little endian),
        /// which uses the low 24 bits of an int
        /// </summary>
        SoundIoFormatS24LE,

        /// <summary>
        /// Signed 24-bit (big endian),
        /// which uses the low 24 bits of an int
        /// </summary>
        SoundIoFormatS24BE,

        /// <summary>
        /// Unsigned 24-bit (little endian),
        /// which uses the low 24 bits of an int
        /// </summary>
        SoundIoFormatU24LE,

        /// <summary>
        /// Unsigned 24-bit (big endian),
        /// which uses the low 24 bits of an int
        /// </summary>
        SoundIoFormatU24BE,

        /// <summary>
        /// Signed 32-bit (little endian)
        /// </summary>
        SoundIoFormatS32LE,

        /// <summary>
        /// Signed 32-bit (big endian)
        /// </summary>
        SoundIoFormatS32BE,

        /// <summary>
        /// Unsigned 32-bit (little endian)
        /// </summary>
        SoundIoFormatU32LE,

        /// <summary>
        /// Unsigned 32-bit (big endian)
        /// </summary>
        SoundIoFormatU32BE,

        /// <summary>
        /// 32-bit floating point (little endian),
        /// from -1.0 to 1.0
        /// </summary>
        SoundIoFormatFloat32LE,

        /// <summary>
        /// 32-bit floating point (big endian),
        /// from -1.0 to 1.0
        /// </summary>
        SoundIoFormatFloat32BE,

        /// <summary>
        /// 64-bit floating point (little endian),
        /// from -1.0 to 1.0
        /// </summary>
        SoundIoFormatFloat64LE,

        /// <summary>
        /// 64-bit floating point (big endian),
        /// from -1.0 to 1.0
        /// </summary>
        SoundIoFormatFloat64BE
    }
}
