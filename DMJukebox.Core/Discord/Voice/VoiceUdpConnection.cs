using DMJukebox.Interop;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DMJukebox.Discord.Voice
{
    internal class VoiceUdpConnection
    {
        private readonly UdpClient Client;

        private readonly IPEndPoint DiscordEndpoint;

        private readonly uint SynchronizationSource;

        private ushort Sequence;

        private uint Timestamp;

        private readonly byte[] SendBuffer;

        private readonly IntPtr NonceBufferPtr;

        private readonly IntPtr PlaybackAudio;

        private readonly IntPtr OpusOutputBuffer;

        public byte[] SecretKey
        {
            set
            {
                if(SecretKeyPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(SecretKeyPtr);
                }
                SecretKeyPtr = Marshal.AllocHGlobal(value.Length);
                Marshal.Copy(value, 0, SecretKeyPtr, value.Length);
            }
        }

        private IntPtr SecretKeyPtr;

        private static readonly byte[] OpusSilenceFrame = { 0xF8, 0xFF, 0xFE };

        private IntPtr OpusEncoderPtr;

        private const int EncryptionOverhead = 16;

        private readonly Stopwatch Timer;

        private readonly long TicksPerFrame;

        private long TimeOfNextSend;

        private readonly DiscordPlaybackBuffer PlaybackBuffer;

        private Task PlaybackTask;

        private readonly object StopLock;

        private bool IsStopping;

        unsafe public VoiceUdpConnection(string ServerAddress, int Port, uint SynchronizationSource)
        {
            this.SynchronizationSource = SynchronizationSource;
            StopLock = new object();
            IPAddress[] addresses = Dns.GetHostAddressesAsync(ServerAddress).Result;
            if (addresses.Length == 0)
            {
                throw new Exception($"Couldn't get an IP address for {ServerAddress}");
            }
            IPAddress address = addresses[0];
            DiscordEndpoint = new IPEndPoint(address, Port);

            IPEndPoint localEndpoint = new IPEndPoint(IPAddress.Any, 0);
            Client = new UdpClient(localEndpoint);

            PlaybackAudio = Marshal.AllocHGlobal(AudioTrackManager.NumberOfPlaybackSamplesPerFrame * 2 * sizeof(float));
            OpusOutputBuffer = Marshal.AllocHGlobal(4096);
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

            OpusErrorCode encoderCreationResult;
            OpusEncoderPtr = OpusInterop.opus_encoder_create(OpusSampleRate._48000, OpusChannelCount._2, OPUS_APPLICATION.OPUS_APPLICATION_AUDIO, out encoderCreationResult);
            if (encoderCreationResult != OpusErrorCode.OPUS_OK)
            {
                throw new Exception($"Creating Opus encoder failed: {encoderCreationResult}");
            }
            Timer = new Stopwatch();
            double ticksPerMillisecond = Stopwatch.Frequency / 1000.0;
            double millisecondsPerFrame = AudioTrackManager.NumberOfPlaybackSamplesPerFrame / 48.0; // Samples per frame / 48000 Hz * 1000 ms/s
            TicksPerFrame = (long)(ticksPerMillisecond * millisecondsPerFrame);

            PlaybackBuffer = new DiscordPlaybackBuffer();
        }

        unsafe public IPEndPoint DiscoverAddress()
        {
            byte[] payload = new byte[70];
            fixed (byte* bufferPointer = payload)
            {
                uint* castedPointer = (uint*)bufferPointer;
                *castedPointer = SynchronizationSource;
            }
            Client.SendAsync(payload, 70, DiscordEndpoint).Wait();

            UdpReceiveResult response = Client.ReceiveAsync().Result;
            byte[] responseBuffer = response.Buffer;
            string localAddressString = Encoding.UTF8.GetString(responseBuffer, 4, 64);
            localAddressString = localAddressString.Trim('\0');

            ushort port;
            fixed (byte* responsePointer = &responseBuffer[68])
            {
                ushort* castedPointer = (ushort*)(responsePointer);
                port = *castedPointer;
            }

            IPAddress localAddress;
            if (!IPAddress.TryParse(localAddressString, out localAddress))
            {
                throw new Exception($"Error parsing received local address: {localAddressString} is not a valid IP.");
            }
            return new IPEndPoint(localAddress, port);
        }

        public void StartSending()
        {
            IsStopping = false;
            Timer.Start();
            PlaybackTask = Task.Run((Action)PrepareNextPacket);
        }

        public void StopSending()
        {
            lock(StopLock)
            {
                IsStopping = true;
            }
            PlaybackTask.Wait();
            Timer.Stop();
            Timer.Reset();
            TimeOfNextSend = 0;
            Timestamp = 0;
        }

        unsafe private void PrepareNextPacket()
        {
            while (true)
            {
                lock(StopLock)
                {
                    if(IsStopping)
                    {
                        return;
                    }
                }

                // Get playback data from the buffer first
                PlaybackBuffer.WritePlaybackDataToAudioBuffer(PlaybackAudio, AudioTrackManager.NumberOfPlaybackSamplesPerFrame);

                ushort sequenceNumber = Sequence;

                // Write the sequence number in big-endian format
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

                // Encode the audio with Opus
                int encodedDataSize;
                fixed (byte* sendBufferPointer = &SendBuffer[12])
                {
                    encodedDataSize = OpusInterop.opus_encode_float(OpusEncoderPtr, PlaybackAudio, AudioTrackManager.NumberOfPlaybackSamplesPerFrame, OpusOutputBuffer, 4096);
                    if (encodedDataSize < 0)
                    {
                        OpusErrorCode error = (OpusErrorCode)encodedDataSize;
                        System.Diagnostics.Debug.WriteLine($"Failed to encode Opus data: {error}");
                        return;
                    }
                    int encryptionResult = SodiumInterop.crypto_secretbox_easy((IntPtr)sendBufferPointer, OpusOutputBuffer, (ulong)encodedDataSize, NonceBufferPtr, SecretKeyPtr);
                    if (encryptionResult != 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"Encrypting voice data failed with code {encryptionResult}.");
                        return;
                    }
                }

                int sendPayloadSize = encodedDataSize + 12 + EncryptionOverhead;
                long ticksUntilNextSend = TimeOfNextSend - Timer.ElapsedTicks;
                if (ticksUntilNextSend > 0)
                {
                    // Wait until it's time to send the next frame over.
                    Task.Delay(TimeSpan.FromTicks(ticksUntilNextSend)).Wait();
                    System.Diagnostics.Debug.WriteLine($"Voice delaying for {ticksUntilNextSend} ticks.");
                }
                else
                {
                    Debug.WriteLine($"Voice overdue by {ticksUntilNextSend} ticks!!");
                }
                //System.Diagnostics.Debug.WriteLine($"Sending {sendPayloadSize} bytes of voice data.");
                Task sendTask = Client.SendAsync(SendBuffer, sendPayloadSize, DiscordEndpoint);
                TimeOfNextSend += TicksPerFrame;

                // Update the sequence number and timestamp
                if (Sequence == ushort.MaxValue)
                {
                    Sequence = 0;
                }
                else
                {
                    Sequence++;
                }
                Timestamp = unchecked(Timestamp + AudioTrackManager.NumberOfPlaybackSamplesPerFrame); // TODO: clean this up
            }
        }

        public void AddPlaybackData(float[] PlaybackData, int NumberOfSamplesToWrite)
        {
            PlaybackBuffer.AddPlaybackData(PlaybackData, NumberOfSamplesToWrite);
        }

    }
}
