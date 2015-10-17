using IPlugin.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace IPlugin.Main
{
   public  class KeypadBySendMsg:IKeypad
    {
       private int _hwnd;
       public KeypadBySendMsg(int hwnd)
       {
           _hwnd = hwnd;
       }
        public const Int32 WM_SYSCOMMAND = 0x112;
        public const Int32 SC_SCREENSAVE = 0xF140;

       [DllImport("user32.dll", CharSet = CharSet.Auto)]
       static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
       [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPStr)] string lParam);
       public void SetTextProperty ()
       {
            IntPtr textBoxHandle = IntPtr.Zero/*Get the handle to a textbox control*/;

            //Set textbox text
            //0x000C (or WM_SETTEXT) is a window message (for setting a text property)
            StringBuilder sb = new StringBuilder("Hello Pagli Usha");
            int result = SendMessage(textBoxHandle, 0x000C, (IntPtr)sb.Length, sb.ToString());
       }
    
       public bool KeyDown(VirtualKeyCode k)
       {
           SendMessage((IntPtr)_hwnd, (UInt32)Win32API.WM_KEYDOWN, (IntPtr)SC_SCREENSAVE, (IntPtr)0);
           return true;
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
