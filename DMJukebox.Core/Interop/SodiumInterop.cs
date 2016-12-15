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
        /// Encrypts a data buffer using the provided key and a nonce.
        /// </summary>
        /// <param name="c">(byte*) The output buffer to contain the encrypted data.
        /// Libsodium adds 16 bytes of overhead for encryption, so the size of this buffer
        /// should be the size of the input buffer + 16.</param>
        /// <param name="m">(byte*) The input buffer containing the data to encrypt</param>
        /// <param name="mlen">The size of the input buffer (number of bytes)</param>
        /// <param name="n">(byte*) The nonce to encrypt the data with</param>
        /// <param name="k">(byte*) The secret key to encrypt the data with</param>
        /// <returns></returns>
        [DllImport("libsodium.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int crypto_secretbox_easy(IntPtr c, IntPtr m, ulong mlen, IntPtr n, IntPtr k);
    }
}
