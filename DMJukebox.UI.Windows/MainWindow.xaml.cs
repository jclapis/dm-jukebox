using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace DMJukebox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private static LogCallback LoggerDelegate;

        private readonly AudioTrackManager Player;

        public MainWindow()
        {
            InitializeComponent();
            //LoggerDelegate = Logger;
            try
            {
                Player = new AudioTrackManager();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error during startup: {ex.GetDetails()}");
                Application.Current.Shutdown();
            }

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

        private void HandleAddTrackButton(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string filename = dialog.FileName;
                try
                {
                    AudioTrack track = Player.CreateTrack(filename);
                    ActiveTrackGrid.RowDefinitions.Add(new RowDefinition
                    {
                        Height = GridLength.Auto
                    });
                    TrackControl control = new TrackControl(track, this);
                    ActiveTrackGrid.Children.Add(control);
                    Grid.SetRow(control, ActiveTrackGrid.Children.Count - 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Couldn't load the track: {ex.GetDetails()}");
                }
            }
        }

        private void HandleExitButton(object sender, RoutedEventArgs e)
        {
            Player.StopAllTracks();
            //Player.Dispose();
            Application.Current.Shutdown();
        }

    }
}
