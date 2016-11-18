using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a utility class that figures out what the path for the native libraries is and
    /// adds it to the PATH environment variable during startup.
    /// </summary>
    internal static class NativePathFinder
    {
        /// <summary>
        /// This is just a flag that gets set to true after the first time
        /// <see cref="AddNativeLibraryPathToEnvironmentVariable"/> is called,
        /// so it only runs once.
        /// </summary>
        private static bool PathFound;

        /// <summary>
        /// Determines the path of the native library dependencies based on the current process
        /// architecture and adds it to the PATH environment variable.
        /// </summary>
        public static void AddNativeLibraryPathToEnvironmentVariable()
        {
            if(PathFound)
            {
                return;
            }

            string libraryPath;
            switch (RuntimeInformation.ProcessArchitecture)
            {
                case Architecture.X86:
                    libraryPath = Path.Combine(Directory.GetCurrentDirectory(), "lib", "x86");
                    break;

                case Architecture.X64:
                    libraryPath = Path.Combine(Directory.GetCurrentDirectory(), "lib", "x64");
                    break;

                case Architecture.Arm:
                    libraryPath = Path.Combine(Directory.GetCurrentDirectory(), "lib", "arm");
                    break;

                case Architecture.Arm64:
                    libraryPath = Path.Combine(Directory.GetCurrentDirectory(), "lib", "arm64");
                    break;

                default:
                    throw new NotSupportedException($"Architecture {RuntimeInformation.ProcessArchitecture} is not supported.");
            }

            string path = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
            path += $"{Path.PathSeparator}{libraryPath}";
            Environment.SetEnvironmentVariable("PATH", path);

            PathFound = true;
        }
    }
}
