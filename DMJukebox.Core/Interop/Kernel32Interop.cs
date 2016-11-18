﻿using System;
using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    internal static class Kernel32Interop
    {
        private const string Kernel32Dll = "kernel32.dll";

        [DllImport(Kernel32Dll, SetLastError = true)]
        public static extern int SetStdHandle(int device, IntPtr handle);
    }
}