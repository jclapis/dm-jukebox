/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

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
