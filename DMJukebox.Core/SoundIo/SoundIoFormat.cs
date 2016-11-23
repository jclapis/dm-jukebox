/* 
 * This file contains a C# implementation of the SoundIoFormat enum
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
    internal enum SoundIoFormat
    {
        SoundIoFormatInvalid,
        SoundIoFormatS8,
        SoundIoFormatU8,
        SoundIoFormatS16LE,
        SoundIoFormatS16BE,
        SoundIoFormatU16LE,
        SoundIoFormatU16BE,
        SoundIoFormatS24LE,
        SoundIoFormatS24BE,
        SoundIoFormatU24LE,
        SoundIoFormatU24BE,
        SoundIoFormatS32LE,
        SoundIoFormatS32BE,
        SoundIoFormatU32LE,
        SoundIoFormatU32BE,
        SoundIoFormatFloat32LE,
        SoundIoFormatFloat32BE,
        SoundIoFormatFloat64LE,
        SoundIoFormatFloat64BE
    }
}
