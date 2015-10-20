using IPlugin.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace IPlugin.Main
{
    public class MouseByPostMsg:IMouse
    {
        private int _hwnd;

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        public MouseByPostMsg(int hwnd)
        {
            this._hwnd = hwnd;
        }
        public bool LeftClick()
        {
            var a = PostMessage((IntPtr)_hwnd, Win32API.WM_LBUTTONDOWN, 0, 0);
            var b = PostMessage((IntPtr)_hwnd, Win32API.WM_LBUTTONUP, 0, 0);
            return a && b;
        }

        public bool LeftDown()
        {
            return PostMessage((IntPtr)_hwnd, Win32API.WM_LBUTTONDOWN, 0, 0);
        }

        public bool LeftUp()
        {
            return PostMessage((IntPtr)_hwnd, Win32API.WM_LBUTTONUP, 0, 0);
        }

        public bool RightClick()
        {
            var a = PostMessage((IntPtr)_hwnd, Win32API.WM_RBUTTONDOWN, 0, 0);
            var b = PostMessage((IntPtr)_hwnd, Win32API.WM_RBUTTONUP, 0, 0);
            return a && b;
        }

        public bool RightDown()
        {
            return PostMessage((IntPtr)_hwnd, Win32API.WM_RBUTTONDOWN, 0, 0);
        }

        public bool RightUp()
        {
            return PostMessage((IntPtr)_hwnd, Win32API.WM_RBUTTONUP, 0, 0);
        }

        public bool MiddleClick()
        {
            var a = PostMessage((IntPtr)_hwnd, Win32API.WM_MBUTTONDOWN, 0, 0);
            var b = PostMessage((IntPtr)_hwnd, Win32API.WM_MBUTTONUP, 0, 0);
            return a && b;
        }

        public bool MiddleDown()
        {
            return PostMessage((IntPtr)_hwnd, Win32API.WM_MBUTTONDOWN, 0, 0);
        }

        public bool MiddleUp()
        {
            return PostMessage((IntPtr)_hwnd, Win32API.WM_MBUTTONUP, 0, 0);
        }

        public bool MoveTo(int x, int y)
        {
            return PostMessage((IntPtr)_hwnd, Win32API.WM_MOUSEMOVE, 0, MakeLParam(y, x));
        }

        public bool WheelDown()
        {
            return PostMessage((IntPtr)_hwnd, Win32API.WM_MOUSEWHEEL, -120, 0);
        }

        public bool WheelUp()
        {
            return PostMessage((IntPtr)_hwnd, Win32API.WM_MOUSEWHEEL, 120, 0);
        }
        private int MakeLParam(int LoWord, int HiWord)
        {
            return ((HiWord << 16) | (LoWord & 0xffff));
        }
    }
}
