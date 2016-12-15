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

using System.Windows;

namespace DMJukebox
{
    /// <summary>
    /// This window simply displays the Discord connection settings for easy editing.
    /// </summary>
    internal partial class DiscordSettingsWindow : Window
    {
        /// <summary>
        /// The settings object that holds the Discord configuration
        /// </summary>
        public DiscordSettings Settings { get; }

        /// <summary>
        /// Creates a new DiscordSettingsWindow instance.
        /// </summary>
        /// <param name="Settings">The old settings to display when the window is shown</param>
        public DiscordSettingsWindow(DiscordSettings Settings)
        {
            InitializeComponent();
            this.Settings = Settings;
            BotTokenIdBox.Text = Settings.BotTokenID;
            GuildIdBox.Text = Settings.GuildID;
            ChannelIdBox.Text = Settings.ChannelID;
        }

        /// <summary>
        /// Populates the Discord settings with the values in the text boxes.
        /// </summary>
        /// <param name="Sender">Not used</param>
        /// <param name="Args">Not used</param>
        private void OKButton_Click(object Sender, RoutedEventArgs Args)
        {
            Settings.BotTokenID = BotTokenIdBox.Text;
            Settings.GuildID = GuildIdBox.Text;
            Settings.ChannelID = ChannelIdBox.Text;
            DialogResult = true;
        }

        /// <summary>
        /// Closes the window without saving any changes.
        /// </summary>
        /// <param name="Sender">Not used</param>
        /// <param name="Args">Not used</param>
        private void CancelButton_Click(object Sender, RoutedEventArgs Args)
        {
            DialogResult = false;
        }

    }
}
