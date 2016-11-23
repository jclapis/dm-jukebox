/* 
 * This file contains a C# implementation of the SoundIoChannelId enum
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
    internal enum SoundIoChannelId
    {
        SoundIoChannelIdInvalid,
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
        SoundIoChannelIdTopBackRight,
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
        SoundIoChannelIdMsMid,
        SoundIoChannelIdMsSide,
        SoundIoChannelIdAmbisonicW,
        SoundIoChannelIdAmbisonicX,
        SoundIoChannelIdAmbisonicY,
        SoundIoChannelIdAmbisonicZ,
        SoundIoChannelIdXyX,
        SoundIoChannelIdXyY,
        SoundIoChannelIdHeadphonesLeft,
        SoundIoChannelIdHeadphonesRight,
        SoundIoChannelIdClickTrack,
        SoundIoChannelIdForeignLanguage,
        SoundIoChannelIdHearingImpaired,
        SoundIoChannelIdNarration,
        SoundIoChannelIdHaptic,
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
        SoundIoChannelIdAux15
    }
}
