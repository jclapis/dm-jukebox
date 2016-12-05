using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DMJukebox.Interop
{
    internal static class SodiumInterop
    {
        [DllImport("libsodium.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int crypto_secretbox_easy(IntPtr c, IntPtr m, ulong mlen, IntPtr n, IntPtr k);
    }
}
