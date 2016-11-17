using System;
using System.Text;

namespace DMJukebox
{
    public static class Utility
    {
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
