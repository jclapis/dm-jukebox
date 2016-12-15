/* ========================================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * ====================================================================== */

using System;
using System.Text;
using System.Windows;

namespace DMJukebox
{
    /// <summary>
    /// TrackInfoWindow shows a few statistics / details about a track.
    /// </summary>
    internal partial class TrackInfoWindow : Window
    {
        /// <summary>
        /// Creates a new TrackInfoWindow instance.
        /// </summary>
        /// <param name="TrackName">The name of the track</param>
        /// <param name="Info">The info for the track</param>
        public TrackInfoWindow(string TrackName, TrackInfo Info)
        {
            InitializeComponent();
            Title = $"Info for {TrackName}";
            PathBlock.Text = Info.Path;
            
            // Duration formatting
            TimeSpan duration = Info.Duration;
            StringBuilder builder = new StringBuilder();
            if(duration.Hours > 0)
            {
                builder.Append($"{duration.Hours} Hours, ");
                builder.Append($"{duration.Minutes} Minutes, ");
                builder.Append($"{duration.Seconds} Seconds");
            }
            else if(duration.Minutes > 0)
            {
                builder.Append($"{duration.Minutes} Minutes, ");
                builder.Append($"{duration.Seconds} Seconds");
            }
            else
            {
                builder.Append($"{duration.Seconds} Seconds");
            }
            DurationLabel.Content = builder.ToString();

            CodecLabel.Content = Info.CodecName;
            ChannelsLabel.Content = Info.NumberOfChannels;

            double bitrate = Info.Bitrate / 1024.0;
            BitRateLabel.Content = $"{bitrate.ToString("N2")} kbps";

            SampleRateLabel.Content = $"{Info.SampleRate} Hz";
        }
    }
}
