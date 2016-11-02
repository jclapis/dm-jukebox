using DiscordJukebox.Interop;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace DiscordJukebox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static LogCallback LoggerDelegate;

        static MainWindow()
        {

        }

        public MainWindow()
        {
            InitializeComponent();
            LoggerDelegate = Logger;
            AvFormatInterface.av_log_set_callback(LoggerDelegate);
            AvFormatInterface.av_register_all();
        }

        private void Logger(IntPtr avcl, int level, string fmt, IntPtr args)
        {
            StringBuilder builder = new StringBuilder(Msvcrt._vscprintf(fmt, args) + 1);
            Msvcrt.vsprintf(builder, fmt, args);

            //StuffBox.Text += $"[{level}] {builder.ToString()}";
        }

        protected override void OnClosing(CancelEventArgs e)
        {

        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            bool? result = dialog.ShowDialog();
            if(result == true)
            {
                string filename = dialog.FileName;
                IntPtr formatContext = IntPtr.Zero;
                try
                {
                    IntPtr options = IntPtr.Zero;
                    formatContext = AvFormatInterface.avformat_alloc_context();
                    int openResult = AvFormatInterface.avformat_open_input(ref formatContext, filename, IntPtr.Zero, ref options);
                    if(openResult != 0)
                    {
                        StuffBox.Text += $"Load failed: {openResult}" + Environment.NewLine;
                        return;
                    }
                    AVFormatContext format = (AVFormatContext)Marshal.PtrToStructure(formatContext, typeof(AVFormatContext));
                    StuffBox.Text += $"Loaded!{Environment.NewLine}";

                    StuffBox.Text += $"Found {format.nb_streams} streams.{Environment.NewLine}";
                    for (int i = 0; i < format.nb_streams; i++)
                    {
                        IntPtr streamPtr = Marshal.ReadIntPtr(format.streams + i);
                        AVStream stream = Marshal.PtrToStructure<AVStream>(streamPtr);
                        stream.codec
                    }
                }
                finally
                {
                    if(formatContext != IntPtr.Zero)
                    {
                        AvFormatInterface.avformat_free_context(formatContext);
                    }
                }
            }
        }

    }
}
