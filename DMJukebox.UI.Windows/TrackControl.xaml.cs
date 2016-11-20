using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DMJukebox
{
    /// <summary>
    /// Interaction logic for TrackControl.xaml
    /// </summary>
    internal partial class TrackControl : UserControl
    {
        private readonly Delegate HandleClickOutOfNameBoxDelegate;

        private readonly AudioTrack Track;

        private bool IsPlaying;

        private readonly BitmapImage PlayIcon;

        private readonly BitmapImage StopIcon;

        private readonly MainWindow MainWindow;

        public TrackControl()
        {
            InitializeComponent();
        }

        public TrackControl(AudioTrack Track, MainWindow MainWindow)
        {
            InitializeComponent();
            this.Track = Track;
            NameLabelBlock.Text = Track.Name;
            this.MainWindow = MainWindow;
            HandleClickOutOfNameBoxDelegate = new MouseButtonEventHandler(HandleClickOutOfNameBox);
            Uri playIconUri = new Uri("pack://application:,,,/Resources/Play.png");
            Uri stopIconUri = new Uri("pack://application:,,,/Resources/Stop.png");
            PlayIcon = new BitmapImage(playIconUri);
            StopIcon = new BitmapImage(stopIconUri);
        }

        private void EnableRename(object sender, MouseButtonEventArgs e)
        {
            NameBox.Text = NameLabelBlock.Text;
            NameLabel.Visibility = Visibility.Hidden;
            NameBox.Visibility = Visibility.Visible;
            NameBox.Focus();

            Mouse.Capture(this, CaptureMode.SubTree);
            AddHandler(Mouse.PreviewMouseDownOutsideCapturedElementEvent, HandleClickOutOfNameBoxDelegate, true);
        }

        private void SetName()
        {
            string newName = NameBox.Text;
            if(string.IsNullOrEmpty(newName))
            {
                return;
            }

            NameLabelBlock.Text = newName;
            EndRename();
        }

        private void EndRename()
        {
            NameLabel.Visibility = Visibility.Visible;
            NameBox.Visibility = Visibility.Hidden;
            RemoveHandler(Mouse.PreviewMouseDownOutsideCapturedElementEvent, HandleClickOutOfNameBoxDelegate);
            ReleaseMouseCapture();
        }

        private void NameBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Return:
                    SetName();
                    break;

                case Key.Escape:
                    EndRename();
                    break;
            }
        }

        private void NameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            EndRename();
        }

        private void HandleClickOutOfNameBox(object Sender, MouseButtonEventArgs Args)
        {
            EndRename();
        }

        private void HandlePlayButtonClick(object sender, RoutedEventArgs e)
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

        private void HandleLoopBoxChecked(object sender, RoutedEventArgs e)
        {
            Track.Loop = true;
        }

        private void HandleLoopBoxUnchecked(object sender, RoutedEventArgs e)
        {
            Track.Loop = false;
        }

        private void HandleVolumeSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(Track == null)
            {
                return;
            }

            Track.Volume = (float)VolumeSlider.Value;
        }

        private void HandleInfoButtonClick(object sender, RoutedEventArgs e)
        {
            TrackInfoWindow infoWindow = new TrackInfoWindow(Track.Name, Track.Info);
            infoWindow.Owner = MainWindow;
            infoWindow.ShowDialog();
        }

    }
}
