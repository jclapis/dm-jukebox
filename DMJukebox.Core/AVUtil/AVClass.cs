using System;
using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate string item_name(IntPtr ctx);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate IntPtr child_next(IntPtr obj, IntPtr prev);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate IntPtr child_class_next(IntPtr prev);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate AVClassCategory get_category(IntPtr ctx);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    internal delegate int query_ranges(ref IntPtr ranges, IntPtr obj, string key, int flags);

    /// <summary>
    /// Describe the class of an AVClass context structure. That is an
    /// arbitrary struct of which the first field is a pointer to an
    /// AVClass struct (e.g. AVCodecContext, AVFormatContext etc.).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct AVClass
    {
        /// <summary>
        /// The name of the class; usually it is the same name as the
        /// context structure type to which the AVClass is associated.
        /// </summary>
        public string class_name;

        /// <summary>
        /// A pointer to a function which returns the name of a context
        /// instance ctx associated with the class.
        /// </summary>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public item_name item_name;

        /// <summary>
        /// a pointer to the first option specified in the class if any or NULL
        /// @see av_set_default_options()
        /// </summary>
        public IntPtr option;

        /// <summary>
        /// LIBAVUTIL_VERSION with which this structure was created.
        /// This is used to allow fields to be added without requiring major
        /// version bumps everywhere.
        /// </summary>
        public int version;

        /// <summary>
        /// Offset in the structure where log_level_offset is stored.
        /// 0 means there is no such variable
        /// </summary>
        public int log_level_offset_offset;

        /// <summary>
        /// Offset in the structure where a pointer to the parent context for
        /// logging is stored.For example a decoder could pass its AVCodecContext
        /// to eval as such a parent context, which an av_log() implementation
        /// could then leverage to display the parent context.
        /// The offset can be NULL.
        /// </summary>
        public int parent_log_context_offset;

        /// <summary>
        /// Return next AVOptions-enabled child or NULL
        /// </summary>
        public child_next child_next;

        /// <summary>
        /// Return an AVClass corresponding to the next potential
        /// AVOptions-enabled child.
        /// The difference between child_next and this is that
        /// child_next iterates over _already existing_ objects, while
        /// child_class_next iterates over _all possible_ children.
        /// </summary>
        public child_class_next child_class_next;

        /// <summary>
        /// Category used for visualization (like color)
        /// This is only set if the category is equal for all objects using this class.
        /// available since version(51 << 16 | 56 << 8 | 100)
        /// </summary>
        public AVClassCategory category;

        /// <summary>
        /// Callback to return the category.
        /// available since version(51 << 16 | 59 << 8 | 100)
        /// </summary>
        public get_category get_category;

        /// <summary>
        /// Callback to return the supported/allowed ranges.
        /// available since version (52.12)
        /// </summary>
        public query_ranges query_ranges;
    }
}
