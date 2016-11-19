/* 
 * This file contains a C# implementation of the SoundIoChannelLayoutId enum
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

namespace DMJukebox.Interop
{
    /// <summary>
    /// Built-in channel layouts for convenience.
    /// </summary>
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
