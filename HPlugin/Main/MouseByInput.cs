using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace IPlugin.Main
{
    public class MouseByInput:IMouse
    {
        public bool LeftClick()
        {
            InputSimulator.SimulateLeftClick();
            return true;
        }

        public bool RightClick()
        {
            return InputSimulator.SimulateRightClick();
        }

        public bool RightDown()
        {
            return InputSimulator.SimulateRightDown();
        }

        public bool RightUp()
        {
            return InputSimulator.SimulateRightUp();
        }

        public bool MiddleClick()
        {
            return InputSimulator.SimulateMiddleClick();
        }

        public bool MiddleDown()
        {
            return InputSimulator.SimulateMiddleDown();
        }

        public bool MiddleUp()
        {
            return InputSimulator.SimulateMiddleUp();
        }

        public bool MoveTo(int x, int y)
        {
            return InputSimulator.SimulateMoveTo(x,y);
        }

        public bool WheelDown()
        {
            return InputSimulator.SimulateWheel(-120);
        }

        public bool WheelUp()
        {
            return InputSimulator.SimulateWheel(120);
        }


        public bool LeftDown()
        {
            return InputSimulator.SimulateLeftDown();
        }

        public bool LeftUp()
        {
            return InputSimulator.SimulateLeftUp();
        }
    }
}
