using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    /// <summary>
    /// fractional numbers for exact pts handling
    /// The exact value of the fractional number is: 'val + num / den'.
    /// num is assumed to be 0 <= num<den.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct AVFrac
    {
        public long val;

        public long num;

        public long den;
    }
}
