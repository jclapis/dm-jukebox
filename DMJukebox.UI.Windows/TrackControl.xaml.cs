using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DMJukebox
{
    /// <summary>
    /// Interaction logic for TrackControl.xaml
    /// </summary>
    public partial class TrackControl : UserControl
    {
        private readonly Delegate HandleClickOutOfNameBoxDelegate;

        public TrackControl()
        {
            InitializeComponent();
            HandleClickOutOfNameBoxDelegate = new MouseButtonEventHandler(HandleClickOutOfNameBox);
        }

        private void EnableRename(object sender, MouseButtonEventArgs e)
        {
            NameBox.Text = NameLabelText.Text;
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

            NameLabelText.Text = newName;
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

    }
}
