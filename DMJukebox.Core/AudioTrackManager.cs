/*
 * Copyright (c) 2016 Joe Clapis.
 */

using DMJukebox.Interop;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DMJukebox
{
    /// <summary>
    /// AudioTrackManager is the main public interface for DMJukebox's core. This is what you use to
    /// create AudioTracks. Internally this is what runs all of the playback logic.
    /// </summary>
    public class AudioTrackManager
    {
        private readonly object ActiveTrackLock;

        private readonly object StopLock;

        private readonly AutoResetEvent ActiveTrackWaiter;

        private bool _IsClosing;

        private readonly Dictionary<string, AudioTrack> Tracks;

        private readonly HashSet<AudioTrack> ActiveTracks;

        private readonly LocalSoundPlayer LocalPlayer;

        private Task PlayTask;


        private PlaybackMode _PlaybackMode;


        internal const int NumberOfPlaybackSamplesPerFrame = 480;

        private readonly float[] PlaybackBuffer;

        private static readonly AudioTrackManager Instance;

        private bool IsClosing
        {
            get
            {
                lock (StopLock)
                {
                    return _IsClosing;
                }
            }

            set
            {
                lock (StopLock)
                {
                    _IsClosing = value;
                }
            }
        }
        public PlaybackMode PlaybackMode { get; set; }

        static AudioTrackManager()
        {
            AVFormatInterop.av_register_all();
        }

        public AudioTrackManager()
        {
            ActiveTrackWaiter = new AutoResetEvent(false);
            ActiveTrackLock = new object();
            StopLock = new object();
            Tracks = new Dictionary<string, AudioTrack>();
            ActiveTracks = new HashSet<AudioTrack>();
            LocalPlayer = new LocalSoundPlayer();
            PlaybackBuffer = new float[NumberOfPlaybackSamplesPerFrame * 2];
            IsClosing = false;
            PlayTask = Task.Run((Action)PlaybackLoop);
        }

        public void Close()
        {
            StopAllTracks();
            IsClosing = true;
            if (PlayTask != null && PlayTask.Status == TaskStatus.Running)
            {
                PlayTask.Wait(2000);
            }
        }

        public void StopAllTracks()
        {
            lock(ActiveTrackLock)
            {
                foreach (AudioTrack track in ActiveTracks)
                {
                    track.Reset();
                }
                ActiveTracks.Clear();
            }
        }

        public AudioTrack CreateTrack(string FilePath)
        {
            if (Tracks.ContainsKey(FilePath))
            {
                throw new Exception($"A track has already been added for file \"{FilePath}\"");
            }
            lock (ActiveTrackLock)
            {
                AudioTrack stream = new AudioTrack(this, FilePath);
                Tracks.Add(FilePath, stream);
                return stream;
            }
        }

        public void RemoveTrack(AudioTrack Track)
        {
            Tracks.Remove(Track.Info.Path);
            ActiveTracks.Remove(Track);
            Track.Dispose();
            // TODO: Stop playback
        }

        internal void AddTrackToPlaybackList(AudioTrack Track)
        {
            lock (ActiveTrackLock)
            {
                if (ActiveTracks.Contains(Track))
                {
                    return;
                }
                ActiveTracks.Add(Track);
                ActiveTrackWaiter.Set();
            }
        }

        internal void RemoveTrackFromPlaybackList(AudioTrack Track)
        {
            lock(ActiveTrackLock)
            {
                ActiveTracks.Remove(Track);
                Track.Reset();
            }
        }

        private void PlaybackLoop()
        {
            while (!IsClosing)
            {
                List<AudioTrack> endedTracks = null;
                int maxSamplesReceived = 0; // Keep track of the largest number of samples received during this loop
                bool isFirstStream = true;
                if (ActiveTracks.Count == 0)
                {
                    ActiveTrackWaiter.WaitOne();
                    LocalPlayer.Start();
                }

                lock (ActiveTrackLock)
                {
                    foreach (AudioTrack track in ActiveTracks)
                    {
                        // Start off by figuring out if there's enough data left in the stream's buffer to read right away,
                        // or if we have to process a new frame from it.
                        bool trackEnded = false;
                        int availableData = track.AvailableData;

                        // Read new frames until we have enough data to work with.
                        while (availableData < NumberOfPlaybackSamplesPerFrame)
                        {
                            // Not enough data left, we have to decode a new frame from the stream.
                            trackEnded = !track.ProcessNextFrame();
                            if (trackEnded)
                            {
                                // We hit the end of the file, handle the leftovers and then remove the stream.
                                track.WriteDataIntoPlaybackBuffer(PlaybackBuffer, availableData, isFirstStream);
                                maxSamplesReceived = Math.Max(maxSamplesReceived, availableData);
                                if (endedTracks == null)
                                {
                                    endedTracks = new List<AudioTrack>();
                                }
                                endedTracks.Add(track);

                                // If this is the first stream, we have to clear the remaining data from the buffer so the data from the
                                // previous loop doesn't leak into this one.
                                if (isFirstStream)
                                {
                                    Array.Clear(PlaybackBuffer, availableData * 2, (NumberOfPlaybackSamplesPerFrame - availableData) * 2);
                                }
                                break;
                            }
                            availableData = track.AvailableData;
                        }

                        // If we hit the end of the stream above, there's nothing left to do.
                        if (trackEnded)
                        {
                            isFirstStream = false;
                            continue;
                        }

                        // Otherwise, merge the new data into the buffers!
                        track.WriteDataIntoPlaybackBuffer(PlaybackBuffer, NumberOfPlaybackSamplesPerFrame, isFirstStream);
                        maxSamplesReceived = Math.Max(maxSamplesReceived, NumberOfPlaybackSamplesPerFrame);
                        isFirstStream = false;
                    }
                }

                // Now the merge buffers have the aggregated sound data from all of the streams, with volume control already done,
                // so all that's left to do is send the data off to the output.
                LocalPlayer.AddPlaybackData(PlaybackBuffer, maxSamplesReceived);

                // Remove all of the tracks that ended during this iteration
                if (endedTracks != null)
                {
                    lock (ActiveTrackLock)
                    {
                        foreach (AudioTrack track in endedTracks)
                        {
                            ActiveTracks.Remove(track);
                            track.Reset();
                        }

                        // Clean up if playback is done.
                        if (ActiveTracks.Count == 0)
                        {
                            LocalPlayer.Stop();
                        }
                    }
                }
            }
        }

    }
}
