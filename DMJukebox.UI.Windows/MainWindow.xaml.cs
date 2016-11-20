using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace DMJukebox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private static LogCallback LoggerDelegate;

        private readonly AudioTrackManager Player;

        private AudioTrack Stream;

        public MainWindow()
        {
            InitializeComponent();
            //LoggerDelegate = Logger;
            Player = new AudioTrackManager();

            //AVUtilInterop.av_log_set_callback(LoggerDelegate);
            
        }

       /* private void Logger(IntPtr avcl, int level, string fmt, IntPtr args)
        {
            StringBuilder builder = new StringBuilder(MsvcrtInterop._vscprintf(fmt, args) + 1);
            MsvcrtInterop.vsprintf(builder, fmt, args);

            Dispatcher.Invoke(() =>
            {
                StuffBox.Text += $"[{level}] {builder.ToString()}";
            });
        }*/

        protected override void OnClosing(CancelEventArgs e)
        {

        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string filename = dialog.FileName;
                try
                {
                    Stream = Player.CreateTrack(filename);
                    using (StringWriter writer = new StringWriter())
                    {
                        writer.WriteLine($"Loaded file {Path.GetFileName(filename)}");
                        TrackInfo info = Stream.Info;
                        writer.WriteLine($"Codec: {info.CodecName}");
                        writer.WriteLine($"Bitrate: {info.Bitrate}");
                        writer.WriteLine($"Duration: {info.Duration}");
                        writer.WriteLine($"Channels: {info.NumberOfChannels}");
                        writer.WriteLine($"Sample rate: {info.SampleRate}");
                        writer.WriteLine();
                        //StuffBox.Text += writer.ToString();
                    }
                }
                catch(Exception ex)
                {
                    //StuffBox.Text += $"Error opening file: {ex.GetDetails()}";
                }
            }
        }

        private void PlayButtonClick(object sender, RoutedEventArgs e)
        {
            Stream.Play();
            System.Diagnostics.Debug.WriteLine("Started playing");
        }

        private void StopButtonClick(object sender, RoutedEventArgs e)
        {
            Player.StopAllTracks();
            System.Diagnostics.Debug.WriteLine("Stopped playing");
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(VolumeSlider != null && Stream != null)
            {
                Stream.Volume = (float)VolumeSlider.Value;
            }
        }

        private void LoopBox_Checked(object sender, RoutedEventArgs e)
        {
            if(Stream != null)
            {
                if (LoopBox.IsChecked == true)
                {
                    Stream.Loop = true;
                }
                else
                {
                    Stream.Loop = false;
                }
            }
        }

    }
}
