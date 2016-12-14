/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DMJukebox
{
    /// <summary>
    /// MainWindow is the main UI window and holds the basic application logic
    /// for the project.
    /// </summary>
    internal partial class MainWindow : Window
    {
        // Ignore this, it was used to debug FFmpeg early on in the project
        /* private static LogCallback LoggerDelegate; */

        /// <summary>
        /// The jukebox's playback core
        /// </summary>
        private readonly JukeboxCore Core;

        /// <summary>
        /// Creates a new MainWindow instance.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                Core = new JukeboxCore();

                // Set up the playback mode dropdown
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error during startup: {ex.GetDetails()}");
                Application.Current.Shutdown();
            }

            // These are placeholder tracks to help test the playlist resizing function
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


            // Ignore this, old FFmpeg debugging code
            /* LoggerDelegate = Logger;
            AVUtilInterop.av_log_set_callback(LoggerDelegate); */
        }

        // Ignore this, old FFmpeg debugging code
        /* private void Logger(IntPtr avcl, int level, string fmt, IntPtr args)
        {
            StringBuilder builder = new StringBuilder(MsvcrtInterop._vscprintf(fmt, args) + 1);
            MsvcrtInterop.vsprintf(builder, fmt, args);

            Dispatcher.Invoke(() =>
            {
                StuffBox.Text += $"[{level}] {builder.ToString()}";
            });
        } */

        /// <summary>
        /// Disposes of the core and cleans up when the application closes.
        /// </summary>
        /// <param name="Args">Not used</param>
        protected override void OnClosing(CancelEventArgs Args)
        {
            Core.Dispose();
        }

        /// <summary>
        /// Adds a new track to the system.
        /// </summary>
        /// <param name="Sender">Not used</param>
        /// <param name="Args">Not used</param>
        private void HandleAddTrackButton(object Sender, RoutedEventArgs Args)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string filename = dialog.FileName;
                try
                {
                    AudioTrack track = Core.CreateTrack(filename);
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

        /// <summary>
        /// Cleans up the core during shutdown.
        /// </summary>
        /// <param name="Sender">Not used</param>
        /// <param name="Args">Not used</param>
        private void HandleExitButton(object Sender, RoutedEventArgs Args)
        {
            //Manager.StopAllTracks();
            Core.Dispose();
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Updates the core when the user changes the playback mode.
        /// </summary>
        /// <param name="Sender">Not used</param>
        /// <param name="Args">The arguments with the new mode</param>
        private void PlaybackModeBox_SelectionChanged(object Sender, SelectionChangedEventArgs Args)
        {
            object selection = PlaybackModeBox.SelectedItem;
            if (Core != null && selection != null)
            {
                Core.PlaybackMode = ((PlaybackModeItem)Args.AddedItems[0]).Value;
            }
        }

        /// <summary>
        /// NYI
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="Args"></param>
        private void StopAllButton_Click(object Sender, RoutedEventArgs Args)
        {

        }

        /// <summary>
        /// Connects to Discord.
        /// </summary>
        /// <param name="Sender">Not used</param>
        /// <param name="Args">Not used</param>
        private void DiscordConnectButton_Click(object Sender, RoutedEventArgs Args)
        {
            Task connectTask = Core.ConnectToDiscord();
        }

        /// <summary>
        /// Shows the discord settings window.
        /// </summary>
        /// <param name="Sender">Not used</param>
        /// <param name="Args">Not used</param>
        private void DiscordSettingsButton_Click(object Sender, RoutedEventArgs Args)
        {
            Configuration config = Core.Configuration;
            if (config.DiscordSettings == null)
            {
                config.DiscordSettings = new DiscordSettings();
            }
            DiscordSettingsWindow settingsWindow = new DiscordSettingsWindow(config.DiscordSettings);
            settingsWindow.Owner = this;
            if (settingsWindow.ShowDialog() == true)
            {
                Core.SetDiscordSettings(settingsWindow.Settings);
            }
        }

    }

    /// <summary>
    /// This is a helper class that acts as the items for the
    /// <see cref="MainWindow.PlaybackModeBox"/> selection.
    /// </summary>
    internal class PlaybackModeItem
    {
        /// <summary>
        /// The name of the mode
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The mode this item represents
        /// </summary>
        public PlaybackMode Value { get; set; }
    }
}
