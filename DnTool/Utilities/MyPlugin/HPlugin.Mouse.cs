using DnTool.Utilities.API;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DnTool.Utilities.MyPlugin
{
    public partial class HPlugin
    {
        IMouse m;

        public bool LeftClick()
        {
            return m.LeftClick();
        }

        public bool RightClick()
        {
            return m.RightClick();
        }

        public bool RightDown()
        {
            return m.RightDown();
        }

        public bool RightUp()
        {
            return m.RightUp();
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
            return m.MoveTo(x, y);
        }

        public bool WheelDown()
        {
            throw new NotImplementedException();
        }

        public bool WheelUp()
        {
            throw new NotImplementedException();
        }

        public bool GetCursorPos(out int x,out int y)
        {
            Point point = new Point();
            bool ret= Win32API.GetCursorPos(out point);
            x = point.X;
            y = point.Y;
            return ret;
        }
    }
}
