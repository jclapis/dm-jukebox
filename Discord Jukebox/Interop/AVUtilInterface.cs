using System;
using System.Runtime.InteropServices;

namespace DiscordJukebox.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public delegate void LogCallback(IntPtr avcl, int level, string fmt, IntPtr args);

    internal static class AVUtilInterface
    {
        private const string AvUtilDll = "avutil-55.dll";

        [DllImport(AvUtilDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_log_set_callback(
            [MarshalAs(UnmanagedType.FunctionPtr)] LogCallback callback);

        [DllImport(AvUtilDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr av_frame_alloc();

        [DllImport(AvUtilDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void av_frame_free(ref IntPtr frame);
    }
}
