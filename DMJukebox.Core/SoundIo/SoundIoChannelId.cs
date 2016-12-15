/* ========================================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * ====================================================================== */

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a C# implementation of the SoundIoChannelId enum in libsoundio.
    /// It describes the ID of a single audio output channel (speaker location).
    /// </summary>
    /// <remarks>
    /// This enum is defined in soundio.h of libsoundio.
    /// For more information, please see the documentation at
    /// http://libsound.io/doc-1.1.0/soundio_8h.html
    /// or the source code at https://github.com/andrewrk/libsoundio.
    /// </remarks>
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
