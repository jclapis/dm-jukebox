namespace DiscordJukebox.Interop
{
    internal enum SoundIoError
    {
        SoundIoErrorNone,

        /// <summary>
        /// Out of memory.
        /// </summary>
        SoundIoErrorNoMem,

        /// <summary>
        /// The backend does not appear to be active or running.
        /// </summary>
        SoundIoErrorInitAudioBackend,

        /// <summary>
        /// A system resource other than memory was not available.
        /// </summary>
        SoundIoErrorSystemResources,

        /// <summary>
        /// Attempted to open a device and failed.
        /// </summary>
        SoundIoErrorOpeningDevice,

        /// <summary>
        /// The device doesn't exist.
        /// </summary>
        SoundIoErrorNoSuchDevice,

        /// <summary>
        /// The programmer did not comply with the API.
        /// </summary>
        SoundIoErrorInvalid,

        /// <summary>
        /// libsoundio was compiled without support for that backend.
        /// </summary>
        SoundIoErrorBackendUnavailable,

        /// <summary>
        /// An open stream had an error that can only be recovered from by
        /// destroying the stream and creating it again.
        /// </summary>
        SoundIoErrorStreaming,

        /// <summary>
        /// Attempted to use a device with parameters it cannot support.
        /// </summary>
        SoundIoErrorIncompatibleDevice,

        /// <summary>
        /// When JACK returns `JackNoSuchClient`
        /// </summary>
        SoundIoErrorNoSuchClient,

        /// <summary>
        /// Attempted to use parameters that the backend cannot support.
        /// </summary>
        SoundIoErrorIncompatibleBackend,

        /// <summary>
        /// Backend server shutdown or became inactive.
        /// </summary>
        SoundIoErrorBackendDisconnected,
        SoundIoErrorInterrupted,

        /// <summary>
        /// Buffer underrun occurred.
        /// </summary>
        SoundIoErrorUnderflow,

        /// <summary>
        /// Unable to convert to or from UTF-8 to the native string format.
        /// </summary>
        SoundIoErrorEncodingString
    }
}
