using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsInput;

namespace DnTool.Utilities.MyPlugin
{
    public class MouseByInput:IMouse
    {
        public bool LeftClick()
        {
            //WindowsInput.InputSimulator.
            return true;
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
