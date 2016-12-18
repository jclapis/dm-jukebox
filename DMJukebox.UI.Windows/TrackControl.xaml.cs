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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DMJukebox
{
    /// <summary>
    /// TrackControl is the UI control for a single audio track.
    /// </summary>
    internal partial class TrackControl : UserControl
    {
        /// <summary>
        /// This delegate is used while editing the name of the track,
        /// to get notified if the user clicks out of the name box.
        /// </summary>
        private readonly MouseButtonEventHandler HandleClickOutOfNameBoxDelegate;

        /// <summary>
        /// The track backing this control
        /// </summary>
        private readonly AudioTrack Track;

        /// <summary>
        /// This flag keeps track of whether the track is playing or not
        /// </summary>
        private bool IsPlaying;

        /// <summary>
        /// The play icon
        /// </summary>
        private readonly BitmapImage PlayIcon;

        /// <summary>
        /// The stop icon
        /// </summary>
        private readonly BitmapImage StopIcon;

        /// <summary>
        /// The parent window (used to provide the owner of the
        /// <see cref="TrackInfoWindow"/> when the user clicks
        /// on the info button
        /// </summary>
        private readonly MainWindow MainWindow;

        /// <summary>
        /// Temporary constructor for demo / debug purposes - this will
        /// go away once I get the whole playlist system working.
        /// </summary>
        public TrackControl()
        {
            InitializeComponent();
            HandleClickOutOfNameBoxDelegate = new MouseButtonEventHandler(HandleClickOutOfNameBox);
            Uri playIconUri = new Uri("pack://application:,,,/Resources/Play.png");
            Uri stopIconUri = new Uri("pack://application:,,,/Resources/Stop.png");
            PlayIcon = new BitmapImage(playIconUri);
            StopIcon = new BitmapImage(stopIconUri);
        }

        /// <summary>
        /// Creates a new TrackControl instance.
        /// </summary>
        /// <param name="Track">The track that this control represents</param>
        /// <param name="MainWindow">The parent window</param>
        public TrackControl(AudioTrack Track, MainWindow MainWindow)
        {
            InitializeComponent();
            this.Track = Track;
            this.MainWindow = MainWindow;

            NameLabelBlock.Text = Track.Name;
            HandleClickOutOfNameBoxDelegate = new MouseButtonEventHandler(HandleClickOutOfNameBox);
            Uri playIconUri = new Uri("pack://application:,,,/Resources/Play.png");
            Uri stopIconUri = new Uri("pack://application:,,,/Resources/Stop.png");
            PlayIcon = new BitmapImage(playIconUri);
            StopIcon = new BitmapImage(stopIconUri);
            Track.Stopped += HandleTrackStopped;
        }

        /// <summary>
        /// Temporary debug function, this will go away once playlists and
        /// categories work
        /// </summary>
        /// <param name="Name"></param>
        public void SetName(string Name)
        {
            NameLabelBlock.Text = Name;
        }

        /// <summary>
        /// Updates the control when a track stops playing.
        /// </summary>
        /// <param name="Sender">Not used</param>
        /// <param name="Args">Not used</param>
        private void HandleTrackStopped(object Sender, EventArgs Args)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                PlayImage.Source = PlayIcon;
            }));
            IsPlaying = false;
        }

        /// <summary>
        /// Enables the name text box, which can be used to change the name of the track
        /// </summary>
        /// <param name="Sender">Not used</param>
        /// <param name="Args">Not used</param>
        private void EnableRename(object Sender, MouseButtonEventArgs Args)
        {
            NameBox.Text = NameLabelBlock.Text;
            NameLabel.Visibility = Visibility.Hidden;
            NameBox.Visibility = Visibility.Visible;

            // This sets up a listener that gets pinged when the user clicks outside of the name box,
            // thus ending the rename function
            Mouse.Capture(this, CaptureMode.SubTree);
            AddHandler(Mouse.PreviewMouseDownOutsideCapturedElementEvent, HandleClickOutOfNameBoxDelegate, true);
        }

        /// <summary>
        /// Sets the name of the track to whatever is in <see cref="NameLabelBlock.Text"/>.
        /// </summary>
        private void SetName()
        {
            string newName = NameBox.Text;
            if(string.IsNullOrEmpty(newName))
            {
                return;
            }

            NameLabelBlock.Text = newName;
            Track.Name = newName;
            EndRename();
        }

        /// <summary>
        /// Ends the rename function, hiding the text box and bringing the readonly label back.
        /// </summary>
        private void EndRename()
        {
            NameLabel.Visibility = Visibility.Visible;
            NameBox.Visibility = Visibility.Hidden;
            RemoveHandler(Mouse.PreviewMouseDownOutsideCapturedElementEvent, HandleClickOutOfNameBoxDelegate);
            ReleaseMouseCapture();
        }

        /// <summary>
        /// Detects when the user presses Enter or Escape while renaming the track,
        /// and handles them appropriately.
        /// </summary>
        /// <param name="Sender">Not used</param>
        /// <param name="Args">The arguments containing the key that was pressed</param>
        private void NameBox_KeyDown(object Sender, KeyEventArgs Args)
        {
            switch(Args.Key)
            {
                case Key.Return:
                    SetName();
                    break;

                case Key.Escape:
                    EndRename();
                    break;
            }
        }

        /// <summary>
        /// Ends the rename function when the name box loses focus.
        /// </summary>
        /// <param name="Sender">Not used</param>
        /// <param name="Args">Not used</param>
        private void NameBox_LostFocus(object Sender, RoutedEventArgs Args)
        {
            EndRename();
        }

        /// <summary>
        /// Ends the rename function when the user clicks on a control other
        /// than the name box (even if the box retains focus).
        /// </summary>
        /// <param name="Sender">Not used</param>
        /// <param name="Args">Not used</param>
        private void HandleClickOutOfNameBox(object Sender, MouseButtonEventArgs Args)
        {
            EndRename();
        }

        /// <summary>
        /// Starts or stops playback when the play button is clicked.
        /// </summary>
        /// <param name="Sender">Not used</param>
        /// <param name="Args">Not used</param>
        private void HandlePlayButtonClick(object Sender, RoutedEventArgs Args)
        {
            if(!IsPlaying)
            {
                PlayImage.Source = StopIcon;
                Track.Play();
                IsPlaying = true;
            }
            else
            {
                PlayImage.Source = PlayIcon;
                Track.Stop();
                IsPlaying = false;
            }
        }

        /// <summary>
        /// Sets the track's loop flag to true.
        /// </summary>
        /// <param name="Sender">Not used</param>
        /// <param name="Args">Not used</param>
        private void HandleLoopBoxChecked(object Sender, RoutedEventArgs Args)
        {
            Track.IsLoopEnabled = true;
        }

        /// <summary>
        /// Sets the track's loop flag to false.
        /// </summary>
        /// <param name="Sender">Not used</param>
        /// <param name="Args">Not used</param>
        private void HandleLoopBoxUnchecked(object Sender, RoutedEventArgs Args)
        {
            Track.IsLoopEnabled = false;
        }

        /// <summary>
        /// Updates the track's volume when the volume slider changes.
        /// </summary>
        /// <param name="Sender">Not used</param>
        /// <param name="Args">The arguments containing the new volume value</param>
        private void HandleVolumeSliderValueChanged(object Sender, RoutedPropertyChangedEventArgs<double> Args)
        {
            if(Track == null)
            {
                return;
            }

            Track.Volume = (float)Args.NewValue;
        }

        /// <summary>
        /// Opens the <see cref="TrackInfoWindow"/> when the info button is pressed.
        /// </summary>
        /// <param name="Sender">Not used</param>
        /// <param name="Args">Not used</param>
        private void HandleInfoButtonClick(object Sender, RoutedEventArgs Args)
        {
            TrackInfoWindow infoWindow = new TrackInfoWindow(Track.Name, Track.FilePath, Track.Info);
            infoWindow.Owner = MainWindow;
            infoWindow.ShowDialog();
        }

    }
}
