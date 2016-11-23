/* 
 * This file contains a C# implementation of the SoundIoChannelLayout struct
 * as defined in soundio.h of the libsoundio project, for interop use.
 * 
 * For more information, please see the documentation at
 * http://libsound.io/doc-1.1.0/soundio_8h.html or the source code at
 * https://github.com/andrewrk/libsoundio.
 * 
 * Copyright (c) 2016 Joe Clapis.
 */

using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct SoundIoChannelLayout
    {
        public string name;

        public int channel_count;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public SoundIoChannelId[] channels;
    }
}
