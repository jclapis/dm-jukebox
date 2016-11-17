using System;

namespace DMJukebox.Interop
{
    [Flags]
    internal enum SLICE_FLAG
    {
        /// <summary>
        /// draw_horiz_band() is called in coded order instead of display
        /// </summary>
        SLICE_FLAG_CODED_ORDER = 0x1,

        /// <summary>
        /// allow draw_horiz_band() with field slices (MPEG-2 field pics)
        /// </summary>
        SLICE_FLAG_ALLOW_FIELD = 0x2,

        /// <summary>
        /// allow draw_horiz_band() with 1 component at a time (SVQ1)
        /// </summary>
        SLICE_FLAG_ALLOW_PLANE = 0x4
    }
}
