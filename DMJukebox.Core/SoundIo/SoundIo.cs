/* 
 * This file contains a C# implementation of the SoundIo struct
 * as defined in soundio.h of the libsoundio project, for interop use.
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
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void on_devices_change(IntPtr soundio);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void on_backend_disconnect(IntPtr soundio, int err);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void on_events_signal(IntPtr soundio);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void emit_rtprio_warning();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void jack_info_callback(
        [MarshalAs(UnmanagedType.LPStr)] string msg);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void jack_error_callback(
        [MarshalAs(UnmanagedType.LPStr)] string msg);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct SoundIo
    {
        /// <summary>
        /// Optional. Put whatever you want here. Defaults to NULL.
        /// </summary>
        public IntPtr userdata;

        /// <summary>
        /// Optional callback. Called when the list of devices change. Only called
        /// during a call to ::soundio_flush_events or ::soundio_wait_events.
        /// </summary>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public on_devices_change on_devices_change;

        /// <summary>
        /// Optional callback. Called when the backend disconnects. For example,
        /// when the JACK server shuts down. When this happens, listing devices
        /// and opening streams will always fail with
        /// SoundIoErrorBackendDisconnected. This callback is only called during a
        /// call to ::soundio_flush_events or ::soundio_wait_events.
        /// If you do not supply a callback, the default will crash your program
        /// with an error message. This callback is also called when the thread
        /// that retrieves device information runs into an unrecoverable condition
        /// such as running out of memory.
        ///
        /// Possible errors:
        /// * #SoundIoErrorBackendDisconnected
        /// * #SoundIoErrorNoMem
        /// * #SoundIoErrorSystemResources
        /// * #SoundIoErrorOpeningDevice - unexpected problem accessing device
        ///   information
        /// </summary>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public on_backend_disconnect on_backend_disconnect;

        /// <summary>
        /// Optional callback. Called from an unknown thread that you should not use
        /// to call any soundio functions. You may use this to signal a condition
        /// variable to wake up. Called when ::soundio_wait_events would be woken up.
        /// </summary>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public on_events_signal on_events_signal;

        /// <summary>
        /// Read-only. After calling ::soundio_connect or ::soundio_connect_backend,
        /// this field tells which backend is currently connected.
        /// </summary>
        public SoundIoBackend current_backend;

        /// <summary>
        /// Optional: Application name.
        /// PulseAudio uses this for "application name".
        /// JACK uses this for `client_name`.
        /// Must not contain a colon (":").
        /// </summary>
        public string app_name;

        /// <summary>
        /// Optional: Real time priority warning.
        /// This callback is fired when making thread real-time priority failed. By
        /// default, it will print to stderr only the first time it is called
        /// a message instructing the user how to configure their system to allow
        /// real-time priority threads. This must be set to a function not NULL.
        /// To silence the warning, assign this to a function that does nothing.
        /// </summary>
        public emit_rtprio_warning emit_rtprio_warning;

        /// <summary>
        /// Optional: JACK info callback.
        /// By default, libsoundio sets this to an empty function in order to
        /// silence stdio messages from JACK. You may override the behavior by
        /// setting this to `NULL` or providing your own function. This is
        /// registered with JACK regardless of whether ::soundio_connect_backend
        /// succeeds.
        /// </summary>
        public jack_info_callback jack_info_callback;

        /// <summary>
        /// Optional: JACK error callback.
        /// See SoundIo::jack_info_callback
        /// </summary>
        public jack_error_callback jack_error_callback;
    }
}
