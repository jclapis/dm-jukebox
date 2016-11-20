/*
 * Copyright (c) 2016 Joe Clapis.
 */

using System;

namespace DMJukebox
{
    /// <summary>
    /// TrackInfo describes some of the behind-the-scenes details about a track.
    /// </summary>
    /// <remarks>
    /// The stuff in here is far from all we have access to, we have all of ffmpeg at our disposal so we
    /// can technically put anything we want in here. If people start asking for specific things like
    /// sample size or something, I'll add it to TrackInfo.
    /// </remarks>
    public class TrackInfo
    {
        /// <summary>
        /// The name of the codec used to encode the file
        /// </summary>
        public string CodecName { get; }

        /// <summary>
        /// The number of channels contained within the audio stream
        /// </summary>
        public int NumberOfChannels { get; }

        /// <summary>
        /// The bit rate (bits per second) of the audio track 
        /// </summary>
        public long Bitrate { get; }

        /// <summary>
        /// The sample rate (number of samples per second) for the track
        /// </summary>
        public int SampleRate { get; }

        /// <summary>
        /// The length of the track
        /// </summary>
        public TimeSpan Duration { get; }

        /// <summary>
        /// Creates a new TrackInfo instance.
        /// </summary>
        /// <param name="CodecName">The name of the codec used to encode the file</param>
        /// <param name="NumberOfChannels">The number of channels contained within the audio stream</param>
        /// <param name="Bitrate">The bit rate (bits per second) of the audio track </param>
        /// <param name="SampleRate">The sample rate (number of samples per second) for the track</param>
        /// <param name="Duration">The length of the track</param>
        internal TrackInfo(string CodecName, int NumberOfChannels, long Bitrate, int SampleRate, TimeSpan Duration)
        {
            this.CodecName = CodecName;
            this.NumberOfChannels = NumberOfChannels;
            this.Bitrate = Bitrate;
            this.SampleRate = SampleRate;
            this.Duration = Duration;
        }
    }
}
