using System.Runtime.InteropServices;

namespace DiscordJukebox.Interop
{
    /// <summary>
    /// rational number numerator/denominator
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct AVRational
    {
        /// <summary>
        /// numerator
        /// </summary>
        public int num;

        /// <summary>
        /// denominator
        /// </summary>
        public int den;
    }
}
