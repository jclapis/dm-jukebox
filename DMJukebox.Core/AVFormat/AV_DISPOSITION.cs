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

using System;

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a C# implementation of the AV_DISPOSITION enum in FFmpeg.
    /// I think this is just used to describe the purpose, or usage of
    /// a media stream. It shows up in <see cref="AVStream.disposition"/>. 
    /// </summary>
    /// <remarks>
    /// This enum is defined in avformat.h of the libavformat project.
    /// It isn't technically an enum in FFmpeg, just a bunch of macros.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    [Flags]
    internal enum AV_DISPOSITION : int
    {
        AV_DISPOSITION_DEFAULT = 0x1,
        AV_DISPOSITION_DUB = 0x2,
        AV_DISPOSITION_ORIGINAL = 0x4,
        AV_DISPOSITION_COMMENT = 0x8,
        AV_DISPOSITION_LYRICS = 0x10,
        AV_DISPOSITION_KARAOKE = 0x20,
        AV_DISPOSITION_FORCED = 0x40,
        AV_DISPOSITION_HEARING_IMPAIRED = 0x80,
        AV_DISPOSITION_VISUAL_IMPAIRED = 0x100,
        AV_DISPOSITION_CLEAN_EFFECTS = 0x200,
        AV_DISPOSITION_ATTACHED_PIC = 0x400,
        AV_DISPOSITION_TIMED_THUMBNAILS = 0x800,
        AV_DISPOSITION_CAPTIONS = 0x10000,
        AV_DISPOSITION_DESCRIPTIONS = 0x20000,
        AV_DISPOSITION_METADATA = 0x40000
    }
}
