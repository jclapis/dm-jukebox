using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordJukebox
{
    internal class Player
    {
        private readonly object StreamLock;

        private readonly object StopLock;

        private bool _IsStopping;

        private readonly List<AudioStream> Streams;

        private Task PlayTask;

        public bool PlayToSpeakers { get; set; }

        public bool StreamToDiscord { get; set; }



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
            Streams = new List<AudioStream>();
        }

        public void Start()
        {
            IsStopping = false;
            PlayTask = Task.Run((Action)Run);
        }

        public void AddStream(AudioStream Stream)
        {
            lock(StreamLock)
            {
                Streams.Add(Stream);
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
                    foreach(AudioStream stream in Streams)
                    {
                        AudioFrame frame = stream.GetNextFrame();
                    }
                }
            }
        }

        private void ProcessSingleStream()
        {

        }

        private void ProcessMultipleStreams()
        {

        }

        private void Test()
        {
        }

    }
}
