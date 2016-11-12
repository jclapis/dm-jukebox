using System;
using System.Runtime.InteropServices;

namespace DiscordJukebox.Interop
{
    internal static class LibSoundIoInterop
    {
        private const string SoundIoDll = "lib/soundio.dll";
        
        [DllImport(SoundIoDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr soundio_create();

        [DllImport(SoundIoDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void soundio_destroy(IntPtr soundio);

        [DllImport(SoundIoDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_connect(IntPtr soundio);
        
        [DllImport(SoundIoDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void soundio_flush_events(IntPtr soundio);

        [DllImport(SoundIoDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int soundio_output_device_count(IntPtr soundio);

        [DllImport(SoundIoDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr soundio_get_output_device(IntPtr soundio, int index);

        [DllImport(SoundIoDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int soundio_default_output_device_index(IntPtr soundio);

        [DllImport(SoundIoDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr soundio_outstream_create(IntPtr device);

        [DllImport(SoundIoDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_outstream_open(IntPtr outstream);

        [DllImport(SoundIoDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_outstream_start(IntPtr outstream);

        [DllImport(SoundIoDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void soundio_outstream_destroy(IntPtr outstream);

        [DllImport(SoundIoDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void soundio_device_unref(IntPtr device);

        [DllImport(SoundIoDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_outstream_begin_write(IntPtr outstream, IntPtr areas, IntPtr frame_count);

        [DllImport(SoundIoDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_outstream_end_write(IntPtr outstream);

        [DllImport(SoundIoDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_outstream_pause(IntPtr outstream, bool pause);
    }
}
