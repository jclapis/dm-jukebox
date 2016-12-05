using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMJukebox.Discord.Voice
{
    internal enum VoiceConnectionStep
    {
        Disconnected,
        WaitForReady,
        WaitForSessionDescription
    }
}
