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
        static extern bool PostMessage(int hWnd, uint Msg, IntPtr wParam, IntPtr lParam);


        public bool KeyDown(VirtualKeyCode k)
        {
            return PostMessage(_hwnd,0,(IntPtr)0,(IntPtr)0);
        }

        public bool KeyUp(VirtualKeyCode k)
        {
            throw new NotImplementedException();
        }

        public bool KeyPress(VirtualKeyCode k)
        {
            throw new NotImplementedException();
        }

        public int WaitKey(VirtualKeyCode k, int time)
        {
            throw new NotImplementedException();
        }
    }
}
