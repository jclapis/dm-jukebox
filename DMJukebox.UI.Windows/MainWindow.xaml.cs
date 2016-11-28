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
    internal partial class MainWindow : Window
    {
        //private static LogCallback LoggerDelegate;

        private readonly AudioTrackManager Manager;

        public MainWindow()
        {
            InitializeComponent();
            //LoggerDelegate = Logger;
            try
            {
                Manager = new AudioTrackManager();
                PlaybackModeBox.Items.Add(new PlaybackModeItem
                {
                    Name = "Local Speakers",
                    Value = PlaybackMode.LocalSpeakers
                });
                PlaybackModeBox.Items.Add(new PlaybackModeItem
                {
                    Name = "Discord",
                    Value = PlaybackMode.Discord
                });
                PlaybackModeBox.SelectedIndex = 0;
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error during startup: {ex.GetDetails()}");
                Application.Current.Shutdown();
            }

            ((TrackControl)TensionPanel.Children[0]).SetName("Woedica");
            ((TrackControl)TensionPanel.Children[1]).SetName("Temple of Skean");
            ((TrackControl)TensionPanel.Children[2]).SetName("Veiled Mist");
            ((TrackControl)TensionPanel.Children[3]).SetName("Ancient Sorrow");
            ((TrackControl)TensionPanel.Children[4]).SetName("Succession of Witches");
            ((TrackControl)TensionPanel.Children[5]).SetName("The Monastery");
            ((TrackControl)TensionPanel.Children[6]).SetName("13th Age - Mystery 2");
            ((TrackControl)TensionPanel.Children[7]).SetName("LIMB Clinic");
            ((TrackControl)TensionPanel.Children[8]).SetName("Od Nua B");
            ((TrackControl)TensionPanel.Children[9]).SetName("Unmarked Stone");


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
                    AudioTrack track = Manager.CreateTrack(filename);
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
            Manager.StopAllTracks();
            //Player.Dispose();
            Application.Current.Shutdown();
        }

        private void PlaybackModeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object selection = PlaybackModeBox.SelectedItem;
            if (Manager != null && selection != null)
            {
                Manager.PlaybackMode = ((PlaybackModeItem)selection).Value;
            }
        }

        private void StopAllButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void DiscordConnectButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.ConnectToDiscord();
        }

        private void DiscordSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Configuration config = Manager.Configuration;
            DiscordSettingsWindow settingsWindow = new DiscordSettingsWindow(config.DiscordSettings);
            settingsWindow.Owner = this;
            if (settingsWindow.ShowDialog() == true)
            {
                Manager.SetDiscordSettings(settingsWindow.Settings);
            }
        }

    }

    internal class PlaybackModeItem
    {
        public string Name { get; set; }

        public PlaybackMode Value { get; set; }
    }
}
