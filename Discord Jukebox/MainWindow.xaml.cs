using DiscordJukebox.Interop;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace DiscordJukebox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static LogCallback LoggerDelegate;

        private readonly Player Player;

        public MainWindow()
        {
            InitializeComponent();
            LoggerDelegate = Logger;
            Player = new Player();

            AVUtilInterop.av_log_set_callback(LoggerDelegate);
            AVFormatInterop.av_register_all();
        }

        private void Logger(IntPtr avcl, int level, string fmt, IntPtr args)
        {
            StringBuilder builder = new StringBuilder(MsvcrtInterop._vscprintf(fmt, args) + 1);
            MsvcrtInterop.vsprintf(builder, fmt, args);

            //StuffBox.Text += $"[{level}] {builder.ToString()}";
        }

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
                    AudioStream stream = new AudioStream(filename);
                    using (StringWriter writer = new StringWriter())
                    {
                        writer.WriteLine($"Loaded file {Path.GetFileName(filename)}");
                        writer.WriteLine($"Codec: {stream.CodecName}");
                        writer.WriteLine($"Bitrate: {stream.Bitrate}");
                        writer.WriteLine($"Duration: {stream.Duration}");
                        writer.WriteLine($"Channels: {stream.NumberOfChannels}");
                        writer.WriteLine($"Samples per Frame: {stream.SamplesPerFrame}");
                        writer.WriteLine();
                        StuffBox.Text += writer.ToString();
                    }
                    Player.AddStream(stream);
                }
                catch(Exception ex)
                {
                    StuffBox.Text += $"Error opening file: {ex.GetType().Name} - {ex.Message}{Environment.NewLine}{ex.StackTrace}";
                }
            }
        }

        private void PlayButtonClick(object sender, RoutedEventArgs e)
        {
            Player.Start();
            System.Diagnostics.Debug.WriteLine("Started playing");
        }

        private void StopButtonClick(object sender, RoutedEventArgs e)
        {
            Player.Stop();
            System.Diagnostics.Debug.WriteLine("Stopped playing");
        }

    }
}
