namespace DiscordJukebox.Interop
{
    internal enum AV_PKT_FLAG
    {
        /// <summary>
        /// The packet contains a keyframe
        /// </summary>
        AV_PKT_FLAG_KEY = 0x0001,

        /// <summary>
        /// The packet content is corrupted
        /// </summary>
        AV_PKT_FLAG_CORRUPT = 0x0002
    }
}
