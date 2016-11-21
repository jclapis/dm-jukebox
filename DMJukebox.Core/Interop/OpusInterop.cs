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
        private static extern IntPtr opus_encoder_create_windows(OpusSampleRate Fs, OpusChannelCount channels, OPUS_APPLICATION application, ref OpusErrorCode error);

        [DllImport(WindowsOpusLibrary, EntryPoint = nameof(opus_encoder_destroy), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void opus_encoder_destroy_windows(IntPtr st);

        #endregion

        #region Linux Functions

        [DllImport(LinuxOpusLibrary, EntryPoint = nameof(opus_encoder_create), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr opus_encoder_create_linux(OpusSampleRate Fs, OpusChannelCount channels, OPUS_APPLICATION application, ref OpusErrorCode error);

        [DllImport(LinuxOpusLibrary, EntryPoint = nameof(opus_encoder_destroy), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void opus_encoder_destroy_linux(IntPtr st);

        #endregion

        #region OSX Functions

        [DllImport(MacOpusLibrary, EntryPoint = nameof(opus_encoder_create), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr opus_encoder_create_osx(OpusSampleRate Fs, OpusChannelCount channels, OPUS_APPLICATION application, ref OpusErrorCode error);

        [DllImport(MacOpusLibrary, EntryPoint = nameof(opus_encoder_destroy), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void opus_encoder_destroy_osx(IntPtr st);

        #endregion

        #region Delegates and Platform-Dependent Loading

        // These delegates all represent the function signatures for the libopus methods I need to call.

        private delegate IntPtr opus_encoder_create_delegate(OpusSampleRate Fs, OpusChannelCount channels, OPUS_APPLICATION application, ref OpusErrorCode error);
        private delegate void opus_encoder_destroy_delegate(IntPtr st);

        // These fields represent function pointers towards each of the extern functions. They get set
        // to the proper platform-specific functions by the static constructor. For example, if this is
        // running on a Windows machine, each of these pointers will point to the various avcodec_XXX_windows
        // extern functions listed above.

        private static opus_encoder_create_delegate opus_encoder_create_impl;
        private static opus_encoder_destroy_delegate opus_encoder_destroy_impl;

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
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                opus_encoder_create_impl = opus_encoder_create_linux;
                opus_encoder_destroy_impl = opus_encoder_destroy_linux;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                opus_encoder_create_impl = opus_encoder_create_osx;
                opus_encoder_destroy_impl = opus_encoder_destroy_osx;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region Public API

        /// <summary>
        /// Allocates and initializes an encoder state.
        /// 
        /// There are three coding modes:
        /// 
        /// OPUS_APPLICATION_VOIP gives best quality at a given bitrate for voice signals. It enhances the input signal by
        /// high-pass filtering and emphasizing formants and harmonics. Optionally it includes in-band forward error
        /// correction to protect against packet loss.Use this mode for typical VoIP applications.Because of the enhancement,
        /// even at high bitrates the output may sound different from the input.
        /// 
        /// OPUS_APPLICATION_AUDIO gives best quality at a given bitrate for most non-voice signals like music. Use this mode
        /// for music and mixed (music/voice) content, broadcast, and applications requiring less than 15 ms of coding delay.
        /// 
        /// OPUS_APPLICATION_RESTRICTED_LOWDELAY configures low-delay mode that disables the speech-optimized mode in exchange
        /// for slightly reduced delay. This mode can only be set on an newly initialized or freshly reset encoder because it
        /// changes the codec delay.
        /// 
        /// This is useful when the caller knows that the speech-optimized modes will not be needed (use with caution).
        /// </summary>
        /// <param name="Fs">Sampling rate of input signal (Hz) This must be one of 8000, 12000, 16000, 24000, or 48000.</param>
        /// <param name="channels">Number of channels (1 or 2) in input signal</param>
        /// <param name="application">Coding mode (OPUS_APPLICATION_VOIP/OPUS_APPLICATION_AUDIO/OPUS_APPLICATION_RESTRICTED_LOWDELAY)</param>
        /// <param name="error">Error codes</param>
        /// <returns></returns>
        public static IntPtr opus_encoder_create(OpusSampleRate Fs, OpusChannelCount channels, OPUS_APPLICATION application, ref OpusErrorCode error)
        {
            return opus_encoder_create_impl(Fs, channels, application, ref error);
        }

        /// <summary>
        /// Frees an OpusEncoder allocated by opus_encoder_create().
        /// </summary>
        /// <param name="st">State to be freed.</param>
        public static void opus_encoder_destroy(IntPtr st)
        {
            opus_encoder_destroy_impl(st);
        }

        #endregion
    }
}
