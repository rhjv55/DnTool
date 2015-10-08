using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DnTool.Utilities.MyPlugin
{
    public partial class HPlugin
    {
        IMouse m = new MouseByWinIO();

        public bool LeftClick()
        {
            return m.LeftClick();
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
