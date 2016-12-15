/* ========================================================================
 * 
 * This file is part of the DM Jukebox project.
 * Copyright (c) 2016 Joe Clapis. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * ====================================================================== */

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
