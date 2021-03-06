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

using DMJukebox.Interop;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DMJukebox.Discord.Voice
{
    /// <summary>
    /// This is the class that handles playing audio to Discord via its encrypted
    /// UDP connection. If you want to know how to actually play stuff once the
    /// connection has been established, this is where you want to look.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://discordapp.com/developers/docs/topics/voice-connections.
    /// </remarks>
    internal class VoiceUdpConnection : IDisposable
    {
        /// <summary>
        /// This is the amount of overhead, in bytes, that libsodium's encryption
        /// process adds to the original payload.
        /// </summary>
        private const int EncryptionOverhead = 16;

        /// <summary>
        /// This buffer represents a single frame of silence after it's been
        /// encoded in Opus. Discord requires that we send this five times once
        /// audio has been stopped, to clear Opus's buffer out on the remote side.
        /// </summary>
        private static readonly byte[] OpusSilenceFrame = { 0xF8, 0xFF, 0xFE };

        /// <summary>
        /// The UDP client that connects to Discord's UDP server and
        /// sends audio data to it
        /// </summary>
        private readonly UdpClient Client;

        /// <summary>
        /// The server endpoint of the Discord UDP connection
        /// </summary>
        private readonly IPEndPoint DiscordEndpoint;

        /// <summary>
        /// The synchronization source provided by the server as a key
        /// part of the audio packet header
        /// </summary>
        private readonly uint SynchronizationSource;

        /// <summary>
        /// The sequence number of the next audio packet to send
        /// </summary>
        private ushort Sequence;

        /// <summary>
        /// The timestamp of the next audio packet (the time in
        /// milliseconds, offset from the beginning of playback,
        /// that this packet should be played)
        /// </summary>
        private uint Timestamp;

        /// <summary>
        /// This is the buffer that holds the fully processed 
        /// audio data to send. It has to be managed because the
        /// UDP client needs to be able to access it.
        /// </summary>
        private readonly byte[] SendBuffer;

        /// <summary>
        /// (byte*) This is the nonce buffer, which is part of the
        /// encryption process.
        /// </summary>
        private IntPtr NonceBufferPtr;

        /// <summary>
        /// (byte*) This buffer holds the secret key used for encryption
        /// </summary>
        private IntPtr SecretKeyPtr;

        /// <summary>
        /// This is the Opus encoder context
        /// </summary>
        private IntPtr OpusEncoderPtr;

        /// <summary>
        /// This timer is used to keep track of when to send the next
        /// audio frame (it has to be sent at the right time, or else it
        /// will end up jumping ahead / behind and make the audio sound
        /// skippy or laggy).
        /// </summary>
        private readonly Stopwatch Timer;

        /// <summary>
        /// The number of stopwatch ticks in a single frame of audio data
        /// </summary>
        private readonly long TicksPerFrame;

        /// <summary>
        /// The time, in stopwatch ticks, to send the next audio frame
        /// </summary>
        private long TimeOfNextSend;

        /// <summary>
        /// This buffer stores incoming data and holds onto it until
        /// it's ready for playback
        /// </summary>
        private readonly DiscordPlaybackBuffer PlaybackBuffer;

        /// <summary>
        /// This is the task that encodes and sends audio to Discord.
        /// </summary>
        private Task PlaybackTask;

        /// <summary>
        /// This is a synchronization object used to stop the playback
        /// task in a thread-safe manner.
        /// </summary>
        private readonly object StopLock;

        /// <summary>
        /// This flag tells the playback task when it's time to stop.
        /// </summary>
        private bool IsStopping;

        /// <summary>
        /// This sets the secret key used for encrypting audio data.
        /// </summary>
        public byte[] SecretKey
        {
            set
            {
                if (SecretKeyPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(SecretKeyPtr);
                }
                SecretKeyPtr = Marshal.AllocHGlobal(value.Length);
                Marshal.Copy(value, 0, SecretKeyPtr, value.Length);
            }
        }

        /// <summary>
        /// Creates a new VoiceUdpConnection instance.
        /// </summary>
        /// <param name="ServerAddress">The hostname or IP address of the Discord voice server</param>
        /// <param name="Port">The UDP port of the Discord server to connect to</param>
        /// <param name="SynchronizationSource">The sync source provided by the voice server
        /// for validating audio data is being sent from the authenticated source (our client)</param>
        unsafe public VoiceUdpConnection(string ServerAddress, int Port, uint SynchronizationSource)
        {
            // Get the endpoint for the server from its address and port
            this.SynchronizationSource = SynchronizationSource;
            StopLock = new object();
            IPAddress[] addresses = Dns.GetHostAddressesAsync(ServerAddress).Result;
            if (addresses.Length == 0)
            {
                throw new Exception($"Couldn't get an IP address for {ServerAddress}");
            }
            IPAddress address = addresses[0];
            DiscordEndpoint = new IPEndPoint(address, Port);

            // Set up the local UDP client
            IPEndPoint localEndpoint = new IPEndPoint(IPAddress.Any, 0);
            Client = new UdpClient(localEndpoint);

            // Set up some of the buffers
            SendBuffer = new byte[4096 + 12 + EncryptionOverhead];
            NonceBufferPtr = Marshal.AllocHGlobal(24);
            Marshal.Copy(SendBuffer, 0, NonceBufferPtr, 24); // Zero out the nonce buffer
            SendBuffer[0] = 0x80;
            SendBuffer[1] = 0x78;
            byte* ssrcPointer = (byte*)&SynchronizationSource;
            SendBuffer[8] = ssrcPointer[3];
            SendBuffer[9] = ssrcPointer[2];
            SendBuffer[10] = ssrcPointer[1];
            SendBuffer[11] = ssrcPointer[0];
            PlaybackBuffer = new DiscordPlaybackBuffer(JukeboxCore.NumberOfSamplesInPlaybackBuffer * 20);

            // Set up Opus
            OpusErrorCode encoderCreationResult;
            OpusEncoderPtr = OpusInterop.opus_encoder_create(OpusSampleRate._48000, OpusChannelCount._2, OPUS_APPLICATION.OPUS_APPLICATION_AUDIO, out encoderCreationResult);
            if (encoderCreationResult != OpusErrorCode.OPUS_OK)
            {
                throw new Exception($"Creating Opus encoder failed: {encoderCreationResult}");
            }

            // Set up the timer
            Timer = new Stopwatch();
            double ticksPerMillisecond = Stopwatch.Frequency / 1000.0;
            double millisecondsPerFrame = JukeboxCore.NumberOfSamplesInPlaybackBuffer / 48.0; // Samples per frame / 48000 Hz * 1000 ms/s
            TicksPerFrame = (long)(ticksPerMillisecond * millisecondsPerFrame);
        }

        /// <summary>
        /// Figures out what our local address is, so this can be sent to the Discord
        /// server during the handshake
        /// </summary>
        /// <returns>The endpoint representing our local UDP client</returns>
        unsafe public IPEndPoint DiscoverAddress()
        {
            // The payload for this message is a 70 byte message. The first 4
            // bytes are the sync source, the rest is empty.
            byte[] payload = new byte[70];
            fixed (byte* bufferPointer = payload)
            {
                uint* castedPointer = (uint*)bufferPointer;
                *castedPointer = SynchronizationSource;
            }
            Client.SendAsync(payload, 70, DiscordEndpoint).Wait();

            // The response is also a 70 byte packet. It has our address as a
            // null-terminated UTF8 string in the first 68 bytes, and our local port
            // in the remaining 2 bytes.
            UdpReceiveResult response = Client.ReceiveAsync().Result;
            byte[] responseBuffer = response.Buffer;
            string localAddressString = Encoding.UTF8.GetString(responseBuffer, 4, 64);
            localAddressString = localAddressString.Trim('\0');

            // Get the port with some type punning nonsense, because I have an irrational
            // dislike for bit shifting each byte one at a time.
            ushort port;
            fixed (byte* responsePointer = &responseBuffer[68])
            {
                ushort* castedPointer = (ushort*)(responsePointer);
                port = *castedPointer;
            }

            // Assemble it all into our local address!
            IPAddress localAddress;
            if (!IPAddress.TryParse(localAddressString, out localAddress))
            {
                throw new Exception($"Error parsing received local address: {localAddressString} is not a valid IP.");
            }
            return new IPEndPoint(localAddress, port);
        }

        /// <summary>
        /// Begins transmission of audio data to Discord.
        /// </summary>
        public void StartSending()
        {
            IsStopping = false;
            Timer.Start();
            PlaybackTask = Task.Run((Action)PlayAudioLoop);
        }

        /// <summary>
        /// Stops sending audio data to Discord.
        /// </summary>
        public void StopSending()
        {
            lock (StopLock)
            {
                IsStopping = true;
            }
            // Unblock the playback thread if it's stuck waiting for new audio frames
            PlaybackBuffer.ReleasePlaybackWaiter();
            if(PlaybackTask != null)
            {
                PlaybackTask.Wait();
            }

            // Discord requires us to send 5 silence frames once playback stops
            // TODO: Should I just make this a global unmanaged buffer like the others?
            IntPtr silenceBuffer = Marshal.AllocHGlobal(OpusSilenceFrame.Length);
            Marshal.Copy(OpusSilenceFrame, 0, silenceBuffer, OpusSilenceFrame.Length);
            for(int i = 0; i < 5; i++)
            {
                PrepareAndSendPacketToDiscord(silenceBuffer, OpusSilenceFrame.Length);
            }
            Marshal.FreeHGlobal(silenceBuffer);

            // Once all that's out of the way, we can reset everything
            Timer.Stop();
            Timer.Reset();
            TimeOfNextSend = 0;
            Timestamp = 0;
            Sequence = 0;
            PlaybackBuffer.Reset();
        }

        /// <summary>
        /// This is the body of the <see cref="PlaybackTask"/> which gets raw playback audio from the mixing thread,
        /// encodes it with Opus, and sends it to Discord.
        /// </summary>
        private void PlayAudioLoop()
        {
            int outputBufferSize = 4096; // 4096 is just a crazy high upper bound, it'll never get this big
            IntPtr rawPlaybackAudioBuffer = Marshal.AllocHGlobal(JukeboxCore.NumberOfSamplesInPlaybackBuffer * 2 * sizeof(float));
            IntPtr opusEncodedAudioBuffer = Marshal.AllocHGlobal(outputBufferSize); 
            try
            {
                while (true)
                {
                    lock (StopLock)
                    {
                        if (IsStopping)
                        {
                            return;
                        }
                    }

                    // Get playback data from the buffer first
                    PlaybackBuffer.WritePlaybackDataToAudioBuffer(rawPlaybackAudioBuffer, JukeboxCore.NumberOfSamplesInPlaybackBuffer);
                    if (IsStopping)
                    {
                        return;
                    }

                    // Encode the audio with Opus, then process it and send it to Discord
                    int encodedDataSize = EncodeRawAudioWithOpus(rawPlaybackAudioBuffer, opusEncodedAudioBuffer, outputBufferSize);
                    PrepareAndSendPacketToDiscord(opusEncodedAudioBuffer, encodedDataSize);
                }
            }
            finally
            {
                if(rawPlaybackAudioBuffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(rawPlaybackAudioBuffer);
                }
                if(opusEncodedAudioBuffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(opusEncodedAudioBuffer);
                }
            }
        }

        /// <summary>
        /// This encodes a frame of raw audio with Opus.
        /// </summary>
        /// <param name="PlaybackAudio">The audio to encode</param>
        /// <returns>The number of bytes that Opus wrote to the output buffer</returns>
        private int EncodeRawAudioWithOpus(IntPtr RawInputBuffer, IntPtr EncodedOutputBuffer, int OutputBufferSize)
        {
            int encodedDataSize = OpusInterop.opus_encode_float(
                OpusEncoderPtr, RawInputBuffer, JukeboxCore.NumberOfSamplesInPlaybackBuffer, EncodedOutputBuffer, OutputBufferSize);
            if (encodedDataSize < 0)
            {
                OpusErrorCode error = (OpusErrorCode)encodedDataSize;
                System.Diagnostics.Debug.WriteLine($"Failed to encode Opus data: {error}");
                return -1;
            }
            return encodedDataSize;
        }

        /// <summary>
        /// Here it is! The main method that prepares and sends audio data! Every
        /// line of code in the entire DMJukebox.Core.Discord folder is all there
        /// just to support this one function.
        /// </summary>
        unsafe private void PrepareAndSendPacketToDiscord(IntPtr OpusEncodedAudioBuffer, int EncodedDataSize)
        {
            // Write the sequence number in big-endian format
            ushort sequenceNumber = Sequence;
            byte* sequencePtr = (byte*)&sequenceNumber;
            SendBuffer[2] = sequencePtr[1];
            SendBuffer[3] = sequencePtr[0];

            // Write the timestamp in big-endian format
            uint timestamp = Timestamp;
            byte* timestampPtr = (byte*)&timestamp;
            SendBuffer[4] = timestampPtr[3];
            SendBuffer[5] = timestampPtr[2];
            SendBuffer[6] = timestampPtr[1];
            SendBuffer[7] = timestampPtr[0];

            // Update the nonce
            Marshal.Copy(SendBuffer, 0, NonceBufferPtr, 12);

            // Encrypt the packet
            fixed (byte* sendBufferPointer = &SendBuffer[12])
            {
                int encryptionResult = SodiumInterop.crypto_secretbox_easy((IntPtr)sendBufferPointer, OpusEncodedAudioBuffer, (ulong)EncodedDataSize, NonceBufferPtr, SecretKeyPtr);
                if (encryptionResult != 0)
                {
                    System.Diagnostics.Debug.WriteLine($"Encrypting voice data failed with code {encryptionResult}.");
                    return;
                }
            }

            // Check to see if we need to wait before sending the packet, or if we're overdue and
            // have to send it right now. 
            long ticksUntilNextSend = TimeOfNextSend - Timer.ElapsedTicks;
            if (ticksUntilNextSend > 0)
            {
                Thread.Sleep(TimeSpan.FromTicks(ticksUntilNextSend));
                //System.Diagnostics.Debug.WriteLine($"Voice delaying for {ticksUntilNextSend} ticks.");
            }
            else
            {
                //Debug.WriteLine($"Voice overdue by {ticksUntilNextSend} ticks!!");
            }

            // Send the data off to Discord for playback.
            int sendPayloadSize = EncodedDataSize + 12 + EncryptionOverhead;
            Task sendTask = Client.SendAsync(SendBuffer, sendPayloadSize, DiscordEndpoint);

            // Update the sequence number and timestamp
            if (Sequence == ushort.MaxValue)
            {
                Sequence = 0;
            }
            else
            {
                Sequence++;
            }
            Timestamp = unchecked(Timestamp + JukeboxCore.NumberOfSamplesInPlaybackBuffer); // TODO: clean this up

            // Finally, set the time that the next frame needs to be sent.
            TimeOfNextSend += TicksPerFrame;
        }

        /// <summary>
        /// Adds raw audio data to be played over Discord.
        /// </summary>
        /// <param name="PlaybackData">The interleaved, 48 kHz stereo audio to play</param>
        /// <param name="NumberOfSamplesToWrite">The number of samples to write from
        /// the playback buffer to Discord</param>
        public void AddPlaybackData(float[] PlaybackData, int NumberOfSamplesToWrite)
        {
            PlaybackBuffer.AddPlaybackData(PlaybackData, NumberOfSamplesToWrite);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(PlaybackTask != null && PlaybackTask.Status == TaskStatus.Running)
                    {
                        StopSending();
                    }
                    Client.Dispose();
                }

                if(NonceBufferPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(NonceBufferPtr);
                    NonceBufferPtr = IntPtr.Zero;
                }
                if(SecretKeyPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(SecretKeyPtr);
                    SecretKeyPtr = IntPtr.Zero;
                }
                if(OpusEncoderPtr != IntPtr.Zero)
                {
                    OpusInterop.opus_encoder_destroy(OpusEncoderPtr);
                    OpusEncoderPtr = IntPtr.Zero;
                }

                disposedValue = true;
            }
        }
        
        ~VoiceUdpConnection()
        {
            Dispose(false);
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
