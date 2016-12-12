/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

using System;
using System.Text;
using System.Windows;

namespace DMJukebox
{
    /// <summary>
    /// Interaction logic for TrackInfoWindow.xaml
    /// </summary>
    internal partial class TrackInfoWindow : Window
    {
        public TrackInfoWindow(string TrackName, TrackInfo Info)
        {
            InitializeComponent();
            Title = $"Info for {TrackName}";
            PathBlock.Text = Info.Path;

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
