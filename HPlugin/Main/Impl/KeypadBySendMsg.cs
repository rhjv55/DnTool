using IPlugin.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace IPlugin.Main
{
   public  class KeypadBySendMsg:IKeypad
    {
       private int _hwnd;
       public KeypadBySendMsg(int hwnd)
       {
           _hwnd = hwnd;
       }


       [DllImport("user32.dll", CharSet = CharSet.Auto)]
       static extern bool SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
      
       public bool KeyDown(VirtualKeyCode k)
       {
          return SendMessage((IntPtr)_hwnd, Win32API.WM_KEYDOWN, (int)k, 0);
       }

       public bool KeyUp(VirtualKeyCode k)
       {
          return SendMessage((IntPtr)_hwnd, Win32API.WM_KEYUP, (int)k, 0);
       }

       public bool KeyPress(VirtualKeyCode k)
       {
           return SendMessage((IntPtr)_hwnd, Win32API.WM_CHAR, (int)k, 0);
       }

       public int WaitKey(VirtualKeyCode k, int time)
       {
           throw new NotImplementedException();
       }
    }
}
