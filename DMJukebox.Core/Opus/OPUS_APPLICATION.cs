/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a C# implementation of the OPUS_APPLICATION enum in Opus.
    /// It defines the kind of audio that you plan to encode, which lets the
    /// encoder select the best algorithm for encoding it.
    /// </summary>
    /// <remarks>
    /// This enum is defiend in opus_defines.h of the libopus project.
    /// It isn't technically an enum in Opus, just a bunch of macros.
    /// For more information, please see the documentation at 
    /// https://opus-codec.org/docs/opus_api-1.1.3/index.html
    /// or the source code at https://git.xiph.org/?p=opus.git.
    /// </remarks>
    internal enum OPUS_APPLICATION
    {
        /// <summary>
        /// This is meant for voice / teleconference audio. The encoder will
        /// prioritize overall listening quality so you can distinguish
        /// easily between different words and syllables, even though they
        /// may not be a perfect reproduction of the incoming sound.
        /// </summary>
        OPUS_APPLICATION_VOIP = 2048,

        /// <summary>
        /// This is meant for music / broadcast audio, where the resulting
        /// sound should be as close as possible to the original input.
        /// </summary>
        OPUS_APPLICATION_AUDIO = 2049,

        /// <summary>
        /// This is a special mode for time-critical applications where
        /// the lowest possible encoding latency is required, so most of
        /// the optimizations get thrown out the window.
        /// </summary>
        OPUS_APPLICATION_RESTRICTED_LOWDELAY = 2051
    }
}
