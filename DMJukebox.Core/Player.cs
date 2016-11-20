using DMJukebox.Interop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMJukebox
{
    public class Player
    {
        private readonly object StreamLock;

        private readonly object StopLock;

        private bool _IsStopping;

        private readonly List<AudioTrack> Streams;

        private Task PlayTask;

        private LocalSoundPlayer LocalPlayer;

        public bool PlayToSpeakers { get; set; }

        public bool StreamToDiscord { get; set; }

        internal const int MergeBufferLength = 480;

        private readonly float[] LeftChannelMergeBuffer;

        private readonly float[] RightChannelMergeBuffer;

        private static readonly Player Instance;

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

        static Player()
        {
            AVFormatInterop.av_register_all();
            Instance = new Player();
        }

        public static Player Create()
        {
            return Instance;
        }

        private Player()
        {
            StreamLock = new object();
            StopLock = new object();
            Streams = new List<AudioTrack>();
            LocalPlayer = new LocalSoundPlayer();
            LeftChannelMergeBuffer = new float[MergeBufferLength];
            RightChannelMergeBuffer = new float[MergeBufferLength];
        }

        public void Start()
        {
            IsStopping = false;
            PlayTask = Task.Run((Action)Run);
            LocalPlayer.Start();
        }

        public void StopAll()
        {
            IsStopping = true;
            if(PlayTask != null && PlayTask.Status == TaskStatus.Running)
            {
                PlayTask.Wait(2000);
            }
            foreach (AudioTrack stream in Streams)
            {
                stream.Stop();
            }
            Array.Clear(LeftChannelMergeBuffer, 0, MergeBufferLength);
            Array.Clear(RightChannelMergeBuffer, 0, MergeBufferLength);
            LocalPlayer.Stop();
            LocalPlayer.ResetBuffer();
        }

        public AudioTrack AddTrack(string Filename)
        {
            lock(StreamLock)
            {
                AudioTrack stream = new AudioTrack(Filename);
                Streams.Add(stream);
                return stream;
            }
        }

        private void Run()
        {
            while (!IsStopping)
            {
                lock(StreamLock)
                {
                    int maxSamplesReceived = 0; // Keep track of the largest number of samples received during this loop
                    for (int i = 0; i < Streams.Count; i++)
                    {
                        // Start off by figuring out if there's enough data left in the stream's buffer to read right away,
                        // or if we have to process a new frame from it.
                        AudioTrack stream = Streams[i];
                        bool streamEnded = false;
                        bool isFirstStream = i == 0;
                        int availableData = stream.AvailableData;

                        // Read new frames until we have enough data to work with.
                        while (availableData < MergeBufferLength)
                        {
                            // Not enough data left, we have to decode a new frame from the stream.
                            bool success = stream.ProcessNextFrame();
                            if(!success)
                            {
                                // We hit the end of the file, handle the leftovers and then remove the stream.
                                stream.WriteDataIntoPlaybackBuffers(LeftChannelMergeBuffer, RightChannelMergeBuffer, availableData, isFirstStream);
                                maxSamplesReceived = Math.Max(maxSamplesReceived, availableData);
                                Streams.RemoveAt(i);
                                streamEnded = true;
                                i--;

                                // If this is the first stream, we have to clear the remaining data from the buffer so the data from the
                                // previous loop doesn't leak into this one.
                                if(isFirstStream)
                                {
                                    Array.Clear(LeftChannelMergeBuffer, availableData, MergeBufferLength - availableData);
                                    Array.Clear(RightChannelMergeBuffer, availableData, MergeBufferLength - availableData);
                                }
                                break;
                            }
                            availableData = stream.AvailableData;
                        }

                        // If we hit the end of the stream above, there's nothing left to do.
                        if(streamEnded)
                        {
                            continue;
                        }

                        // Otherwise, merge the new data into the buffers!
                        stream.WriteDataIntoPlaybackBuffers(LeftChannelMergeBuffer, RightChannelMergeBuffer, MergeBufferLength, isFirstStream);
                        maxSamplesReceived = Math.Max(maxSamplesReceived, MergeBufferLength);
                    }

                    // Now the merge buffers have the aggregated sound data from all of the streams, with volume control already done,
                    // so all that's left to do is send the data off to the output.
                    LocalPlayer.AddPlaybackData(LeftChannelMergeBuffer, RightChannelMergeBuffer, maxSamplesReceived);

                    // Clean up if playback is done.
                    if(Streams.Count == 0)
                    {
                        IsStopping = true;
                        LocalPlayer.Stop();
                        System.Diagnostics.Debug.WriteLine("Finished playback, standing by.");
                        return;
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
