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
            AVUtilInterface.av_log_set_callback(LoggerDelegate);
            AVFormatInterface.av_register_all();
        }

        private void Logger(IntPtr avcl, int level, string fmt, IntPtr args)
        {
            StringBuilder builder = new StringBuilder(MsvcrtInterface._vscprintf(fmt, args) + 1);
            MsvcrtInterface.vsprintf(builder, fmt, args);

            //StuffBox.Text += $"[{level}] {builder.ToString()}";
        }

        protected override void OnClosing(CancelEventArgs e)
        {

        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string filename = dialog.FileName;
                IntPtr formatContext = IntPtr.Zero;
                try
                {
                    IntPtr options = IntPtr.Zero;
                    formatContext = AVFormatInterface.avformat_alloc_context();
                    int openResult = AVFormatInterface.avformat_open_input(ref formatContext, filename, IntPtr.Zero, ref options);
                    if (openResult != 0)
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
                        AVCodecContext codecContext = Marshal.PtrToStructure<AVCodecContext>(stream.codec);
                        if (codecContext.codec_type == AVMediaType.AVMEDIA_TYPE_AUDIO)
                        {
                            IntPtr codecPtr = AVCodecInterface.avcodec_find_decoder(codecContext.codec_id);
                            if (codecPtr == IntPtr.Zero)
                            {
                                StuffBox.Text += $"Error loading audio codec: finding the decoder for codec ID {codecContext.codec_id} failed.{Environment.NewLine}";
                                return;
                            }
                            AVCodec codec = Marshal.PtrToStructure<AVCodec>(codecPtr);
                            StuffBox.Text += $"Found an audio stream, id {i}, codec {codec.long_name}.{Environment.NewLine}";
                            options = IntPtr.Zero;
                            openResult = AVCodecInterface.avcodec_open2(stream.codec, codecPtr, ref options);
                            if (openResult != 0)
                            {
                                StuffBox.Text += $"Error loading audio codec: opening codec failed with {openResult}.{Environment.NewLine}";
                                return;
                            }
                            StuffBox.Text += $"Codec opened, ready to play!{Environment.NewLine}";

                            AVPacket packet = new AVPacket();
                            int bufferSize = AVCodecInterface.AVCODEC_MAX_AUDIO_FRAME_SIZE + AVCodecInterface.AV_INPUT_BUFFER_PADDING_SIZE;
                            IntPtr packetBuffer = Marshal.AllocHGlobal(bufferSize);
                            packet.data = packetBuffer;
                            packet.size = bufferSize;
                            IntPtr packetPtr = Marshal.AllocHGlobal(Marshal.SizeOf<AVPacket>());
                            Marshal.StructureToPtr(packet, packetPtr, true);
                            IntPtr framePtr = AVUtilInterface.av_frame_alloc();

                            int frames = 0;
                            while (AVFormatInterface.av_read_frame(formatContext, packetPtr) == 0)
                            {
                                int readResult = AVCodecInterface.avcodec_send_packet(stream.codec, packetPtr);
                                if (readResult != 0)
                                {
                                    StuffBox.Text += $"Error reading audio packet: {readResult}.{Environment.NewLine}";
                                    return;
                                }
                                readResult = AVCodecInterface.avcodec_receive_frame(stream.codec, framePtr);
                                if (readResult != 0)
                                {
                                    StuffBox.Text += $"Error receiving decoded audio frame: {readResult}.{Environment.NewLine}";
                                    return;
                                }
                                frames++;
                                AVFrame frame = Marshal.PtrToStructure<AVFrame>(framePtr);
                                //StuffBox.Text += $"Finished reading Frame nb_samples: {frame.nb_samples} line size: {frame.linesize[0]}{Environment.NewLine}";
                                //StuffBox.Text += $"Successfully decoded an audio frame.{Environment.NewLine}";
                            }
                            StuffBox.Text += $"Finished decoding audio. Found {frames} frames.{Environment.NewLine}";

                            Marshal.FreeHGlobal(packetPtr);
                            Marshal.FreeHGlobal(packetBuffer);

                            break;
                        }
                    }


                }
                finally
                {
                    if (formatContext != IntPtr.Zero)
                    {
                        AVFormatInterface.avformat_free_context(formatContext);
                    }
                }
            }
        }

    }
}
