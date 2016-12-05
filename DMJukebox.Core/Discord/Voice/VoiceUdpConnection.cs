using DMJukebox.Interop;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
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

        private readonly byte[] NonceBuffer;

        private readonly float[] AudioBuffer;

        private readonly byte[] OpusOutputBuffer;

        public byte[] SecretKey { get; set; }

        private static readonly byte[] OpusSilenceFrame = { 0xF8, 0xFF, 0xFE };

        private IntPtr OpusEncoderPtr;

        private const int EncryptionOverhead = 16;

        private readonly Stopwatch Timer;

        private readonly long TicksPerFrame;

        private readonly double TicksPerMillisecond;

        private long TimeOfNextSend;

        private long TimeOfLastSend;

        unsafe public VoiceUdpConnection(string ServerAddress, int Port, uint SynchronizationSource)
        {
            this.SynchronizationSource = SynchronizationSource;
            IPAddress[] addresses = Dns.GetHostAddressesAsync(ServerAddress).Result;
            if (addresses.Length == 0)
            {
                throw new Exception($"Couldn't get an IP address for {ServerAddress}");
            }
            IPAddress address = addresses[0];
            DiscordEndpoint = new IPEndPoint(address, Port);

            IPEndPoint localEndpoint = new IPEndPoint(IPAddress.Any, 0);
            Client = new UdpClient(localEndpoint);

            AudioBuffer = new float[AudioTrackManager.NumberOfPlaybackSamplesPerFrame * 2];
            OpusOutputBuffer = new byte[4096];
            SendBuffer = new byte[4096 + 12 + EncryptionOverhead];
            NonceBuffer = new byte[24];
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
            TicksPerMillisecond = Stopwatch.Frequency / 1000.0;
            double millisecondsPerFrame = AudioTrackManager.NumberOfPlaybackSamplesPerFrame / 48.0; // Samples per frame / 48000 Hz * 1000 ms/s
            TicksPerFrame = (long)(TicksPerMillisecond * millisecondsPerFrame);
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
            Timer.Start();
        }

        public void StopSending()
        {
            Timer.Stop();
            Timer.Reset();
            TimeOfLastSend = 0;
            TimeOfNextSend = 0;
            Timestamp = 0;
        }

        unsafe private void PrepareNextPacket()
        {
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
            Buffer.BlockCopy(SendBuffer, 0, NonceBuffer, 0, 12);

            // Encode the audio with Opus
            int encodedDataSize;
            fixed (float* audioPointer = AudioBuffer)
            fixed (byte* opusBufferPointer = OpusOutputBuffer)
            fixed (byte* sendBufferPointer = &SendBuffer[12])
            fixed (byte* noncePointer = NonceBuffer)
            fixed (byte* secretKeyPointer = SecretKey)
            {
                IntPtr input = (IntPtr)audioPointer;
                IntPtr encodedDataPtr = (IntPtr)opusBufferPointer;
                encodedDataSize = OpusInterop.opus_encode_float(OpusEncoderPtr, input, AudioTrackManager.NumberOfPlaybackSamplesPerFrame, encodedDataPtr, OpusOutputBuffer.Length);
                if (encodedDataSize < 0)
                {
                    OpusErrorCode error = (OpusErrorCode)encodedDataSize;
                    System.Diagnostics.Debug.WriteLine($"Failed to encode Opus data: {error}");
                    return;
                }
                int encryptionResult = SodiumInterop.crypto_secretbox_easy((IntPtr)sendBufferPointer, encodedDataPtr, (ulong)encodedDataSize, (IntPtr)noncePointer, (IntPtr)secretKeyPointer);
                if(encryptionResult != 0)
                {
                    System.Diagnostics.Debug.WriteLine($"Encrypting voice data failed with code {encryptionResult}.");
                    return;
                }
            }

            int sendPayloadSize = encodedDataSize + 12 + EncryptionOverhead;
            long ticksSinceLastSend = Timer.ElapsedTicks - TimeOfLastSend;
            long ticksUntilNextSend = TimeOfNextSend - ticksSinceLastSend;
            if(ticksUntilNextSend > 0)
            {
                // Wait until it's time to send the next frame over.
                Task.Delay(TimeSpan.FromTicks(ticksUntilNextSend)).Wait();
            }
            Task sendTask = Client.SendAsync(SendBuffer, sendPayloadSize, DiscordEndpoint);
            TimeOfLastSend = Timer.ElapsedTicks;
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
}
