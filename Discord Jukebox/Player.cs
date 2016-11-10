using DiscordJukebox.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordJukebox
{
    internal class Player
    {
        private readonly object StreamLock;

        private readonly object StopLock;

        private bool _IsStopping;

        private readonly List<MediaFile> Files;

        private Task PlayTask;

        private bool IsStopping
        {
            get
            {
                lock(StopLock)
                {
                    return _IsStopping;
                }
            }

            set
            {
                lock(StopLock)
                {
                    _IsStopping = value;
                }
            }
        }

        public Player()
        {
            StreamLock = new object();
            StopLock = new object();
            Files = new List<MediaFile>();
        }

        public void Start()
        {
            IsStopping = false;
            PlayTask = Task.Run((Action)Run);
        }

        public void AddFile(MediaFile File)
        {
            lock(StreamLock)
            {
                Files.Add(File);
            }
        }

        public void Stop()
        {
            IsStopping = true;
            if(PlayTask != null && PlayTask.Status == TaskStatus.Running)
            {
                PlayTask.Wait(2000);
            }
        }

        private void Run()
        {
            while(!IsStopping)
            {
                lock(StreamLock)
                {
                    foreach(MediaFile file in Files)
                    {
                        AVFrame frame = file.GetNextFrame();
                        StringBuilder builder = new StringBuilder();
                        builder.Append("Read frame. ");

                        for (int i = 0; i < frame.channels; i++)
                        {
                            IntPtr channel = frame.extended_data + i;
                            byte[] buffer = new byte[frame.linesize[0]];
                            Marshal.Copy(channel, buffer, 0, buffer.Length);
                            builder.Append($"Channel {i}: {BitConverter.ToString(buffer)}. ");
                        }

                        System.Diagnostics.Debug.WriteLine(builder.ToString());
                        Thread.Sleep(100);
                    }
                }
            }
        }

    }
}
