using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DMJukebox.Interop
{
    public static class MsvcrtInterop
    {
        private const string MsvcrtDll = "Msvcrt.dll";

        [DllImport(MsvcrtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int vsprintf(StringBuilder buffer, string format, IntPtr args);

        [DllImport(MsvcrtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int _vscprintf(string format, IntPtr ptr);

        [DllImport(MsvcrtDll, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr memcpy(IntPtr dest, IntPtr src, IntPtr count);
    }
}
