﻿/* ========================================================================
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

using System.Windows;

namespace DMJukebox
{
    /// <summary>
    /// This window is used to get the name for a new playlist.
    /// </summary>
    internal partial class AddPlaylistWindow : Window
    {
        public string PlaylistName { get; private set; }

        /// <summary>
        /// Creates a new AddPlaylistWindow instance.
        /// </summary>
        public AddPlaylistWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Submits the name of the playlist.
        /// </summary>
        /// <param name="Sender">Not used</param>
        /// <param name="Args">Not used</param>
        private void DoneButton_Click(object Sender, RoutedEventArgs Args)
        {
            PlaylistName = PlaylistNameBox.Text;
            DialogResult = true;
        }

    }
}
