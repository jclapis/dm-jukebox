using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DiscordJukebox.Interop
{
    internal static class MsvcrtInterop
    {
        private const string MsvcrtDll = "Msvcrt.dll";

        [DllImport(MsvcrtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int vsprintf(StringBuilder buffer, string format, IntPtr args);

        [DllImport(MsvcrtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int _vscprintf(string format, IntPtr ptr);
    }
}
