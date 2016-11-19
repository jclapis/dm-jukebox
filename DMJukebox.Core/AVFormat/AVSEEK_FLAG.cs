using System;

namespace DMJukebox.Interop
{
    [Flags]
    internal enum AVSEEK_FLAG
    {
        /// <summary>
        /// seek backward
        /// </summary>
        AVSEEK_FLAG_BACKWARD = 1,

        /// <summary>
        /// seeking based on position in bytes
        /// </summary>
        AVSEEK_FLAG_BYTE = 2,

        /// <summary>
        /// seek to any frame, even non-keyframes
        /// </summary>
        AVSEEK_FLAG_ANY = 4,

        /// <summary>
        /// seeking based on frame number
        /// </summary>
        AVSEEK_FLAG_FRAME = 8
    }
}
