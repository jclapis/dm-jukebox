/* 
 * This file contains a C# implementation of the SoundIoOutStream struct
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

namespace DiscordJukebox.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void write_callback(IntPtr outstream, int frame_count_min, int frame_count_max);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void underflow_callback(IntPtr outstream);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate void error_callback(IntPtr outstream, SoundIoError err);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct SoundIoOutStream
    {
        /// <summary>
        /// Populated automatically when you call ::soundio_outstream_create.
        /// </summary>
        public IntPtr device;

        /// <summary>
        /// Defaults to #SoundIoFormatFloat32NE, followed by the first one
        /// supported.
        /// </summary>
        public SoundIoFormat format;

        /// <summary>
        /// Sample rate is the number of frames per second.
        /// Defaults to 48000 (and then clamped into range).
        /// </summary>
        public int sample_rate;

        /// <summary>
        /// Defaults to Stereo, if available, followed by the first layout
        /// supported.
        /// </summary>
        public SoundIoChannelLayout layout;

        /// <summary>
        /// Ignoring hardware latency, this is the number of seconds it takes for
        /// the last sample in a full buffer to be played.
        /// After you call ::soundio_outstream_open, this value is replaced with the
        /// actual software latency, as near to this value as possible.
        /// On systems that support clearing the buffer, this defaults to a large
        /// latency, potentially upwards of 2 seconds, with the understanding that
        /// you will call ::soundio_outstream_clear_buffer when you want to reduce
        /// the latency to 0. On systems that do not support clearing the buffer,
        /// this defaults to a reasonable lower latency value.
        ///
        /// On backends with high latencies (such as 2 seconds), `frame_count_min`
        /// will be 0, meaning you don't have to fill the entire buffer. In this
        /// case, the large buffer is there if you want it; you only have to fill
        /// as much as you want. On backends like JACK, `frame_count_min` will be
        /// equal to `frame_count_max` and if you don't fill that many frames, you
        /// will get glitches.
        ///
        /// If the device has unknown software latency min and max values, you may
        /// still set this, but you might not get the value you requested.
        /// For PulseAudio, if you set this value to non-default, it sets
        /// `PA_STREAM_ADJUST_LATENCY` and is the value used for `maxlength` and
        /// `tlength`.
        ///
        /// For JACK, this value is always equal to
        /// SoundIoDevice::software_latency_current of the device.
        /// </summary>
        public double software_latency;

        /// <summary>
        /// Defaults to NULL. Put whatever you want here.
        /// </summary>
        public IntPtr userdata;

        /// <summary>
        /// In this callback, you call ::soundio_outstream_begin_write and
        /// ::soundio_outstream_end_write as many times as necessary to write
        /// at minimum `frame_count_min` frames and at maximum `frame_count_max`
        /// frames. `frame_count_max` will always be greater than 0. Note that you
        /// should write as many frames as you can; `frame_count_min` might be 0 and
        /// you can still get a buffer underflow if you always write
        /// `frame_count_min` frames.
        ///
        /// For Dummy, ALSA, and PulseAudio, `frame_count_min` will be 0. For JACK
        /// and CoreAudio `frame_count_min` will be equal to `frame_count_max`.
        ///
        /// The code in the supplied function must be suitable for real-time
        /// execution. That means that it cannot call functions that might block
        /// for a long time. This includes all I/O functions (disk, TTY, network),
        /// malloc, free, printf, pthread_mutex_lock, sleep, wait, poll, select,
        /// pthread_join, pthread_cond_wait, etc.
        /// </summary>
        public write_callback write_callback;

        /// <summary>
        /// This optional callback happens when the sound device runs out of
        /// buffered audio data to play. After this occurs, the outstream waits
        /// until the buffer is full to resume playback.
        /// This is called from the SoundIoOutStream::write_callback thread context.
        /// </summary>
        public underflow_callback underflow_callback;

        /// <summary>
        /// Optional callback. `err` is always SoundIoErrorStreaming.
        /// SoundIoErrorStreaming is an unrecoverable error. The stream is in an
        /// invalid state and must be destroyed.
        /// If you do not supply error_callback, the default callback will print
        /// a message to stderr and then call `abort`.
        /// This is called from the SoundIoOutStream::write_callback thread context.
        /// </summary>
        public error_callback error_callback;

        /// <summary>
        /// Optional: Name of the stream. Defaults to "SoundIoOutStream"
        /// PulseAudio uses this for the stream name.
        /// JACK uses this for the client name of the client that connects when you
        /// open the stream.
        /// WASAPI uses this for the session display name.
        /// Must not contain a colon (":").
        /// </summary>
        public string name;

        /// <summary>
        /// Optional: Hint that this output stream is nonterminal. This is used by
        /// JACK and it means that the output stream data originates from an input
        /// stream. Defaults to `false`.
        /// </summary>
        public bool non_terminal_hint;

        /// <summary>
        /// computed automatically when you call ::soundio_outstream_open
        /// </summary>
        public int bytes_per_frame;

        /// <summary>
        /// computed automatically when you call ::soundio_outstream_open
        /// </summary>
        public int bytes_per_sample;

        /// <summary>
        /// If setting the channel layout fails for some reason, this field is set
        /// to an error code. Possible error codes are:
        /// * #SoundIoErrorIncompatibleDevice
        /// </summary>
        public SoundIoError layout_error;
    }
}
