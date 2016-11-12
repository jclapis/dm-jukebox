/* 
 * This file contains a C# implementation of the SoundIoChannelArea struct
 * as defined in soundio.h of the libsoundio project, for interop use.
 * 
 * All of the documentation and comments have been copied directly from
 * that header and are not my own work - they are the work of Andrew Kelley
 * and the other contributors to libsoundio. Credit goes to them.
 * 
 * For more information, please see the documentation at
 * http://libsound.io/doc-1.1.0/soundio_8h.html or the source code at
 * https://github.com/andrewrk/libsoundio.
 * 
 * Copyright (c) 2016 Joe Clapis.
 */

using System;
using System.Runtime.InteropServices;

namespace DiscordJukebox.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct SoundIoChannelArea
    {
        /// <summary>
        /// Base address of buffer.
        /// </summary>
        public IntPtr ptr;

        /// <summary>
        /// How many bytes it takes to get from the beginning of one sample to
        /// the beginning of the next sample.
        /// </summary>
        public int step;
    }
}
