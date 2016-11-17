namespace DMJukebox.Interop
{
    internal enum FF_MB_DECISION
    {
        /// <summary>
        /// uses mb_cmp
        /// </summary>
        FF_MB_DECISION_SIMPLE,

        /// <summary>
        /// chooses the one which needs the fewest bits
        /// </summary>
        FF_MB_DECISION_BITS,

        /// <summary>
        /// rate distortion
        /// </summary>
        FF_MB_DECISION_RD
    }
}
