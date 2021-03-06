﻿/* ========================================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * ====================================================================== */

using DMJukebox.Discord.Gateway;
using DMJukebox.Discord.Voice;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DMJukebox.Discord
{
    /// <summary>
    /// This class handles all of the connection and audio delivery code for
    /// talking to the Discord server.
    /// </summary>
    internal class DiscordClient : IDisposable
    {
        /// <summary>
        /// This is the internal implementation for managing a connection to
        /// the Discord gateway server
        /// </summary>
        private readonly GatewayConnection Gateway;

        /// <summary>
        /// This is the internal implementation for managing a connection to
        /// the Discord voice server
        /// </summary>
        private readonly VoiceClient Voice;

        /// <summary>
        /// This is the endpoint for Discord's REST API, which is used to 
        /// get the URI for the gateway websocket server.
        /// </summary>
        private const string DiscordApiUri = "https://discordapp.com/api/";

        /// <summary>
        /// The authentication token for the provided Discord bot account
        /// (basically the password for it)
        /// </summary>
        public string AuthenticationToken
        {
            get
            {
                return Gateway.AuthenticationToken;
            }
            set
            {
                Gateway.AuthenticationToken = value;
            }
        }

        /// <summary>
        /// The ID of the guild that owns the voice channel you
        /// want to connect to
        /// </summary>
        public string GuildID
        {
            get
            {
                return Gateway.GuildID;
            }
            set
            {
                Gateway.GuildID = value;
                Voice.GuildID = value;
            }
        }

        /// <summary>
        /// The ID of the voice channel you want to play sound over
        /// </summary>
        public string ChannelID
        {
            get
            {
                return Gateway.ChannelID;
            }
            set
            {
                Gateway.ChannelID = value;
            }
        }

        /// <summary>
        /// Creates a new DiscordClient instance.
        /// </summary>
        public DiscordClient()
        {
            Gateway = new GatewayConnection();
            Voice = new VoiceClient();
        }

        /// <summary>
        /// Connects to the Discord server so it's ready to play audio.
        /// </summary>
        /// <returns>The task running the method</returns>
        public async Task Connect()
        {
            // Gets the URI of the gateway websocket server
            Uri gatewayRequestUri = new Uri(new Uri(DiscordApiUri), "gateway");
            HttpClient client = new HttpClient();
            string responseBody = await client.GetStringAsync(gatewayRequestUri);
            GetGatewayResponse response = JsonConvert.DeserializeObject<GetGatewayResponse>(responseBody);
            Uri websocketUri = new Uri($"{response.Url}/?encoding=json&v=5");

            // Connect to the gateway, then the voice server, then we're done!
            await Gateway.Connect(websocketUri);
            await Voice.Connect(Gateway.BotUserID, Gateway.VoiceSessionID, Gateway.VoiceSessionToken, Gateway.VoiceServerHostname);
            System.Diagnostics.Debug.WriteLine("Connected to Discord!");
        }

        /// <summary>
        /// Adds audio data to be played back over Discord.
        /// </summary>
        /// <param name="PlaybackData">The audio to be played back. This has to be an interleaved buffer
        /// (left sample, then right sample, then left, then right...) in 48 kHz stereo format.</param>
        /// <param name="NumberOfSamplesToWrite">The number of samples per channel to write from the
        /// playback data buffer to Discord</param>
        public void AddPlaybackData(float[] PlaybackData, int NumberOfSamplesToWrite)
        {
            Voice.AddPlaybackData(PlaybackData, NumberOfSamplesToWrite);
        }

        /// <summary>
        /// Starts the Discord playback thread.
        /// </summary>
        public void Start()
        {
            Voice.Start();
        }

        /// <summary>
        /// Stops the Discord playback thread.
        /// </summary>
        public void Stop()
        {
            Voice.Stop();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Stop();
                    Voice.Dispose();
                    Gateway.Dispose();
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
