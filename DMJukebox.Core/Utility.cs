/*
 * Copyright (c) 2016 Joe Clapis.
 */

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
