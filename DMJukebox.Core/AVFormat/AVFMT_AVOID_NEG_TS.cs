namespace DMJukebox.Interop
{
    internal enum AVFMT_AVOID_NEG_TS
    {
        /// <summary>
        /// Enabled when required by target format
        /// </summary>
        AUTO = -1,

        /// <summary>
        /// Shift timestamps so they are non negative
        /// </summary>
        MAKE_NON_NEGATIVE = 1,

        /// <summary>
        /// Shift timestamps so that they start at 0
        /// </summary>
        MAKE_ZERO = 2
    }

}
