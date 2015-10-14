using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPlugin.Main
{
    public class MouseByEvent:IMouse
    {
        //[DllImport("user32.dll")]
        //private static extern int SetCursorPos(int x, int y);

        //public enum MouseEventFlags
        //{
        //    Move = 0x0001,
        //    LeftDown = 0x0002,
        //    LeftUp = 0x0004,
        //    RightDown = 0x0008,
        //    RightUp = 0x0010,
        //    MiddleDown = 0x0020,
        //    MiddleUp = 0x0040,
        //    Wheel = 0x0800,
        //    Absolute = 0x8000
        //}
        //[DllImport("User32")]
        //public extern static void mouse_event(int dwFlags, int dx, int dy, int dwData, IntPtr dwExtraInfo);

        ///// <summary>        
        ///// 移动鼠标到指定的坐标点        
        ///// </summary>        
        //public void MoveMouseToPoint()
        //{
        //    string settingValue = ConfigurationManager.AppSettings.Get("Point");
        //    string[] pList = settingValue.Split(',');
        //    Point centerP = new Point(int.Parse(pList[0]), int.Parse(pList[1]));
        //    if (checkBox1.Checked)
        //    {
        //        SetCursorPos(centerP.X, centerP.Y);
        //        mouse_event((int)(MouseEventFlags.LeftDown | MouseEventFlags.Absolute), 0, 0, 0, IntPtr.Zero);
        //        mouse_event((int)(MouseEventFlags.LeftUp | MouseEventFlags.Absolute), 0, 0, 0, IntPtr.Zero);
        //    }
        //}
        ///// <summary>       
        ///// /// 设置鼠标的移动范围        
        ///// </summary>        
        //public void SetMouseRectangle(Rectangle rectangle)
        //{
        //    System.Windows.Forms.Cursor.Clip = rectangle;
        //}
        public bool LeftClick()
        {
            throw new NotImplementedException();
        }

        public bool RightClick()
        {
            throw new NotImplementedException();
        }

        public bool RightDown()
        {
            throw new NotImplementedException();
        }

        public bool RightUp()
        {
            throw new NotImplementedException();
        }

        public bool MiddleClick()
        {
            throw new NotImplementedException();
        }

        public bool MiddleDown()
        {
            throw new NotImplementedException();
        }

        public bool MiddleUp()
        {
            throw new NotImplementedException();
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
