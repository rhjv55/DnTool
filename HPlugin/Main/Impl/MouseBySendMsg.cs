using IPlugin.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace IPlugin.Main
{
    public class MouseBySendMsg:IMouse
    {
        private int _hwnd;
        public MouseBySendMsg(int hwnd)
        {
            _hwnd = hwnd;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern bool SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        public bool LeftClick()
        {
            var a = SendMessage((IntPtr)_hwnd, Win32API.WM_LBUTTONDOWN, 0, 0);
            var b = SendMessage((IntPtr)_hwnd, Win32API.WM_LBUTTONUP, 0, 0);
            return a && b;
        }

        public bool LeftDown()
        {
            return SendMessage((IntPtr)_hwnd, Win32API.WM_LBUTTONDOWN, 0, 0);
        }

        public bool LeftUp()
        {
            return SendMessage((IntPtr)_hwnd, Win32API.WM_LBUTTONUP, 0, 0);
        }

        public bool RightClick()
        {
            var a = SendMessage((IntPtr)_hwnd, Win32API.WM_RBUTTONDOWN, 0, 0);
            var b = SendMessage((IntPtr)_hwnd, Win32API.WM_RBUTTONUP, 0, 0);
            return a && b;
        }

        public bool RightDown()
        {
            return SendMessage((IntPtr)_hwnd, Win32API.WM_RBUTTONDOWN, 0, 0);
        }

        public bool RightUp()
        {
            return SendMessage((IntPtr)_hwnd, Win32API.WM_RBUTTONUP, 0, 0);
        }

        public bool MiddleClick()
        {
            var a = SendMessage((IntPtr)_hwnd, Win32API.WM_MBUTTONDOWN, 0, 0);
            var b = SendMessage((IntPtr)_hwnd, Win32API.WM_MBUTTONUP, 0, 0);
            return a && b;
        }

        public bool MiddleDown()
        {
            return SendMessage((IntPtr)_hwnd, Win32API.WM_MBUTTONDOWN, 0, 0);
        }

        public bool MiddleUp()
        {
            return SendMessage((IntPtr)_hwnd, Win32API.WM_MBUTTONUP, 0, 0);
        }

        public bool MoveTo(int x, int y)
        {
            throw new NotImplementedException();
        }

        public bool WheelDown()
        {
            throw new NotImplementedException();
        }

        public bool WheelUp()
        {
            throw new NotImplementedException();
        }
    }
}
