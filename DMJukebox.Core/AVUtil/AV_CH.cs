/* 
 * This file contains a C# implementation of the AV_CH enum
 * as defined in channel_layout.h of the libavutil project, for interop use.
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
    /// <summary>
    /// Audio channel masks
    ///
    /// A channel layout is a 64-bits integer with a bit set for every channel.
    /// The number of bits set must be equal to the number of channels.
    /// The value 0 means that the channel layout is not known.
    /// @note this data structure is not powerful enough to handle channels
    /// combinations that have the same channel multiple times, such as
    /// dual-mono.
    /// </summary>
    [Flags]
    internal enum AV_CH : ulong
    {
        AV_CH_FRONT_LEFT = 0x00000001,
        AV_CH_FRONT_RIGHT = 0x00000002,
        AV_CH_FRONT_CENTER = 0x00000004,
        AV_CH_LOW_FREQUENCY = 0x00000008,
        AV_CH_BACK_LEFT = 0x00000010,
        AV_CH_BACK_RIGHT = 0x00000020,
        AV_CH_FRONT_LEFT_OF_CENTER = 0x00000040,
        AV_CH_FRONT_RIGHT_OF_CENTER = 0x00000080,
        AV_CH_BACK_CENTER = 0x00000100,
        AV_CH_SIDE_LEFT = 0x00000200,
        AV_CH_SIDE_RIGHT = 0x00000400,
        AV_CH_TOP_CENTER = 0x00000800,
        AV_CH_TOP_FRONT_LEFT = 0x00001000,
        AV_CH_TOP_FRONT_CENTER = 0x00002000,
        AV_CH_TOP_FRONT_RIGHT = 0x00004000,
        AV_CH_TOP_BACK_LEFT = 0x00008000,
        AV_CH_TOP_BACK_CENTER = 0x00010000,
        AV_CH_TOP_BACK_RIGHT = 0x00020000,

        /// <summary>
        /// Stereo downmix.
        /// </summary>
        AV_CH_STEREO_LEFT = 0x20000000,

        /// <summary>
        /// Stereo downmix.
        /// </summary>
        AV_CH_STEREO_RIGHT = 0x40000000,

        AV_CH_WIDE_LEFT = 0x0000000080000000,
        AV_CH_WIDE_RIGHT = 0x0000000100000000,
        AV_CH_SURROUND_DIRECT_LEFT = 0x0000000200000000,
        AV_CH_SURROUND_DIRECT_RIGHT = 0x0000000400000000,
        AV_CH_LOW_FREQUENCY_2 = 0x0000000800000000,

        /// <summary>
        /// Channel mask value used for AVCodecContext.request_channel_layout
        /// to indicate that the user requests the channel order of the decoder output
        /// to be the native codec channel order.
        /// </summary>
        AV_CH_LAYOUT_NATIVE = 0x8000000000000000
    }
}
