/* ===================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * =================================================== */

using DMJukebox.Discord;
using DMJukebox.Interop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DMJukebox
{
    /// <summary>
    /// JukeboxCore is the main interface for DMJukebox's core. This is what you use to
    /// create <see cref="AudioTrack"/>s, connect to Discord, and initialize things.
    /// </summary>
    public class JukeboxCore : IDisposable
    {
        /// <summary>
        /// A synchronization object for adding or removing tracks
        /// from the <see cref="ActiveTracks"/> collection.
        /// </summary>
        private readonly object ActiveTrackLock;

        /// <summary>
        /// This is a synchronization object that will block the
        /// <see cref="PlayTask"/>, leaving it idle, until at least
        /// one track has started playback.
        /// </summary>
        private readonly AutoResetEvent ActiveTrackWaiter;

        /// <summary>
        /// A list of all of the tracks loaded into the system
        /// </summary>
        private readonly Dictionary<string, AudioTrack> Tracks;

        /// <summary>
        /// A list of tracks that are actively playing audio
        /// </summary>
        private readonly HashSet<AudioTrack> ActiveTracks;

        /// <summary>
        /// The player used for playing audio to the local speakers
        /// </summary>
        private readonly LocalSoundPlayer LocalPlayer;

        /// <summary>
        /// The client used for sending data to Discord
        /// </summary>
        private readonly DiscordClient Discord;

        /// <summary>
        /// This task runs the actual audio processing and playback
        /// code.
        /// </summary>
        private Task PlayTask;

        /// <summary>
        /// This is the name of the configuration file to save
        /// for serializing the persistent program configuration.
        /// </summary>
        private const string ConfigurationFile = "Jukebox.cfg";

        /// <summary>
        /// This little buffer is the heart of the entire program.
        /// It stores all of the data from each actively playing track,
        /// merged together into a single aggregate audio frame and
        /// adjusted for their individual volume settings. Thus, this
        /// buffer stores the data that's ready for playback.
        /// </summary>
        private readonly float[] PlaybackBuffer;

        /// <summary>
        /// This is a synchronization object used to read and write
        /// the <see cref="IsClosing"/> flag in a thread-safe manner
        /// </summary>
        private readonly object StopLock;
        
        /// <summary>
        /// This is a flag used in <see cref="PlaybackLoop"/> when it
        /// detects that there are no more active tracks to play. If
        /// there were some in the previous iteration, it needs to tell
        /// the playback endpoint to stop.
        /// </summary>
        private bool WasPreviouslyPlaying;

        /// <summary>
        /// This is just a backing field for the <see cref="IsClosing"/>
        /// property.
        /// </summary>
        private bool _IsClosing;

        /// <summary>
        /// A flag that the <see cref="PlayTask"/> uses to see if
        /// it needs to stop. This is thread-safe so it can be
        /// written and read by the different threads in the program.
        /// </summary>
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

        /// <summary>
        /// The number of samples per channel that are stored within
        /// the audio merge and playback buffer. This gets used as a
        /// base value for some of the other internal classes to use
        /// when determining the size of their own buffers.
        /// </summary>
        internal const int NumberOfSamplesInPlaybackBuffer = 480;

        /// <summary>
        /// The playback mode / the destination for output audio.
        /// </summary>
        public PlaybackMode PlaybackMode { get; set; }

        /// <summary>
        /// The configuration for this manager, used to store
        /// persistent state between runs of the application.
        /// </summary>
        public Configuration Configuration { get; }

        /// <summary>
        /// Initializes FFmpeg when the program starts up / the first time
        /// JukeboxCore gets used
        /// </summary>
        static JukeboxCore()
        {
            AVFormatInterop.av_register_all();
        }

        /// <summary>
        /// Creates a new JukeboxCore instance and initializes the audio
        /// loading / playback system.
        /// </summary>
        public JukeboxCore()
        {
            // Load the config file if it exists, or create an empty one if it doesn't.
            if(File.Exists(ConfigurationFile))
            {
                using (FileStream stream = new FileStream(ConfigurationFile, FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string serializedConfig = reader.ReadToEnd();
                    Configuration = JsonConvert.DeserializeObject<Configuration>(serializedConfig);
                }
            }
            else
            {
                Configuration = new Configuration();
            }

            // Initialization
            ActiveTrackWaiter = new AutoResetEvent(false);
            ActiveTrackLock = new object();
            StopLock = new object();
            Tracks = new Dictionary<string, AudioTrack>();
            ActiveTracks = new HashSet<AudioTrack>();
            LocalPlayer = new LocalSoundPlayer();
            PlaybackBuffer = new float[NumberOfSamplesInPlaybackBuffer * 2];
            IsClosing = false;
            PlayTask = Task.Run((Action)PlaybackLoop);

            // Set up the Discord client with the config settings
            Discord = new DiscordClient();
            if (Configuration.DiscordSettings != null)
            {
                SetDiscordSettings(Configuration.DiscordSettings);
            }
        }

        /// <summary>
        /// Sets the configuration for connecting to Discord.
        /// </summary>
        /// <param name="Settings">The new Discord settings</param>
        public void SetDiscordSettings(DiscordSettings Settings)
        {
            Discord.AuthenticationToken = Settings.BotTokenID;
            Discord.GuildID = Settings.GuildID;
            Discord.ChannelID = Settings.ChannelID;

            Configuration.DiscordSettings = Settings;
            SaveConfig();
        }

        /// <summary>
        /// Saves the configuration out to the config file.
        /// </summary>
        private void SaveConfig()
        {
            using (FileStream stream = new FileStream(ConfigurationFile, FileMode.Create, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                string serializedConfig = JsonConvert.SerializeObject(Configuration);
                writer.Write(serializedConfig);
            }
        }

        /// <summary>
        /// Connects to the Discord voice server for audio playback.
        /// </summary>
        /// <returns>The task running the connection method</returns>
        public async Task ConnectToDiscord()
        {
            await Discord.Connect();
        }

        /// <summary>
        /// Stops all of the currently playing tracks.
        /// </summary>
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

        /// <summary>
        /// Creates a new audio track from the file at the given path.
        /// </summary>
        /// <param name="FilePath">The path of the media file to load</param>
        /// <returns>An audio track representing the first audio stream
        /// contained within the file, ready for playback.</returns>
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

        /// <summary>
        /// Adds a track to the active track list so it will be
        /// played.
        /// </summary>
        /// <param name="Track">The track to add</param>
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

        /// <summary>
        /// Removes a track from the list of active tracks once it has been
        /// stopped or it ends.
        /// </summary>
        /// <param name="Track">The track to remove</param>
        internal void RemoveTrackFromPlaybackList(AudioTrack Track)
        {
            lock(ActiveTrackLock)
            {
                ActiveTracks.Remove(Track);
                Track.Reset();
            }
        }

        /// <summary>
        /// This function is the body of the <see cref="PlayTask"/>. It reads audio from the active tracks,
        /// merges their data together, and sends it out to the output for playback. All of the actual
        /// processing is done in here.
        /// </summary>
        private void PlaybackLoop()
        {
            while (!IsClosing)
            {
                List<AudioTrack> endedTracks = null;
                int maxSamplesReceived = 0; // Keep track of the largest number of samples received during this loop
                bool isFirstStream = true;

                // Wait until there's at least one track being played
                if (ActiveTracks.Count == 0)
                {
                    // Stop the players if they were just playing, because now there's
                    // nothing left to play.
                    if (WasPreviouslyPlaying)
                    {
                        switch (PlaybackMode)
                        {
                            case PlaybackMode.LocalSpeakers:
                                LocalPlayer.Stop();
                                break;

                            case PlaybackMode.Discord:
                                Discord.Stop();
                                break;
                        }
                        WasPreviouslyPlaying = false;
                    }

                    ActiveTrackWaiter.WaitOne();
                    if(IsClosing)
                    {
                        return;
                    }
                    switch(PlaybackMode)
                    {
                        case PlaybackMode.LocalSpeakers:
                            LocalPlayer.Start();
                            break;

                        case PlaybackMode.Discord:
                            Discord.Start();
                            break;
                    }
                    WasPreviouslyPlaying = true;
                }

                // Merge audio data from each of the active tracks into a single playback buffer
                lock (ActiveTrackLock)
                {
                    foreach (AudioTrack track in ActiveTracks)
                    {
                        // Start off by figuring out if there's enough data left in the stream's buffer to read right away,
                        // or if we have to process a new frame from it.
                        bool trackEnded = false;
                        int availableData = track.AvailableData;

                        // Read new frames until we have enough data to work with.
                        while (availableData < NumberOfSamplesInPlaybackBuffer)
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
                                    Array.Clear(PlaybackBuffer, availableData * 2, (NumberOfSamplesInPlaybackBuffer - availableData) * 2);
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
                        track.WriteDataIntoPlaybackBuffer(PlaybackBuffer, NumberOfSamplesInPlaybackBuffer, isFirstStream);
                        maxSamplesReceived = Math.Max(maxSamplesReceived, NumberOfSamplesInPlaybackBuffer);
                        isFirstStream = false;
                    }
                }

                // Now the merge buffer has the aggregated sound data from all of the streams, with volume control already done,
                // so all that's left to do is send the data off to the output.
                switch(PlaybackMode)
                {
                    case PlaybackMode.LocalSpeakers:
                        LocalPlayer.AddPlaybackData(PlaybackBuffer, maxSamplesReceived);
                        break;

                    case PlaybackMode.Discord:
                        Discord.AddPlaybackData(PlaybackBuffer, maxSamplesReceived);
                        break;
                }

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
                            switch(PlaybackMode)
                            {
                                case PlaybackMode.LocalSpeakers:
                                    LocalPlayer.Stop();
                                    break;

                                case PlaybackMode.Discord:
                                    Discord.Stop();
                                    break;
                            }
                        }
                    }
                }

            } // End of the overall while loop
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Shut down the playback task and wait for it to return
                    IsClosing = true;
                    ActiveTrackWaiter.Set();
                    PlayTask.Wait();

                    LocalPlayer.Dispose();
                    Discord.Dispose();
                }

                disposedValue = true;
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
