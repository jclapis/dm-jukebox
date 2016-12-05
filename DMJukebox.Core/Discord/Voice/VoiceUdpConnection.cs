using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DMJukebox.Discord.Voice
{
    internal class VoiceUdpConnection
    {
        private readonly UdpClient Client;

        private readonly IPEndPoint DiscordEndpoint;

        private readonly uint SynchronizationSource;

        private int Sequence;

        private int Timestamp;

        private byte[] SendBuffer;

        public byte[] SecretKey { get; set; }

        private static readonly byte[] OpusSilenceFrame = { 0xF8, 0xFF, 0xFE };

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
            SendBuffer = new byte[65536];
            SendBuffer[0] = 0x80;
            SendBuffer[1] = 0x78;
            fixed (byte* bufferPointer = &SendBuffer[8])
            {
                uint* castedPointer = (uint*)bufferPointer;
                *castedPointer = SynchronizationSource;
            }
        }

        unsafe public IPEndPoint DiscoverAddress()
        {
            byte[] payload = new byte[70];
            fixed (byte* bufferPointer = &payload[0])
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
            if(!IPAddress.TryParse(localAddressString, out localAddress))
            {
                throw new Exception($"Error parsing received local address: {localAddressString} is not a valid IP.");
            }
            return new IPEndPoint(localAddress, port);
        }

        private byte[] CreateNextPacket()
        {

        }

    }
}
