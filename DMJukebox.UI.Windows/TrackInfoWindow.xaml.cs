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

            int minutes = (int)Info.Duration.TotalMinutes;
            double seconds = Info.Duration.TotalSeconds;
            seconds -= minutes * 60;
            DurationLabel.Content = $"{minutes}:{seconds.ToString("N2")}";

            CodecLabel.Content = Info.CodecName;
            ChannelsLabel.Content = Info.NumberOfChannels;

            double bitrate = Info.Bitrate / 1024.0;
            BitRateLabel.Content = $"{bitrate.ToString("N2")} kbps";

            SampleRateLabel.Content = $"{Info.SampleRate} Hz";
        }
    }
}
