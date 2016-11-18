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

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a utility class that holds the P/Invoke wrappers for libsoundio.
    /// </summary>
    internal static class SoundIoInterop
    {
        /// <summary>
        /// The DLL for Windows
        /// </summary>
        private const string WindowsSoundIoLibrary = "soundio.dll";

        /// <summary>
        /// The SO for Linux
        /// </summary>
        private const string LinuxSoundIoLibrary = "soundio.so";

        /// <summary>
        /// The Dylib for OSX
        /// </summary>
        private const string MacSoundIoLibrary = "soundio.dylib";

        // These regions contain the DllImport function definitions for each OS. Since we can't really set
        // the path of DllImport dynamically (and loading them dynamically using LoadLibrary / dlopen is complicated
        // to manage cross-platform), we have to pre-define them based on the names of the libraries above.

        #region Windows Function

        [DllImport(WindowsSoundIoLibrary, EntryPoint = nameof(soundio_create), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr soundio_create_windows();

        [DllImport(WindowsSoundIoLibrary, EntryPoint = nameof(soundio_destroy), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void soundio_destroy_windows(IntPtr soundio);

        [DllImport(WindowsSoundIoLibrary, EntryPoint = nameof(soundio_connect), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_connect_windows(IntPtr soundio);

        [DllImport(WindowsSoundIoLibrary, EntryPoint = nameof(soundio_flush_events), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void soundio_flush_events_windows(IntPtr soundio);

        [DllImport(WindowsSoundIoLibrary, EntryPoint = nameof(soundio_output_device_count), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int soundio_output_device_count_windows(IntPtr soundio);

        [DllImport(WindowsSoundIoLibrary, EntryPoint = nameof(soundio_get_output_device), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr soundio_get_output_device_windows(IntPtr soundio, int index);

        [DllImport(WindowsSoundIoLibrary, EntryPoint = nameof(soundio_default_output_device_index), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int soundio_default_output_device_index_windows(IntPtr soundio);

        [DllImport(WindowsSoundIoLibrary, EntryPoint = nameof(soundio_outstream_create), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr soundio_outstream_create_windows(IntPtr device);

        [DllImport(WindowsSoundIoLibrary, EntryPoint = nameof(soundio_outstream_open), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_outstream_open_windows(IntPtr outstream);

        [DllImport(WindowsSoundIoLibrary, EntryPoint = nameof(soundio_outstream_start), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_outstream_start_windows(IntPtr outstream);

        [DllImport(WindowsSoundIoLibrary, EntryPoint = nameof(soundio_outstream_destroy), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void soundio_outstream_destroy_windows(IntPtr outstream);

        [DllImport(WindowsSoundIoLibrary, EntryPoint = nameof(soundio_device_unref), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void soundio_device_unref_windows(IntPtr device);

        [DllImport(WindowsSoundIoLibrary, EntryPoint = nameof(soundio_outstream_begin_write), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_outstream_begin_write_windows(IntPtr outstream, ref IntPtr areas, ref int frame_count);

        [DllImport(WindowsSoundIoLibrary, EntryPoint = nameof(soundio_outstream_end_write), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_outstream_end_write_windows(IntPtr outstream);

        [DllImport(WindowsSoundIoLibrary, EntryPoint = nameof(soundio_outstream_pause), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_outstream_pause_windows(IntPtr outstream, bool pause);

        [DllImport(WindowsSoundIoLibrary, EntryPoint = nameof(soundio_channel_layout_get_builtin), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr soundio_channel_layout_get_builtin_windows(SoundIoChannelLayoutId index);

        [DllImport(WindowsSoundIoLibrary, EntryPoint = nameof(soundio_device_supports_layout), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern bool soundio_device_supports_layout_windows(IntPtr device, IntPtr layout);

        [DllImport(WindowsSoundIoLibrary, EntryPoint = nameof(soundio_channel_layout_find_channel), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int soundio_channel_layout_find_channel_windows(IntPtr layout, SoundIoChannelId channel);

        [DllImport(WindowsSoundIoLibrary, EntryPoint = nameof(soundio_channel_layout_get_default), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr soundio_channel_layout_get_default_windows(int channel_count);

        #endregion

        #region Linux Functions

        [DllImport(LinuxSoundIoLibrary, EntryPoint = nameof(soundio_create), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr soundio_create_linux();

        [DllImport(LinuxSoundIoLibrary, EntryPoint = nameof(soundio_destroy), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void soundio_destroy_linux(IntPtr soundio);

        [DllImport(LinuxSoundIoLibrary, EntryPoint = nameof(soundio_connect), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_connect_linux(IntPtr soundio);

        [DllImport(LinuxSoundIoLibrary, EntryPoint = nameof(soundio_flush_events), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void soundio_flush_events_linux(IntPtr soundio);

        [DllImport(LinuxSoundIoLibrary, EntryPoint = nameof(soundio_output_device_count), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int soundio_output_device_count_linux(IntPtr soundio);

        [DllImport(LinuxSoundIoLibrary, EntryPoint = nameof(soundio_get_output_device), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr soundio_get_output_device_linux(IntPtr soundio, int index);

        [DllImport(LinuxSoundIoLibrary, EntryPoint = nameof(soundio_default_output_device_index), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int soundio_default_output_device_index_linux(IntPtr soundio);

        [DllImport(LinuxSoundIoLibrary, EntryPoint = nameof(soundio_outstream_create), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr soundio_outstream_create_linux(IntPtr device);

        [DllImport(LinuxSoundIoLibrary, EntryPoint = nameof(soundio_outstream_open), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_outstream_open_linux(IntPtr outstream);

        [DllImport(LinuxSoundIoLibrary, EntryPoint = nameof(soundio_outstream_start), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_outstream_start_linux(IntPtr outstream);

        [DllImport(LinuxSoundIoLibrary, EntryPoint = nameof(soundio_outstream_destroy), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void soundio_outstream_destroy_linux(IntPtr outstream);

        [DllImport(LinuxSoundIoLibrary, EntryPoint = nameof(soundio_device_unref), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void soundio_device_unref_linux(IntPtr device);

        [DllImport(LinuxSoundIoLibrary, EntryPoint = nameof(soundio_outstream_begin_write), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_outstream_begin_write_linux(IntPtr outstream, ref IntPtr areas, ref int frame_count);

        [DllImport(LinuxSoundIoLibrary, EntryPoint = nameof(soundio_outstream_end_write), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_outstream_end_write_linux(IntPtr outstream);

        [DllImport(LinuxSoundIoLibrary, EntryPoint = nameof(soundio_outstream_pause), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_outstream_pause_linux(IntPtr outstream, bool pause);

        [DllImport(LinuxSoundIoLibrary, EntryPoint = nameof(soundio_channel_layout_get_builtin), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr soundio_channel_layout_get_builtin_linux(SoundIoChannelLayoutId index);

        [DllImport(LinuxSoundIoLibrary, EntryPoint = nameof(soundio_device_supports_layout), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern bool soundio_device_supports_layout_linux(IntPtr device, IntPtr layout);

        [DllImport(LinuxSoundIoLibrary, EntryPoint = nameof(soundio_channel_layout_find_channel), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int soundio_channel_layout_find_channel_linux(IntPtr layout, SoundIoChannelId channel);

        [DllImport(LinuxSoundIoLibrary, EntryPoint = nameof(soundio_channel_layout_get_default), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr soundio_channel_layout_get_default_linux(int channel_count);

        #endregion

        #region OSX Functions

        [DllImport(MacSoundIoLibrary, EntryPoint = nameof(soundio_create), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr soundio_create_osx();

        [DllImport(MacSoundIoLibrary, EntryPoint = nameof(soundio_destroy), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void soundio_destroy_osx(IntPtr soundio);

        [DllImport(MacSoundIoLibrary, EntryPoint = nameof(soundio_connect), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_connect_osx(IntPtr soundio);

        [DllImport(MacSoundIoLibrary, EntryPoint = nameof(soundio_flush_events), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void soundio_flush_events_osx(IntPtr soundio);

        [DllImport(MacSoundIoLibrary, EntryPoint = nameof(soundio_output_device_count), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int soundio_output_device_count_osx(IntPtr soundio);

        [DllImport(MacSoundIoLibrary, EntryPoint = nameof(soundio_get_output_device), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr soundio_get_output_device_osx(IntPtr soundio, int index);

        [DllImport(MacSoundIoLibrary, EntryPoint = nameof(soundio_default_output_device_index), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int soundio_default_output_device_index_osx(IntPtr soundio);

        [DllImport(MacSoundIoLibrary, EntryPoint = nameof(soundio_outstream_create), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr soundio_outstream_create_osx(IntPtr device);

        [DllImport(MacSoundIoLibrary, EntryPoint = nameof(soundio_outstream_open), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_outstream_open_osx(IntPtr outstream);

        [DllImport(MacSoundIoLibrary, EntryPoint = nameof(soundio_outstream_start), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_outstream_start_osx(IntPtr outstream);

        [DllImport(MacSoundIoLibrary, EntryPoint = nameof(soundio_outstream_destroy), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void soundio_outstream_destroy_osx(IntPtr outstream);

        [DllImport(MacSoundIoLibrary, EntryPoint = nameof(soundio_device_unref), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void soundio_device_unref_osx(IntPtr device);

        [DllImport(MacSoundIoLibrary, EntryPoint = nameof(soundio_outstream_begin_write), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_outstream_begin_write_osx(IntPtr outstream, ref IntPtr areas, ref int frame_count);

        [DllImport(MacSoundIoLibrary, EntryPoint = nameof(soundio_outstream_end_write), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_outstream_end_write_osx(IntPtr outstream);

        [DllImport(MacSoundIoLibrary, EntryPoint = nameof(soundio_outstream_pause), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern SoundIoError soundio_outstream_pause_osx(IntPtr outstream, bool pause);

        [DllImport(MacSoundIoLibrary, EntryPoint = nameof(soundio_channel_layout_get_builtin), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr soundio_channel_layout_get_builtin_osx(SoundIoChannelLayoutId index);

        [DllImport(MacSoundIoLibrary, EntryPoint = nameof(soundio_device_supports_layout), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern bool soundio_device_supports_layout_osx(IntPtr device, IntPtr layout);

        [DllImport(MacSoundIoLibrary, EntryPoint = nameof(soundio_channel_layout_find_channel), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int soundio_channel_layout_find_channel_osx(IntPtr layout, SoundIoChannelId channel);

        [DllImport(MacSoundIoLibrary, EntryPoint = nameof(soundio_channel_layout_get_default), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr soundio_channel_layout_get_default_osx(int channel_count);

        #endregion

        #region Delegates and Platform-Dependent Loading

        // These delegates all represent the function signatures for the libsoundio methods I need to call.

        private delegate IntPtr soundio_create_delegate();
        private delegate void soundio_destroy_delegate(IntPtr soundio);
        private delegate SoundIoError soundio_connect_delegate(IntPtr soundio);
        private delegate void soundio_flush_events_delegate(IntPtr soundio);
        private delegate int soundio_output_device_count_delegate(IntPtr soundio);
        private delegate IntPtr soundio_get_output_device_delegate(IntPtr soundio, int index);
        private delegate int soundio_default_output_device_index_delegate(IntPtr soundio);
        private delegate IntPtr soundio_outstream_create_delegate(IntPtr device);
        private delegate SoundIoError soundio_outstream_open_delegate(IntPtr outstream);
        private delegate SoundIoError soundio_outstream_start_delegate(IntPtr outstream);
        private delegate void soundio_outstream_destroy_delegate(IntPtr outstream);
        private delegate void soundio_device_unref_delegate(IntPtr device);
        private delegate SoundIoError soundio_outstream_begin_write_delegate(IntPtr outstream, ref IntPtr areas, ref int frame_count);
        private delegate SoundIoError soundio_outstream_end_write_delegate(IntPtr outstream);
        private delegate SoundIoError soundio_outstream_pause_delegate(IntPtr outstream, bool pause);
        private delegate IntPtr soundio_channel_layout_get_builtin_delegate(SoundIoChannelLayoutId index);
        private delegate bool soundio_device_supports_layout_delegate(IntPtr device, IntPtr layout);
        private delegate int soundio_channel_layout_find_channel_delegate(IntPtr layout, SoundIoChannelId channel);
        private delegate IntPtr soundio_channel_layout_get_default_delegate(int channel_count);

        // These fields represent function pointers towards each of the extern functions. They get set
        // to the proper platform-specific functions by the static constructor. For example, if this is
        // running on a Windows machine, each of these pointers will point to the various soundio_XXX_windows
        // extern functions listed above.

        private static soundio_create_delegate soundio_create_impl;
        private static soundio_destroy_delegate soundio_destroy_impl;
        private static soundio_connect_delegate soundio_connect_impl;
        private static soundio_flush_events_delegate soundio_flush_events_impl;
        private static soundio_output_device_count_delegate soundio_output_device_count_impl;
        private static soundio_get_output_device_delegate soundio_get_output_device_impl;
        private static soundio_default_output_device_index_delegate soundio_default_output_device_index_impl;
        private static soundio_outstream_create_delegate soundio_outstream_create_impl;
        private static soundio_outstream_open_delegate soundio_outstream_open_impl;
        private static soundio_outstream_start_delegate soundio_outstream_start_impl;
        private static soundio_outstream_destroy_delegate soundio_outstream_destroy_impl;
        private static soundio_device_unref_delegate soundio_device_unref_impl;
        private static soundio_outstream_begin_write_delegate soundio_outstream_begin_write_impl;
        private static soundio_outstream_end_write_delegate soundio_outstream_end_write_impl;
        private static soundio_outstream_pause_delegate soundio_outstream_pause_impl;
        private static soundio_channel_layout_get_builtin_delegate soundio_channel_layout_get_builtin_impl;
        private static soundio_device_supports_layout_delegate soundio_device_supports_layout_impl;
        private static soundio_channel_layout_find_channel_delegate soundio_channel_layout_find_channel_impl;
        private static soundio_channel_layout_get_default_delegate soundio_channel_layout_get_default_impl;

        /// <summary>
        /// The static constructor figures out which library to use for P/Invoke based
        /// on the current OS platform.
        /// </summary>
        static SoundIoInterop()
        {
            NativePathFinder.AddNativeLibraryPathToEnvironmentVariable();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                soundio_create_impl = soundio_create_windows;
                soundio_destroy_impl = soundio_destroy_windows;
                soundio_connect_impl = soundio_connect_windows;
                soundio_flush_events_impl = soundio_flush_events_windows;
                soundio_output_device_count_impl = soundio_output_device_count_windows;
                soundio_get_output_device_impl = soundio_get_output_device_windows;
                soundio_default_output_device_index_impl = soundio_default_output_device_index_windows;
                soundio_outstream_create_impl = soundio_outstream_create_windows;
                soundio_outstream_open_impl = soundio_outstream_open_windows;
                soundio_outstream_start_impl = soundio_outstream_start_windows;
                soundio_outstream_destroy_impl = soundio_outstream_destroy_windows;
                soundio_device_unref_impl = soundio_device_unref_windows;
                soundio_outstream_begin_write_impl = soundio_outstream_begin_write_windows;
                soundio_outstream_end_write_impl = soundio_outstream_end_write_windows;
                soundio_outstream_pause_impl = soundio_outstream_pause_windows;
                soundio_channel_layout_get_builtin_impl = soundio_channel_layout_get_builtin_windows;
                soundio_device_supports_layout_impl = soundio_device_supports_layout_windows;
                soundio_channel_layout_find_channel_impl = soundio_channel_layout_find_channel_windows;
                soundio_channel_layout_get_default_impl = soundio_channel_layout_get_default_windows;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                soundio_create_impl = soundio_create_linux;
                soundio_destroy_impl = soundio_destroy_linux;
                soundio_connect_impl = soundio_connect_linux;
                soundio_flush_events_impl = soundio_flush_events_linux;
                soundio_output_device_count_impl = soundio_output_device_count_linux;
                soundio_get_output_device_impl = soundio_get_output_device_linux;
                soundio_default_output_device_index_impl = soundio_default_output_device_index_linux;
                soundio_outstream_create_impl = soundio_outstream_create_linux;
                soundio_outstream_open_impl = soundio_outstream_open_linux;
                soundio_outstream_start_impl = soundio_outstream_start_linux;
                soundio_outstream_destroy_impl = soundio_outstream_destroy_linux;
                soundio_device_unref_impl = soundio_device_unref_linux;
                soundio_outstream_begin_write_impl = soundio_outstream_begin_write_linux;
                soundio_outstream_end_write_impl = soundio_outstream_end_write_linux;
                soundio_outstream_pause_impl = soundio_outstream_pause_linux;
                soundio_channel_layout_get_builtin_impl = soundio_channel_layout_get_builtin_linux;
                soundio_device_supports_layout_impl = soundio_device_supports_layout_linux;
                soundio_channel_layout_find_channel_impl = soundio_channel_layout_find_channel_linux;
                soundio_channel_layout_get_default_impl = soundio_channel_layout_get_default_linux;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                soundio_create_impl = soundio_create_osx;
                soundio_destroy_impl = soundio_destroy_osx;
                soundio_connect_impl = soundio_connect_osx;
                soundio_flush_events_impl = soundio_flush_events_osx;
                soundio_output_device_count_impl = soundio_output_device_count_osx;
                soundio_get_output_device_impl = soundio_get_output_device_osx;
                soundio_default_output_device_index_impl = soundio_default_output_device_index_osx;
                soundio_outstream_create_impl = soundio_outstream_create_osx;
                soundio_outstream_open_impl = soundio_outstream_open_osx;
                soundio_outstream_start_impl = soundio_outstream_start_osx;
                soundio_outstream_destroy_impl = soundio_outstream_destroy_osx;
                soundio_device_unref_impl = soundio_device_unref_osx;
                soundio_outstream_begin_write_impl = soundio_outstream_begin_write_osx;
                soundio_outstream_end_write_impl = soundio_outstream_end_write_osx;
                soundio_outstream_pause_impl = soundio_outstream_pause_osx;
                soundio_channel_layout_get_builtin_impl = soundio_channel_layout_get_builtin_osx;
                soundio_device_supports_layout_impl = soundio_device_supports_layout_osx;
                soundio_channel_layout_find_channel_impl = soundio_channel_layout_find_channel_osx;
                soundio_channel_layout_get_default_impl = soundio_channel_layout_get_default_osx;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region Public API
        
        public static IntPtr soundio_create()
        {
            return soundio_create_impl();
        }
        
        public static void soundio_destroy(IntPtr soundio)
        {
            soundio_destroy_impl(soundio);
        }
        
        public static SoundIoError soundio_connect(IntPtr soundio)
        {
            return soundio_connect_impl(soundio);
        }
        
        public static void soundio_flush_events(IntPtr soundio)
        {
            soundio_flush_events_impl(soundio);
        }
        
        public static int soundio_output_device_count(IntPtr soundio)
        {
            return soundio_output_device_count_impl(soundio);
        }
        
        public static IntPtr soundio_get_output_device(IntPtr soundio, int index)
        {
            return soundio_get_output_device_impl(soundio, index);
        }
        
        public static int soundio_default_output_device_index(IntPtr soundio)
        {
            return soundio_default_output_device_index_impl(soundio);
        }

        public static IntPtr soundio_outstream_create(IntPtr device)
        {
            return soundio_outstream_create_impl(device);
        }
        
        public static SoundIoError soundio_outstream_open(IntPtr outstream)
        {
            return soundio_outstream_open_impl(outstream);
        }
        
        public static SoundIoError soundio_outstream_start(IntPtr outstream)
        {
            return soundio_outstream_start_impl(outstream);
        }
        
        public static void soundio_outstream_destroy(IntPtr outstream)
        {
            soundio_outstream_destroy_impl(outstream);
        }
        
        public static void soundio_device_unref(IntPtr device)
        {
            soundio_device_unref_impl(device);
        }
        
        public static SoundIoError soundio_outstream_begin_write(IntPtr outstream, ref IntPtr areas, ref int frame_count)
        {
            return soundio_outstream_begin_write_impl(outstream, ref areas, ref frame_count);
        }
        
        public static SoundIoError soundio_outstream_end_write(IntPtr outstream)
        {
            return soundio_outstream_end_write_impl(outstream);
        }
        
        public static SoundIoError soundio_outstream_pause(IntPtr outstream, bool pause)
        {
            return soundio_outstream_pause_impl(outstream, pause);
        }
        
        public static IntPtr soundio_channel_layout_get_builtin(SoundIoChannelLayoutId index)
        {
            return soundio_channel_layout_get_builtin_impl(index);
        }
        
        public static bool soundio_device_supports_layout(IntPtr device, IntPtr layout)
        {
            return soundio_device_supports_layout_impl(device, layout);
        }
        
        public static int soundio_channel_layout_find_channel(IntPtr layout, SoundIoChannelId channel)
        {
            return soundio_channel_layout_find_channel_impl(layout, channel);
        }
        
        public static IntPtr soundio_channel_layout_get_default(int channel_count)
        {
            return soundio_channel_layout_get_default_impl(channel_count);
        }

        #endregion
    }
}
