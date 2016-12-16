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
using System.Runtime.InteropServices;

namespace DMJukebox.Interop
{
    /// <summary>
    /// This is a utility class that holds the P/Invoke wrappers for libsodium.
    /// </summary>
    /// <remarks>
    /// For more information, please see the documentation at
    /// https://download.libsodium.org/doc/
    /// or the source code at https://github.com/jedisct1/libsodium/.
    /// </remarks>
    internal static class SodiumInterop
    {
        /// <summary>
        /// The DLL for Windows
        /// </summary>
        private const string WindowsSodiumLibrary = "libsodium.dll";

        /// <summary>
        /// The SO for Linux
        /// </summary>
        private const string LinuxSodiumLibrary = "libsodium.so";

        /// <summary>
        /// The Dylib for OSX
        /// </summary>
        private const string MacSodiumLibrary = "libsodium.dylib";

        // These regions contain the DllImport function definitions for each OS. Since we can't really set
        // the path of DllImport dynamically (and loading them dynamically using LoadLibrary / dlopen is complicated
        // to manage cross-platform), we have to pre-define them based on the names of the libraries above.

        #region Windows Functions

        [DllImport(WindowsSodiumLibrary, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int crypto_secretbox_easy_windows(IntPtr c, IntPtr m, ulong mlen, IntPtr n, IntPtr k);

        #endregion

        #region Linux Functions

        [DllImport(WindowsSodiumLibrary, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int crypto_secretbox_easy_linux(IntPtr c, IntPtr m, ulong mlen, IntPtr n, IntPtr k);

        #endregion

        #region OSX Functions

        [DllImport(WindowsSodiumLibrary, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int crypto_secretbox_easy_osx(IntPtr c, IntPtr m, ulong mlen, IntPtr n, IntPtr k);

        #endregion

        #region Delegates and Platform-Dependent Loading

        // These delegates all represent the function signatures for the libsodium methods I need to call.

        private delegate int crypto_secretbox_easy_delegate(IntPtr c, IntPtr m, ulong mlen, IntPtr n, IntPtr k);

        // These fields represent function pointers towards each of the extern functions. They get set
        // to the proper platform-specific functions by the static constructor. For example, if this is
        // running on a Windows machine, each of these pointers will point to the various XXX_windows
        // extern functions listed above.

        private static crypto_secretbox_easy_delegate crypto_secretbox_easy_impl;

        /// <summary>
        /// The static constructor figures out which library to use for P/Invoke based
        /// on the current OS platform.
        /// </summary>
        static SodiumInterop()
        {
            NativePathFinder.AddNativeLibraryPathToEnvironmentVariable();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                crypto_secretbox_easy_impl = crypto_secretbox_easy_windows;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                crypto_secretbox_easy_impl = crypto_secretbox_easy_linux;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                crypto_secretbox_easy_impl = crypto_secretbox_easy_osx;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        #endregion

        #region Public API

        /// <summary>
        /// Encrypts a data buffer using the provided key and a nonce.
        /// </summary>
        /// <param name="c">(byte*) The output buffer to contain the encrypted data.
        /// Libsodium adds 16 bytes of overhead for encryption, so the size of this buffer
        /// should be the size of the input buffer + 16.</param>
        /// <param name="m">(byte*) The input buffer containing the data to encrypt</param>
        /// <param name="mlen">The size of the input buffer (number of bytes)</param>
        /// <param name="n">(byte*) The nonce to encrypt the data with</param>
        /// <param name="k">(byte*) The secret key to encrypt the data with</param>
        /// <returns>0 on a success, -1 on a failure.</returns>
        public static int crypto_secretbox_easy(IntPtr c, IntPtr m, ulong mlen, IntPtr n, IntPtr k)
        {
            return crypto_secretbox_easy_impl(c, m, mlen, n, k);
        }

        #endregion
    }
}
