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
    /// This is a C# implementation of the SoundIoChannelLayout struct in libsoundio.
    /// It defines a physical configuration of audio channels, representing actual
    /// speaker placement.
    /// </summary>
    /// <remarks>
    /// This struct is defined in soundio.h of libsoundio.
    /// For more information, please see the documentation at
    /// http://libsound.io/doc-1.1.0/soundio_8h.html
    /// or the source code at https://github.com/andrewrk/libsoundio.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct SoundIoChannelLayout
    {
        /// <summary>
        /// The human-readable name of the channel layout
        /// </summary>
        public string name;

        /// <summary>
        /// The number of channels stored in <see cref="channels"/>
        /// </summary>
        public int channel_count;

        /// <summary>
        /// A collection of channels that this layout uses
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public SoundIoChannelId[] channels;
    }
}
