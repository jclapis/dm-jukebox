using DiscordJukebox.Interop;
using System;
using System.Runtime.InteropServices;

namespace DiscordJukebox
{
    internal class AudioFrame // : IDisposable
    {
        public float[] LeftChannel { get; }

        public float[] RightChannel { get; }

        public AudioFrame(float[] LeftChannel, float[] RightChannel)
        {
            this.LeftChannel = LeftChannel;
            this.RightChannel = RightChannel;
        }
        
    }
}
