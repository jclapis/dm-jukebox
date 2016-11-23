using System;
using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    internal static class OpusInterop
    {
        /// <summary>
        /// The DLL for Windows
        /// </summary>
        private const string WindowsOpusLibrary = "opus.dll";

        /// <summary>
        /// The SO for Linux
        /// </summary>
        private const string LinuxOpusLibrary = "opus.so";

        /// <summary>
        /// The Dylib for OSX
        /// </summary>
        private const string MacOpusLibrary = "opus.dylib";

        // These regions contain the DllImport function definitions for each OS. Since we can't really set
        // the path of DllImport dynamically (and loading them dynamically using LoadLibrary / dlopen is complicated
        // to manage cross-platform), we have to pre-define them based on the names of the libraries above.

        #region Windows Functions

        [DllImport(WindowsOpusLibrary, EntryPoint = nameof(opus_encoder_create), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr opus_encoder_create_windows(OpusSampleRate Fs, OpusChannelCount channels, OPUS_APPLICATION application, out OpusErrorCode error);

        [DllImport(WindowsOpusLibrary, EntryPoint = nameof(opus_encoder_destroy), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void opus_encoder_destroy_windows(IntPtr st);

        [DllImport(WindowsOpusLibrary, EntryPoint = nameof(opus_encode_float), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int opus_encode_float_windows(IntPtr st, IntPtr pcm, int frame_size, IntPtr data, int max_data_bytes);

        #endregion

        #region Linux Functions

        [DllImport(LinuxOpusLibrary, EntryPoint = nameof(opus_encoder_create), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr opus_encoder_create_linux(OpusSampleRate Fs, OpusChannelCount channels, OPUS_APPLICATION application, out OpusErrorCode error);

        [DllImport(LinuxOpusLibrary, EntryPoint = nameof(opus_encoder_destroy), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void opus_encoder_destroy_linux(IntPtr st);

        [DllImport(LinuxOpusLibrary, EntryPoint = nameof(opus_encode_float), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int opus_encode_float_linux(IntPtr st, IntPtr pcm, int frame_size, IntPtr data, int max_data_bytes);

        #endregion

        #region OSX Functions

        [DllImport(MacOpusLibrary, EntryPoint = nameof(opus_encoder_create), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr opus_encoder_create_osx(OpusSampleRate Fs, OpusChannelCount channels, OPUS_APPLICATION application, out OpusErrorCode error);

        [DllImport(MacOpusLibrary, EntryPoint = nameof(opus_encoder_destroy), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void opus_encoder_destroy_osx(IntPtr st);

        [DllImport(MacOpusLibrary, EntryPoint = nameof(opus_encode_float), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int opus_encode_float_osx(IntPtr st, IntPtr pcm, int frame_size, IntPtr data, int max_data_bytes);

        #endregion

        #region Delegates and Platform-Dependent Loading

        // These delegates all represent the function signatures for the libopus methods I need to call.

        private delegate IntPtr opus_encoder_create_delegate(OpusSampleRate Fs, OpusChannelCount channels, OPUS_APPLICATION application, out OpusErrorCode error);
        private delegate void opus_encoder_destroy_delegate(IntPtr st);
        private delegate int opus_encode_float_delegate(IntPtr st, IntPtr pcm, int frame_size, IntPtr data, int max_data_bytes);

        // These fields represent function pointers towards each of the extern functions. They get set
        // to the proper platform-specific functions by the static constructor. For example, if this is
        // running on a Windows machine, each of these pointers will point to the various avcodec_XXX_windows
        // extern functions listed above.

        private static opus_encoder_create_delegate opus_encoder_create_impl;
        private static opus_encoder_destroy_delegate opus_encoder_destroy_impl;
        private static opus_encode_float_delegate opus_encode_float_impl;

        /// <summary>
        /// The static constructor figures out which library to use for P/Invoke based
        /// on the current OS platform.
        /// </summary>
        static OpusInterop()
        {
            NativePathFinder.AddNativeLibraryPathToEnvironmentVariable();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                opus_encoder_create_impl = opus_encoder_create_windows;
                opus_encoder_destroy_impl = opus_encoder_destroy_windows;
                opus_encode_float_impl = opus_encode_float_windows;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                opus_encoder_create_impl = opus_encoder_create_linux;
                opus_encoder_destroy_impl = opus_encoder_destroy_linux;
                opus_encode_float_impl = opus_encode_float_linux;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                opus_encoder_create_impl = opus_encoder_create_osx;
                opus_encoder_destroy_impl = opus_encoder_destroy_osx;
                opus_encode_float_impl = opus_encode_float_osx;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region Public API
        
        public static IntPtr opus_encoder_create(OpusSampleRate Fs, OpusChannelCount channels, OPUS_APPLICATION application, out OpusErrorCode error)
        {
            return opus_encoder_create_impl(Fs, channels, application, out error);
        }
        
        public static void opus_encoder_destroy(IntPtr st)
        {
            opus_encoder_destroy_impl(st);
        }

        public static int opus_encode_float(IntPtr st, IntPtr pcm, int frame_size, IntPtr data, int max_data_bytes)
        {
            return opus_encode_float_impl(st, pcm, frame_size, data, max_data_bytes);
        }

        #endregion
    }
}
