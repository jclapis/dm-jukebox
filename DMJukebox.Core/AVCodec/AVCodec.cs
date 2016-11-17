using System;
using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    /// <summary>
    /// AVCodec.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct AVCodec
    {
        /// <summary>
        /// Name of the codec implementation.
        /// The name is globally unique among encoders and among decoders (but an
        /// encoder and a decoder can share the same name).
        /// This is the primary way to find a codec from the user perspective.
        /// </summary>
        public string name;

        /// <summary>
        /// Descriptive name for the codec, meant to be more human readable than name.
        /// You should use the NULL_IF_CONFIG_SMALL() macro to define it.
        /// </summary>
        public string long_name;

        public AVMediaType type;

        public AVCodecID id;

        /// <summary>
        /// Codec capabilities.
        /// see AV_CODEC_CAP_*
        /// </summary>
        public int capabilities;

        /// <summary>
        /// array of supported framerates, or NULL if any, array is terminated by {0,0}
        /// </summary>
        public IntPtr supported_framerates;

        /// <summary>
        /// array of supported pixel formats, or NULL if unknown, array is terminated by -1
        /// </summary>
        public IntPtr pix_fmts;

        /// <summary>
        /// array of supported audio samplerates, or NULL if unknown, array is terminated by 0
        /// </summary>
        public IntPtr supported_samplerates;

        /// <summary>
        /// array of supported sample formats, or NULL if unknown, array is terminated by -1
        /// </summary>
        public IntPtr sample_fmts;

        /// <summary>
        /// array of support channel layouts, or NULL if unknown. array is terminated by 0
        /// </summary>
        public IntPtr channel_layouts;

        /// <summary>
        /// maximum value for lowres supported by the decoder, no direct access, use av_codec_get_max_lowres()
        /// </summary>
        public byte max_lowres;

        /// <summary>
        /// AVClass for the private context
        /// </summary>
        public IntPtr priv_class;

        /// <summary>
        /// array of recognized profiles, or NULL if unknown, array is terminated by {FF_PROFILE_UNKNOWN}
        /// </summary>
        public IntPtr profiles;
    }

}
