using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class UtilExtensions
    {
        public static string FormatWith(this string str, params object[] args)
        {
            return string.Format(str,args);
        }
    }
}
