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
    /// Interaction logic for DiscordSettingsWindow.xaml
    /// </summary>
    internal partial class DiscordSettingsWindow : Window
    {
        public DiscordSettings Settings { get; }

        public DiscordSettingsWindow(DiscordSettings Settings)
        {
            InitializeComponent();
            this.Settings = Settings;
            BotTokenIdBox.Text = Settings.BotTokenID;
            GuildIdBox.Text = Settings.GuildID;
            ChannelIdBox.Text = Settings.ChannelID;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Settings.BotTokenID = BotTokenIdBox.Text;
            Settings.GuildID = GuildIdBox.Text;
            Settings.ChannelID = ChannelIdBox.Text;
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

    }
}
