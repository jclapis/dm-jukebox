using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    internal static class NativePathFinder
    {
        private static bool PathFound;

        public static void AddNativeLibraryPathToEnvironmentVariable()
        {
            System.Diagnostics.Debug.WriteLine($"ARCH = {RuntimeInformation.ProcessArchitecture}");
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

            System.Diagnostics.Debug.WriteLine($"PATH = {path}");

            PathFound = true;
        }
    }
}
