using DnTool.Utilities.API;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace DnTool.Utilities.MyPlugin
{
    public class MouseByWinIO:IMouse
    {

        public bool LeftClick()
        {
            WinIo.MyMouseKey(9,0,0,0);
            Thread.Sleep(100);
            WinIo.MyMouseKey(8, 0, 0, 0);
            return true;
        }

        public bool RightClick()
        {
            WinIo.MyMouseKey(10, 0, 0, 0);
            Thread.Sleep(100);
            WinIo.MyMouseKey(8, 0, 0, 0);
            return true;
        }

        public bool RightDown()
        {
            WinIo.MyMouseKey(10, 0, 0, 0);
            return true;
        }

        public bool RightUp()
        {
            WinIo.MyMouseKey(8, 0, 0, 0);
            return true;
        }

        public bool MiddleClick()
        {
            WinIo.MyMouseKey(12, 0, 0, 0);
            Thread.Sleep(100);
            WinIo.MyMouseKey(8, 0, 0, 0);
            return true;
        }

        public bool MiddleDown()
        {
            WinIo.MyMouseKey(12, 0, 0, 0);
            return true;
        }

        public bool MiddleUp()
        {
            WinIo.MyMouseKey(8, 0, 0, 0);
            return true;
        }

        public bool MoveTo(int x, int y)
        {
            Point point = new Point();
            bool ret = Win32API.GetCursorPos(out point);
            if (ret == false)
                return false;
            int a = point.X - x;
            int b = point.Y - y;
            if (a > 0)
            {
                while(point.X-x!=0)
                {
                    point.X = point.X - 1;
                    WinIo.MyMouseKey(24, point.X, 0, 0);
                }
            }
            if (a < 0)
            {
                while (point.X - x != 0)
                {
                    point.X = point.X - 1;
                    WinIo.MyMouseKey(24, point.X, 0, 0); //向左移
                }
            }
            if (b > 0)
            {
                while (point.Y - y != 0)
                {
                    point.X = point.X - 1;
                    WinIo.MyMouseKey(24, point.X, 0, 0);
                }
            }
            if (b < 0)
            {
                while (y - point.Y != 0)
                {
                    point.Y += 1;
                    WinIo.MyMouseKey(40, point.Y, 0, 0);  //向下移
                }
            }
            return true;
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
