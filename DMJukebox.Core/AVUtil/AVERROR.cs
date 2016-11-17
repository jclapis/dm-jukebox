/* 
 * This file contains a C# enum implementation of the various AVERROR
 * constants defined in error.h of ffmpeg's libavutil project, for interop use.
 * 
 * The documentation and comments have been largely copied from that header and
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

namespace DMJukebox.Interop
{
    /// <summary>
    /// error handling
    /// </summary>
    public enum AVERROR
    {
        /// <summary>
        /// This isn't actually in the header, but since I made it an enum,
        /// I need to add this to handle successful return values.
        /// </summary>
        AVERROR_SUCCESS = 0,

        AVERROR_EAGAIN = -11,

        AVERROR_ENOMEM = -12,

        AVERROR_EINVAL = -22,

        /// <summary>
        /// Bitstream filter not found
        /// </summary>
        AVERROR_BSF_NOT_FOUND = -0x465342F8,

        /// <summary>
        /// Internal bug, also see AVERROR_BUG2
        /// </summary>
        AVERROR_BUG = -0x21475542,

        /// <summary>
        /// Buffer too small
        /// </summary>
        AVERROR_BUFFER_TOO_SMALL = -0x53465542,

        /// <summary>
        /// Decoder not found
        /// </summary>
        AVERROR_DECODER_NOT_FOUND = -0x434544F8,

        /// <summary>
        /// Demuxer not found
        /// </summary>
        AVERROR_DEMUXER_NOT_FOUND = -0x4D4544F8,

        /// <summary>
        /// Encoder not found
        /// </summary>
        AVERROR_ENCODER_NOT_FOUND = -0x434E45F8,

        /// <summary>
        /// End of file
        /// </summary>
        AVERROR_EOF = -0x20464F45,

        /// <summary>
        /// Immediate exit was requested; the called function should not be restarted
        /// </summary>
        AVERROR_EXIT = -0x54495845,

        /// <summary>
        /// Generic error in an external library
        /// </summary>
        AVERROR_EXTERNAL = -0x20545845,

        /// <summary>
        /// Filter not found
        /// </summary>
        AVERROR_FILTER_NOT_FOUND = -0x4C4946F8,

        /// <summary>
        /// Invalid data found when processing input
        /// </summary>
        AVERROR_INVALIDDATA = -0x41444E49,

        /// <summary>
        /// Muxer not found
        /// </summary>
        AVERROR_MUXER_NOT_FOUND = -0x58554DF8,

        /// <summary>
        /// Option not found
        /// </summary>
        AVERROR_OPTION_NOT_FOUND = -0x54504FF8,

        /// <summary>
        /// Not yet implemented in FFmpeg, patches welcome
        /// </summary>
        AVERROR_PATCHWELCOME = -0x45574150,

        /// <summary>
        /// Protocol not found
        /// </summary>
        AVERROR_PROTOCOL_NOT_FOUND = -0x4F5250F8,

        /// <summary>
        /// Stream not found
        /// </summary>
        AVERROR_STREAM_NOT_FOUND = -0x525453F8,

        /// <summary>
        /// This is semantically identical to AVERROR_BUG
        /// it has been introduced in Libav after our AVERROR_BUG and with a modified value.
        /// </summary>
        AVERROR_BUG2 = -0x20475542,

        /// <summary>
        /// Unknown error, typically from an external library
        /// </summary>
        AVERROR_UNKNOWN = -0x4E4B4E55,

        /// <summary>
        /// Requested feature is flagged experimental. Set strict_std_compliance if you really want to use it.
        /// </summary>
        AVERROR_EXPERIMENTAL = -0x2BB2AFA8,

        /// <summary>
        /// Input changed between calls. Reconfiguration is required.
        /// (can be OR-ed with AVERROR_OUTPUT_CHANGED)
        /// </summary>
        AVERROR_INPUT_CHANGED = -0x636E6701,

        /// <summary>
        /// Output changed between calls. Reconfiguration is required. (can be OR-ed with AVERROR_INPUT_CHANGED)
        /// </summary>
        AVERROR_OUTPUT_CHANGED = -0x636E6702,
        
        AVERROR_HTTP_BAD_REQUEST = -0x303034F8,
        AVERROR_HTTP_UNAUTHORIZED = -0x313034F8,
        AVERROR_HTTP_FORBIDDEN = -0x333034F8,
        AVERROR_HTTP_NOT_FOUND = -0x343034F8,
        AVERROR_HTTP_OTHER_4XX = -0x585834F8,
        AVERROR_HTTP_SERVER_ERROR = -0x585835F8
    }
}
