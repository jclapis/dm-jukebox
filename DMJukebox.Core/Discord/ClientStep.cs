using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMJukebox.Discord
{
    internal enum ClientStep
    {
        Disconnected,
        WaitingForHello,
        WaitingForReady,
        WaitingForVoiceServerInfo,
        WaitingForVoiceReady,
        WaitingForVoiceSession,
        Connected
    }
}
