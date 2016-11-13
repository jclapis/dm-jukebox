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

        private LocalSoundPlayer LocalPlayer;

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
            LocalPlayer = new LocalSoundPlayer();
        }

        public void Start()
        {
            IsStopping = false;
            PlayTask = Task.Run((Action)Run);
            LocalPlayer.Start();
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
            LocalPlayer.Pause();
        }

        private void Run()
        {
            int frames = 0;
            while (!IsStopping)
            {
                lock(StreamLock)
                {
                    foreach(AudioStream stream in Streams)
                    {
                        AudioFrame frame = stream.GetNextFrame();
                        frames++;

                        if(frame == null)
                        {
                            System.Diagnostics.Debug.WriteLine($"Finished decoding, read {frames} frames.");
                            return;
                        }

                        LocalPlayer.AddFrame(frame);
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

        private void PlayAudioToLocalSound()
        {

        }

        private void SetupLocalSound()
        {

        }

    }
}
