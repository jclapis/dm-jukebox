namespace DiscordJukebox.SoundIo
{
    internal enum SoundIoFormat
    {
        SoundIoFormatInvalid,

        /// <summary>
        /// Signed 8 bit
        /// </summary>
        SoundIoFormatS8,

        /// <summary>
        /// Unsigned 8 bit
        /// </summary>
        SoundIoFormatU8,

        /// <summary>
        /// Signed 16 bit Little Endian
        /// </summary>
        SoundIoFormatS16LE,

        /// <summary>
        /// Signed 16 bit Big Endian
        /// </summary>
        SoundIoFormatS16BE,

        /// <summary>
        /// Unsigned 16 bit Little Endian
        /// </summary>
        SoundIoFormatU16LE,

        /// <summary>
        /// Unsigned 16 bit Little Endian
        /// </summary>
        SoundIoFormatU16BE,

        /// <summary>
        /// Signed 24 bit Little Endian using low three bytes in 32-bit word
        /// </summary>
        SoundIoFormatS24LE,

        /// <summary>
        /// Signed 24 bit Big Endian using low three bytes in 32-bit word
        /// </summary>
        SoundIoFormatS24BE,

        /// <summary>
        /// Unsigned 24 bit Little Endian using low three bytes in 32-bit word
        /// </summary>
        SoundIoFormatU24LE,

        /// <summary>
        /// Unsigned 24 bit Big Endian using low three bytes in 32-bit word
        /// </summary>
        SoundIoFormatU24BE,

        /// <summary>
        /// Signed 32 bit Little Endian
        /// </summary>
        SoundIoFormatS32LE,

        /// <summary>
        /// Signed 32 bit Big Endian
        /// </summary>
        SoundIoFormatS32BE,

        /// <summary>
        /// Unsigned 32 bit Little Endian
        /// </summary>
        SoundIoFormatU32LE,

        /// <summary>
        /// Unsigned 32 bit Big Endian
        /// </summary>
        SoundIoFormatU32BE,

        /// <summary>
        /// Float 32 bit Little Endian, Range -1.0 to 1.0
        /// </summary>
        SoundIoFormatFloat32LE,

        /// <summary>
        /// Float 32 bit Big Endian, Range -1.0 to 1.0
        /// </summary>
        SoundIoFormatFloat32BE,

        /// <summary>
        /// Float 64 bit Little Endian, Range -1.0 to 1.0
        /// </summary>
        SoundIoFormatFloat64LE,

        /// <summary>
        /// Float 64 bit Big Endian, Range -1.0 to 1.0
        /// </summary>
        SoundIoFormatFloat64BE
    }
}
