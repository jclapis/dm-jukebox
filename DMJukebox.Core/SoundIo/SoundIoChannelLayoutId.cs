/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a C# implementation of the SoundIoChannelLayoutId enum in libsoundio.
    /// It represents the preset, built-in channel layouts.
    /// </summary>
    /// <remarks>
    /// This enum is defined in soundio.h of libsoundio.
    /// For more information, please see the documentation at
    /// http://libsound.io/doc-1.1.0/soundio_8h.html
    /// or the source code at https://github.com/andrewrk/libsoundio.
    /// </remarks>
    internal enum SoundIoChannelLayoutId
    {
        SoundIoChannelLayoutIdMono,
        SoundIoChannelLayoutIdStereo,
        SoundIoChannelLayoutId2Point1,
        SoundIoChannelLayoutId3Point0,
        SoundIoChannelLayoutId3Point0Back,
        SoundIoChannelLayoutId3Point1,
        SoundIoChannelLayoutId4Point0,
        SoundIoChannelLayoutIdQuad,
        SoundIoChannelLayoutIdQuadSide,
        SoundIoChannelLayoutId4Point1,
        SoundIoChannelLayoutId5Point0Back,
        SoundIoChannelLayoutId5Point0Side,
        SoundIoChannelLayoutId5Point1,
        SoundIoChannelLayoutId5Point1Back,
        SoundIoChannelLayoutId6Point0Side,
        SoundIoChannelLayoutId6Point0Front,
        SoundIoChannelLayoutIdHexagonal,
        SoundIoChannelLayoutId6Point1,
        SoundIoChannelLayoutId6Point1Back,
        SoundIoChannelLayoutId6Point1Front,
        SoundIoChannelLayoutId7Point0,
        SoundIoChannelLayoutId7Point0Front,
        SoundIoChannelLayoutId7Point1,
        SoundIoChannelLayoutId7Point1Wide,
        SoundIoChannelLayoutId7Point1WideBack,
        SoundIoChannelLayoutIdOctagonal
    }
}
