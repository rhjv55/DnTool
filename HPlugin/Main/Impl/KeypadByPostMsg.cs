using IPlugin.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace IPlugin.Main
{
    public class KeypadByPostMsg:IKeypad
    {
        private int _hwnd;
        public KeypadByPostMsg(int hwnd)
        {
            _hwnd = hwnd;
        }
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int MapVirtualKey(uint Ucode, uint uMapType);

        public bool KeyDown(VirtualKeyCode k)
        {
            return PostMessage((IntPtr)_hwnd, Win32API.WM_KEYDOWN, (int)k, 0);
        }

        public bool KeyUp(VirtualKeyCode k)
        {
            return PostMessage((IntPtr)_hwnd, Win32API.WM_KEYUP, (int)k, 0);
        }

        public bool KeyPress(VirtualKeyCode k)
        {
            return PostMessage((IntPtr)_hwnd, Win32API.WM_CHAR, (int)k, 0);
        }

        public int WaitKey(VirtualKeyCode k, int time)
        {
            throw new NotImplementedException();
        }
    }
}
