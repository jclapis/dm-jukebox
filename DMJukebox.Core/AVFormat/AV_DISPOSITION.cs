/* 
 * This file contains a C# implementation of the AV_DISPOSITION enum
 * as defined in avformat.h of the libavformat project, for interop use.
 * It isn't technically an enum in ffmpeg, just a bunch of macros.
 * 
 * The documentation and comments have been largely copied from those headers and
 * are not my own work - they are the work of the contributors to ffmpeg.
 * Credit goes to them. I may have modified them in places where it made sense
 * to help document the C# bindings.
 * 
 * For more information, please see the documentation at
 * https://www.ffmpeg.org/doxygen/trunk/index.html or the source code at
 * https://github.com/FFmpeg/FFmpeg.
 * 
 * Copyright (c) 2016 Joe Clapis.
 */

using System;

namespace DMJukebox.Interop
{
    [Flags]
    internal enum AV_DISPOSITION : int
    {
        AV_DISPOSITION_DEFAULT = 0x1,
        AV_DISPOSITION_DUB = 0x2,
        AV_DISPOSITION_ORIGINAL = 0x4,
        AV_DISPOSITION_COMMENT = 0x8,
        AV_DISPOSITION_LYRICS = 0x10,
        AV_DISPOSITION_KARAOKE = 0x20,

        /// <summary>
        /// Track should be used during playback by default.
        /// Useful for subtitle track that should be displayed
        /// even when user did not explicitly ask for subtitles.
        /// </summary>
        AV_DISPOSITION_FORCED = 0x40,

        /// <summary>
        /// stream for hearing impaired audiences
        /// </summary>
        AV_DISPOSITION_HEARING_IMPAIRED = 0x80,

        /// <summary>
        /// stream for visual impaired audiences
        /// </summary>
        AV_DISPOSITION_VISUAL_IMPAIRED = 0x100,

        /// <summary>
        /// stream without voice
        /// </summary>
        AV_DISPOSITION_CLEAN_EFFECTS = 0x200,

        /// <summary>
        /// The stream is stored in the file as an attached picture/"cover art" (e.g.
        /// APIC frame in ID3v2). The first (usually only) packet associated with it
        /// will be returned among the first few packets read from the file unless
        /// seeking takes place. It can also be accessed at any time in
        /// AVStream.attached_pic.
        /// </summary>
        AV_DISPOSITION_ATTACHED_PIC = 0x400,

        /// <summary>
        /// The stream is sparse, and contains thumbnail images, often corresponding
        /// to chapter markers. Only ever used with AV_DISPOSITION_ATTACHED_PIC.
        /// </summary>
        AV_DISPOSITION_TIMED_THUMBNAILS = 0x800,

        /// <summary>
        /// To specify text track kind (different from subtitles default).
        /// </summary>
        AV_DISPOSITION_CAPTIONS = 0x10000,

        AV_DISPOSITION_DESCRIPTIONS = 0x20000,
        AV_DISPOSITION_METADATA = 0x40000
    }
}
