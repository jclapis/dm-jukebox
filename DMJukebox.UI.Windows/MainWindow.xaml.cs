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

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
        /// A lookup from playlists to their UI controls
        /// </summary>
        private readonly Dictionary<Playlist, Expander> PlaylistControlMap;

        /// <summary>
        /// Creates a new MainWindow instance.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                PlaylistControlMap = new Dictionary<Playlist, Expander>();
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

                foreach (Playlist playlist in Core.Playlists)
                {
                    AddPlaylist(playlist);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during startup: {ex.GetDetails()}");
                Application.Current.Shutdown();
            }

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
            if(Core != null)
            {
                Core.Dispose();
            }
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
                    Playlist playlist = Core.Playlists.Last();
                    AudioTrack track = Core.CreateTrack(filename, playlist); // Temporarily use the last playlist until I fix the UI
                    AddTrack(track, playlist);
                    /*ActiveTrackGrid.RowDefinitions.Add(new RowDefinition
                    {
                        Height = GridLength.Auto
                    });
                    TrackControl control = new TrackControl(track, this);
                    ActiveTrackGrid.Children.Add(control);
                    Grid.SetRow(control, ActiveTrackGrid.Children.Count - 1);*/
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

        /// <summary>
        /// Creates a new playlist.
        /// </summary>
        /// <param name="Sender">Not used</param>
        /// <param name="Args">Not used</param>
        private void HandleAddPlaylistButton(object Sender, RoutedEventArgs Args)
        {
            AddPlaylistWindow window = new AddPlaylistWindow();
            window.Owner = this;
            bool? result = window.ShowDialog();
            if(result == true)
            {
                string name = window.PlaylistName;
                Playlist playlist = Core.CreatePlaylist(name);
                AddPlaylist(playlist);
            }
        }

        /// <summary>
        /// Adds a playlist to the UI.
        /// </summary>
        /// <param name="Playlist">The playlist to add</param>
        private void AddPlaylist(Playlist Playlist)
        {
            // Create the playlist's expander
            PlaylistGrid.RowDefinitions.Add(new RowDefinition
            {
                Height = GridLength.Auto
            });
            Expander expander = new Expander
            {
                Header = Playlist.Name,
                BorderBrush = new SolidColorBrush(Colors.SlateGray),
                BorderThickness = new Thickness(1),
                Margin = new Thickness(0, 5, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                Content = new WrapPanel
                {
                    Orientation = Orientation.Horizontal
                }
            };
            PlaylistGrid.Children.Add(expander);
            Grid.SetRow(expander, PlaylistGrid.RowDefinitions.Count - 1);
            PlaylistControlMap.Add(Playlist, expander);

            // Add all of the tracks from that playlist to the UI
            foreach(AudioTrack track in Playlist.Tracks)
            {
                AddTrack(track, Playlist);
            }
        }

        /// <summary>
        /// Adds a <see cref="TrackControl"/> to the UI for the provided
        /// track, inside the provided playlist.
        /// </summary>
        /// <param name="Track">The track to add a control for</param>
        /// <param name="Playlist">The playlist the track belongs to</param>
        private void AddTrack(AudioTrack Track, Playlist Playlist)
        {
            Expander expander = PlaylistControlMap[Playlist];
            TrackControl control = new TrackControl(Track, this);
            WrapPanel panel = (WrapPanel)expander.Content;
            panel.Children.Add(control);
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
