/* 
 * This file contains a C# implementation of the AVClassCategory enum
 * as defined in log.h of the libavutil project, for interop use.
 * 
 * For more information, please see the documentation at
 * https://www.ffmpeg.org/doxygen/trunk/index.html or the source code at
 * https://github.com/FFmpeg/FFmpeg.
 * 
 * Copyright (c) 2016 Joe Clapis.
 */

namespace DMJukebox.Interop
{
    internal enum AVClassCategory
    {
        AV_CLASS_CATEGORY_NA = 0,
        AV_CLASS_CATEGORY_INPUT,
        AV_CLASS_CATEGORY_OUTPUT,
        AV_CLASS_CATEGORY_MUXER,
        AV_CLASS_CATEGORY_DEMUXER,
        AV_CLASS_CATEGORY_ENCODER,
        AV_CLASS_CATEGORY_DECODER,
        AV_CLASS_CATEGORY_FILTER,
        AV_CLASS_CATEGORY_BITSTREAM_FILTER,
        AV_CLASS_CATEGORY_SWSCALER,
        AV_CLASS_CATEGORY_SWRESAMPLER,
        AV_CLASS_CATEGORY_DEVICE_VIDEO_OUTPUT = 40,
        AV_CLASS_CATEGORY_DEVICE_VIDEO_INPUT,
        AV_CLASS_CATEGORY_DEVICE_AUDIO_OUTPUT,
        AV_CLASS_CATEGORY_DEVICE_AUDIO_INPUT,
        AV_CLASS_CATEGORY_DEVICE_OUTPUT,
        AV_CLASS_CATEGORY_DEVICE_INPUT,
        AV_CLASS_CATEGORY_NB,
    }
}
