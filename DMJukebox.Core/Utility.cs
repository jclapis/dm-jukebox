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
using System.Text;

namespace DMJukebox
{
    /// <summary>
    /// This is just a utility class for holding various helper functions.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Prints the details of an exception including its type, its message, the stacktrace, and the same details
        /// for all of the nested inner exceptions.
        /// </summary>
        /// <param name="Ex">The exception to get the details for</param>
        /// <returns>A string describing the exception in glorious detail</returns>
        public static string GetDetails(this Exception Ex)
        {
            StringBuilder builder = new StringBuilder($"{Ex.GetType().Name} - {Ex.Message}{Environment.NewLine}{Ex.StackTrace}");
            Exception inner = Ex.InnerException;
            while(inner != null)
            {
                builder.Append($"\tInner: {inner.GetType().Name} - {inner.Message}{Environment.NewLine}{inner.StackTrace}");
                inner = inner.InnerException;
            }
            return builder.ToString();
        }

    }
}
