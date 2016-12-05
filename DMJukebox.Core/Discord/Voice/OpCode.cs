namespace DMJukebox.Discord.Voice
{
    internal enum OpCode
    {
        Identify,
        SelectProtocol,
        Ready,
        Heartbeat,
        SessionDescription,
        Speaking
    }
}
