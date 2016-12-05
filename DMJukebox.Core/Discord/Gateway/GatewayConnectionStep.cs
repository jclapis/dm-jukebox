namespace DMJukebox.Discord.Gateway
{
    internal enum GatewayConnectionStep
    {
        Disconnected,
        WaitingForHello,
        WaitingForReady,
        WaitingForVoiceServerInfo,
        Connected
    }
}
