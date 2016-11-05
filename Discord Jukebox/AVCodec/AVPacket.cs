using System;
using System.Runtime.InteropServices;

namespace DiscordJukebox.Interop
{
    /// <summary>
    /// This structure stores compressed data. It is typically exported by demuxers
    /// and then passed as input to decoders, or received as output from encoders and
    /// then passed to muxers.
    ///
    /// For video, it should typically contain one compressed frame. For audio it may
    /// contain several compressed frames. Encoders are allowed to output empty
    /// packets, with no compressed data, containing only side data
    /// (e.g. to update some stream parameters at the end of encoding).
    ///
    /// AVPacket is one of the few structs in FFmpeg, whose size is a part of public
    /// ABI. Thus it may be allocated on stack and no new fields can be added to it
    /// without libavcodec and libavformat major bump.
    ///
    /// The semantics of data ownership depends on the buf field.
    /// If it is set, the packet data is dynamically allocated and is
    /// valid indefinitely until a call to av_packet_unref() reduces the
    /// reference count to 0.
    ///
    /// If the buf field is not set av_packet_ref() would make a copy instead
    /// of increasing the reference count.
    ///
    /// The side data is always allocated with av_malloc(), copied by
    /// av_packet_ref() and freed by av_packet_unref().
    ///
    /// @see av_packet_ref
    /// @see av_packet_unref
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct AVPacket
    {
        /// <summary>
        /// A reference to the reference-counted buffer where the packet data is
        /// stored.
        /// May be NULL, then the packet data is not reference-counted.
        /// </summary>
        public IntPtr buf;

        /// <summary>
        /// Presentation timestamp in AVStream->time_base units; the time at which
        /// the decompressed packet will be presented to the user.
        /// Can be AV_NOPTS_VALUE if it is not stored in the file.
        /// pts MUST be larger or equal to dts as presentation cannot happen before
        /// decompression, unless one wants to view hex dumps.Some formats misuse
        /// the terms dts and pts/cts to mean something different.Such timestamps
        /// must be converted to true pts/dts before they are stored in AVPacket.
        /// </summary>
        public long pts;

        /// <summary>
        /// Decompression timestamp in AVStream->time_base units; the time at which
        /// the packet is decompressed.
        /// Can be AV_NOPTS_VALUE if it is not stored in the file.
        /// </summary>
        public long dts;

        public IntPtr data;

        public int size;

        public int stream_index;

        /// <summary>
        /// A combination of AV_PKT_FLAG values
        /// </summary>
        public AV_PKT_FLAG flags;

        /// <summary>
        /// Additional packet data that can be provided by the container.
        /// Packet can contain several types of side information.
        /// </summary>
        public IntPtr side_data;

        public int side_data_elems;

        /// <summary>
        /// Duration of this packet in AVStream->time_base units, 0 if unknown.
        /// Equals next_pts - this_pts in presentation order.
        /// </summary>
        public long duration;

        /// <summary>
        /// byte position in stream, -1 if unknown
        /// </summary>
        public long pos;

        /// <summary>
        /// @deprecated Same as the duration field, but as int64_t. This was required
        /// for Matroska subtitles, whose duration values could overflow when the
        /// duration field was still an int.
        /// </summary>
        public long convergence_duration;
    }
}
