using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DiscordJukebox
{
    class test
    {
        unsafe public test()
        {
            IntPtr ptr = IntPtr.Zero;
            S* thing = (S*)ptr.ToPointer();
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public unsafe struct S
    {
        public fixed ulong thing[12];
    }
}
