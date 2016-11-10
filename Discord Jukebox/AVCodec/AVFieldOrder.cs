namespace DiscordJukebox.Interop
{
    internal enum AVFieldOrder
    {
        AV_FIELD_UNKNOWN,
        AV_FIELD_PROGRESSIVE,

        /// <summary>
        /// Top coded_first, top displayed first
        /// </summary>
        AV_FIELD_TT,

        /// <summary>
        /// Bottom coded first, bottom displayed first
        /// </summary>
        AV_FIELD_BB,

        /// <summary>
        /// Top coded first, bottom displayed first
        /// </summary>
        AV_FIELD_TB,

        /// <summary>
        /// Bottom coded first, top displayed first
        /// </summary>
        AV_FIELD_BT
    }
}
