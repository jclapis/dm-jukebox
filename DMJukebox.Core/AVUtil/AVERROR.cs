/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a C# implementation of the AVERROR enum in FFmpeg.
    /// It defines error codes that some of the FFmpeg functions return.
    /// </summary>
    /// <remarks>
    /// This enum is defined in error.h of ffmpeg's libavutil project.
    /// It isn't technically an enum in FFmpeg, just a bunch of macros.
    /// For more information, please see the documentation at
    /// https://www.ffmpeg.org/doxygen/trunk/index.html
    /// or the source code at https://github.com/FFmpeg/FFmpeg.
    /// </remarks>
    public enum AVERROR
    {
        AVERROR_SUCCESS = 0,
        AVERROR_EAGAIN = -11,
        AVERROR_ENOMEM = -12,
        AVERROR_EINVAL = -22,
        AVERROR_BSF_NOT_FOUND = -0x465342F8,
        AVERROR_BUG = -0x21475542,
        AVERROR_BUFFER_TOO_SMALL = -0x53465542,
        AVERROR_DECODER_NOT_FOUND = -0x434544F8,
        AVERROR_DEMUXER_NOT_FOUND = -0x4D4544F8,
        AVERROR_ENCODER_NOT_FOUND = -0x434E45F8,
        AVERROR_EOF = -0x20464F45,
        AVERROR_EXIT = -0x54495845,
        AVERROR_EXTERNAL = -0x20545845,
        AVERROR_FILTER_NOT_FOUND = -0x4C4946F8,
        AVERROR_INVALIDDATA = -0x41444E49,
        AVERROR_MUXER_NOT_FOUND = -0x58554DF8,
        AVERROR_OPTION_NOT_FOUND = -0x54504FF8,
        AVERROR_PATCHWELCOME = -0x45574150,
        AVERROR_PROTOCOL_NOT_FOUND = -0x4F5250F8,
        AVERROR_STREAM_NOT_FOUND = -0x525453F8,
        AVERROR_BUG2 = -0x20475542,
        AVERROR_UNKNOWN = -0x4E4B4E55,
        AVERROR_EXPERIMENTAL = -0x2BB2AFA8,
        AVERROR_INPUT_CHANGED = -0x636E6701,
        AVERROR_OUTPUT_CHANGED = -0x636E6702,
        AVERROR_HTTP_BAD_REQUEST = -0x303034F8,
        AVERROR_HTTP_UNAUTHORIZED = -0x313034F8,
        AVERROR_HTTP_FORBIDDEN = -0x333034F8,
        AVERROR_HTTP_NOT_FOUND = -0x343034F8,
        AVERROR_HTTP_OTHER_4XX = -0x585834F8,
        AVERROR_HTTP_SERVER_ERROR = -0x585835F8
    }
}
