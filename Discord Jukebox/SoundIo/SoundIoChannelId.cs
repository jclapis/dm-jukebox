/* 
 * This file contains a C# implementation of the SoundIoChannelId enum
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

namespace DiscordJukebox.Interop
{
    /// <summary>
    /// Specifies where a channel is physically located.
    /// </summary>
    internal enum SoundIoChannelId
    {
        SoundIoChannelIdInvalid,

        /// <summary>
        /// First of the more commonly supported ids.
        /// </summary>
        SoundIoChannelIdFrontLeft,
        SoundIoChannelIdFrontRight,
        SoundIoChannelIdFrontCenter,
        SoundIoChannelIdLfe,
        SoundIoChannelIdBackLeft,
        SoundIoChannelIdBackRight,
        SoundIoChannelIdFrontLeftCenter,
        SoundIoChannelIdFrontRightCenter,
        SoundIoChannelIdBackCenter,
        SoundIoChannelIdSideLeft,
        SoundIoChannelIdSideRight,
        SoundIoChannelIdTopCenter,
        SoundIoChannelIdTopFrontLeft,
        SoundIoChannelIdTopFrontCenter,
        SoundIoChannelIdTopFrontRight,
        SoundIoChannelIdTopBackLeft,
        SoundIoChannelIdTopBackCenter,

        /// <summary>
        /// Last of the more commonly supported ids.
        /// </summary>
        SoundIoChannelIdTopBackRight,

        /// <summary>
        /// First of the less commonly supported ids.
        /// </summary>
        SoundIoChannelIdBackLeftCenter,
        SoundIoChannelIdBackRightCenter,
        SoundIoChannelIdFrontLeftWide,
        SoundIoChannelIdFrontRightWide,
        SoundIoChannelIdFrontLeftHigh,
        SoundIoChannelIdFrontCenterHigh,
        SoundIoChannelIdFrontRightHigh,
        SoundIoChannelIdTopFrontLeftCenter,
        SoundIoChannelIdTopFrontRightCenter,
        SoundIoChannelIdTopSideLeft,
        SoundIoChannelIdTopSideRight,
        SoundIoChannelIdLeftLfe,
        SoundIoChannelIdRightLfe,
        SoundIoChannelIdLfe2,
        SoundIoChannelIdBottomCenter,
        SoundIoChannelIdBottomLeftCenter,
        SoundIoChannelIdBottomRightCenter,

        /// <summary>
        /// Mid/side recording
        /// </summary>
        SoundIoChannelIdMsMid,
        SoundIoChannelIdMsSide,

        /// <summary>
        /// first order ambisonic channels
        /// </summary>
        SoundIoChannelIdAmbisonicW,
        SoundIoChannelIdAmbisonicX,
        SoundIoChannelIdAmbisonicY,
        SoundIoChannelIdAmbisonicZ,

        /// <summary>
        /// X-Y Recording
        /// </summary>
        SoundIoChannelIdXyX,
        SoundIoChannelIdXyY,

        /// <summary>
        /// First of the "other" channel ids
        /// </summary>
        SoundIoChannelIdHeadphonesLeft,
        SoundIoChannelIdHeadphonesRight,
        SoundIoChannelIdClickTrack,
        SoundIoChannelIdForeignLanguage,
        SoundIoChannelIdHearingImpaired,
        SoundIoChannelIdNarration,
        SoundIoChannelIdHaptic,

        /// <summary>
        /// Last of the "other" channel ids
        /// </summary>
        SoundIoChannelIdDialogCentricMix,

        SoundIoChannelIdAux,
        SoundIoChannelIdAux0,
        SoundIoChannelIdAux1,
        SoundIoChannelIdAux2,
        SoundIoChannelIdAux3,
        SoundIoChannelIdAux4,
        SoundIoChannelIdAux5,
        SoundIoChannelIdAux6,
        SoundIoChannelIdAux7,
        SoundIoChannelIdAux8,
        SoundIoChannelIdAux9,
        SoundIoChannelIdAux10,
        SoundIoChannelIdAux11,
        SoundIoChannelIdAux12,
        SoundIoChannelIdAux13,
        SoundIoChannelIdAux14,
        SoundIoChannelIdAux15,
    }
}
