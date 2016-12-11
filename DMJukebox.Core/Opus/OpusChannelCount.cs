/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */
 
 namespace DMJukebox.Interop
{
    /// <summary>
    /// This describes the number of channels in the input audio data to encode with Opus.
    /// </summary>
    /// <remarks>
    /// This enum doesn't exist in libopus, I created it as a helper to enforce the
    /// restriction on the channels parameter of <see cref="OpusInterop.opus_encoder_create(
    /// OpusSampleRate, OpusChannelCount, OPUS_APPLICATION, out OpusErrorCode)"/> 
    /// which must be 1 or 2.
    /// </remarks>
    internal enum OpusChannelCount
    {
        /// <summary>
        /// 1 channel (mono)
        /// </summary>
        _1 = 1,

        /// <summary>
        /// 2 channels (stereo)
        /// </summary>
        _2 = 2
    }
}
