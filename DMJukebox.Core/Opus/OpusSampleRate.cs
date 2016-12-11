/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */
 
 namespace DMJukebox.Interop
{
    /// <summary>
    /// This describes the sample rate of input audio data to encode with Opus.
    /// </summary>
    /// <remarks>
    /// This enum doesn't exist in libopus, I created it as a helper to enforce the
    /// restriction on the Fs parameter of <see cref="OpusInterop.opus_encoder_create(
    /// OpusSampleRate, OpusChannelCount, OPUS_APPLICATION, out OpusErrorCode)"/>.
    /// </remarks>
    internal enum OpusSampleRate
    {
        /// <summary>
        /// 8 kHz
        /// </summary>
        _8000 = 8000,

        /// <summary>
        /// 12 kHz 
        /// </summary>
        _12000 = 12000,

        /// <summary>
        /// 16 kHz
        /// </summary>
        _16000 = 16000,

        /// <summary>
        /// 24 kHz
        /// </summary>
        _24000 = 24000,

        /// <summary>
        /// 48 kHz
        /// </summary>
        _48000 = 48000
    }
}
