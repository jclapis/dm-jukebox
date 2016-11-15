using System;
using System.Collections.Generic;
using System.Threading;
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
            int samplesRead = 0;
            while (!IsStopping)
            {
                lock(StreamLock)
                {
                    bool frameReceived = Streams[0].GetNextFrame();
                    if (!frameReceived)
                    {
                        System.Diagnostics.Debug.WriteLine($"Finished decoding, read {frames} frames.");
                        return;
                    }
                    
                    samplesRead += Streams[0].BufferSize;
                    //System.Diagnostics.Debug.WriteLine($"Decoded {Streams[0].BufferSize} samples, writing to local player.");
                    LocalPlayer.WriteData(Streams[0]);
                    frames++;

                    /*if(samplesRead > 4800)
                    {
                        Thread.Sleep(100);
                        samplesRead = 0;
                    }*/
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
