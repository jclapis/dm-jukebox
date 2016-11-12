/* 
 * This file contains a C# implementation of the SoundIoDevice struct
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
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct SoundIoDevice
    {
        /// <summary>
        /// Read-only. Set automatically.
        /// </summary>
        public IntPtr soundio;

        /// <summary>
        /// A string of bytes that uniquely identifies this device.
        /// If the same physical device supports both input and output, that makes
        /// one SoundIoDevice for the input and one SoundIoDevice for the output.
        /// In this case, the id of each SoundIoDevice will be the same, and
        /// SoundIoDevice::aim will be different. Additionally, if the device
        /// supports raw mode, there may be up to four devices with the same id:
        /// one for each value of SoundIoDevice::is_raw and one for each value of
        /// SoundIoDevice::aim.
        /// </summary>
        public string id;

        /// <summary>
        /// User-friendly UTF-8 encoded text to describe the device.
        /// </summary>
        public string name;

        /// <summary>
        /// Tells whether this device is an input device or an output device.
        /// </summary>
        public SoundIoDeviceAim aim;

        /// <summary>
        /// Channel layouts are handled similarly to SoundIoDevice::formats.
        /// If this information is missing due to a SoundIoDevice::probe_error,
        /// layouts will be NULL. It's OK to modify this data, for example calling
        /// ::soundio_sort_channel_layouts on it.
        /// Devices are guaranteed to have at least 1 channel layout.
        /// </summary>
        public IntPtr layouts;

        public int layout_count;

        /// <summary>
        /// See SoundIoDevice::current_format
        /// </summary>
        public SoundIoChannelLayout current_layout;

        /// <summary>
        /// List of formats this device supports. See also SoundIoDevice::current_format.
        /// </summary>
        public IntPtr formats;

        /// <summary>
        /// How many formats are available in SoundIoDevice::formats.
        /// </summary>
        public int format_count;

        /// <summary>
        /// A device is either a raw device or it is a virtual device that is
        /// provided by a software mixing service such as dmix or PulseAudio (see
        /// SoundIoDevice::is_raw). If it is a raw device,
        /// current_format is meaningless;
        /// the device has no current format until you open it. On the other hand,
        /// if it is a virtual device, current_format describes the
        /// destination sample format that your audio will be converted to. Or,
        /// if you're the lucky first application to open the device, you might
        /// cause the current_format to change to your format.
        /// Generally, you want to ignore current_format and use
        /// whatever format is most convenient
        /// for you which is supported by the device, because when you are the only
        /// application left, the mixer might decide to switch
        /// current_format to yours. You can learn the supported formats via
        /// formats and SoundIoDevice::format_count. If this information is missing
        /// due to a probe error, formats will be `NULL`. If current_format is
        /// unavailable, it will be set to #SoundIoFormatInvalid.
        /// Devices are guaranteed to have at least 1 format available.
        /// </summary>
        public SoundIoFormat current_format;

        /// <summary>
        /// Sample rate is the number of frames per second.
        /// Sample rate is handled very similar to SoundIoDevice::formats.
        /// If sample rate information is missing due to a probe error, the field
        /// will be set to NULL.
        /// Devices which have SoundIoDevice::probe_error set to #SoundIoErrorNone are
        /// guaranteed to have at least 1 sample rate available.
        /// </summary>
        public IntPtr sample_rates;

        /// <summary>
        /// How many sample rate ranges are available in
        /// SoundIoDevice::sample_rates. 0 if sample rate information is missing
        /// due to a probe error.
        /// </summary>
        public int sample_rate_count;

        /// <summary>
        /// See SoundIoDevice::current_format
        /// 0 if sample rate information is missing due to a probe error.
        /// </summary>
        public int sample_rate_current;

        /// <summary>
        /// Software latency minimum in seconds. If this value is unknown or
        /// irrelevant, it is set to 0.0.
        /// For PulseAudio and WASAPI this value is unknown until you open a
        /// stream.
        /// </summary>
        public double software_latency_min;

        /// <summary>
        /// Software latency maximum in seconds. If this value is unknown or
        /// irrelevant, it is set to 0.0.
        /// For PulseAudio and WASAPI this value is unknown until you open a
        /// stream.
        /// </summary>
        public double software_latency_max;

        /// <summary>
        /// Software latency in seconds. If this value is unknown or
        /// irrelevant, it is set to 0.0.
        /// For PulseAudio and WASAPI this value is unknown until you open a
        /// stream.
        /// See SoundIoDevice::current_format
        /// </summary>
        public double software_latency_current;

        /// <summary>
        /// Raw means that you are directly opening the hardware device and not
        /// going through a proxy such as dmix, PulseAudio, or JACK. When you open a
        /// raw device, other applications on the computer are not able to
        /// simultaneously access the device. Raw devices do not perform automatic
        /// resampling and thus tend to have fewer formats available.
        /// </summary>
        public bool is_raw;

        /// <summary>
        /// Devices are reference counted. See ::soundio_device_ref and
        /// ::soundio_device_unref.
        /// </summary>
        public int ref_count;

        /// <summary>
        /// This is set to a SoundIoError representing the result of the device
        /// probe. Ideally this will be SoundIoErrorNone in which case all the
        /// fields of the device will be populated. If there is an error code here
        /// then information about formats, sample rates, and channel layouts might
        /// be missing.
        ///
        /// Possible errors:
        /// * #SoundIoErrorOpeningDevice
        /// * #SoundIoErrorNoMem
        /// </summary>
        public SoundIoError probe_error; 
    }
}
