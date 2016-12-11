/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

using System;
using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void write_callback(IntPtr outstream, int frame_count_min, int frame_count_max);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void underflow_callback(IntPtr outstream);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void error_callback(IntPtr outstream, SoundIoError err);

    /// <summary>
    /// This is a C# implementation of the SoundIoOutStream struct in libsoundio.
    /// It defines a single audio stream used for writing output audio data to
    /// the speakers.
    /// </summary>
    /// <remarks>
    /// This struct is defined in soundio.h of libsoundio.
    /// For more information, please see the documentation at
    /// http://libsound.io/doc-1.1.0/soundio_8h.html
    /// or the source code at https://github.com/andrewrk/libsoundio.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct SoundIoOutStream
    {
        /// <summary>
        /// (<see cref="SoundIoDevice"/>*) The device that this stream is
        /// connected to
        /// </summary>
        public IntPtr device;
        
        /// <summary>
        /// The format that playback audio should be in when writing to this
        /// stream
        /// </summary>
        public SoundIoFormat format;
        
        /// <summary>
        /// The sample rate of the playback audio, in Hz
        /// </summary>
        public int sample_rate;
        
        /// <summary>
        /// The speaker layout for the playback audio
        /// </summary>
        public SoundIoChannelLayout layout;
        
        /// <summary>
        /// The latency, in seconds, between the last sample being
        /// written to the stream and when it gets played by the speakers.
        /// </summary>
        public double software_latency;
        
        /// <summary>
        /// This can be whatever you want, it's personal user data.
        /// </summary>
        public IntPtr userdata;
        
        /// <summary>
        /// The callback for writing sound out to the speakers
        /// </summary>
        public write_callback write_callback;
        
        /// <summary>
        /// The callback for handling an underflow (when the playback
        /// thread required data but you didn't give it enough in time).
        /// This callback is optional.
        /// </summary>
        public underflow_callback underflow_callback;
        
        /// <summary>
        /// The callback that gets called when libsoundio hits an
        /// unrecoverable error during playback.
        /// This callback is optional.
        /// </summary>
        public error_callback error_callback;
        
        /// <summary>
        /// An optional name for the stream. This is used by the different
        /// backends to identify this stream. It doesn't have to be unique.
        /// </summary>
        public string name;
        
        /// <summary>
        /// An optional flag that indicates whether or not the playback
        /// data comes directly from an input stream.
        /// </summary>
        public bool non_terminal_hint;
        
        /// <summary>
        /// The size of an audio frame, in bytes
        /// </summary>
        public int bytes_per_frame;
        
        /// <summary>
        /// The size of a single audio sample, in bytes
        /// </summary>
        public int bytes_per_sample;
        
        /// <summary>
        /// This is set to an error code if setting the channel layout
        /// for the stream fails.
        /// </summary>
        public SoundIoError layout_error;
    }
}
