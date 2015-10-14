﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsInput;

namespace DnTool.Utilities.MyPlugin
{
    public class KeypadByInput:IKeypad
    {
        public bool KeyDown(Keys k)
        {
            InputSimulator.SimulateKeyDown((VirtualKeyCode)k);
            return true;
        }

        public bool KeyUp(Keys k)
        {
            InputSimulator.SimulateKeyUp((VirtualKeyCode)k);
            return true;
        }

        public bool KeyPress(Keys k)
        {
            InputSimulator.SimulateKeyPress((VirtualKeyCode)k);
            return true;
        }

        public int WaitKey(Keys k, int time)
        {
            throw new NotImplementedException();
        }
    }
}
