/* 
 * This file contains C# wrappers for some of the exported functions 
 * defined in soundio.h of the libsoundio project, for interop use.
 * 
 * All of the documentation and comments have been copied directly from
 * that header and are not my own work - they are the work of Andrew Kelley
 * and the other contributors to libsoundio. Credit goes to them.
 * 
 * For more information, please see the documentation at
 * http://libsound.io/doc-1.1.0/soundio_8h.html or the source code at
 * https://github.com/andrewrk/libsoundio.
 * 
 * Copyright (c) 2016 Joe Clapis.
 */

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
