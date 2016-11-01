using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DiscordJukebox.Interop
{
    internal static class Msvcrt
    {
        private const string Dll = "Msvcrt.dll";

        [DllImport(Dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int vsprintf(StringBuilder buffer, string format, IntPtr args);

        [DllImport(Dll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int _vscprintf(string format, IntPtr ptr);
    }
}
