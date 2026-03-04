using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SESSION_11
{
    internal static class NumHelper 
    {
        public static bool IsMultipleOf(this int value, int num)
        {
            if (num == 0) return false;
            return value % num == 0;
        }
    }
}
