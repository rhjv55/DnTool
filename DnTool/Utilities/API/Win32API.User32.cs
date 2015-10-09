using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DnTool.Utilities.API
{
    public partial class Win32API
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
       public static extern bool GetCursorPos(out Point lpPoint);
        // Or use System.Drawing.Point (Forms only)
    }
}
