using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace IPlugin.Main
{
    public class KeypadByInput:IKeypad
    {
        public bool KeyDown(VirtualKeyCode k)
        {
            InputSimulator.SimulateKeyDown((VirtualKeyCode)k);
            return true;
        }

        public bool KeyUp(VirtualKeyCode k)
        {
            InputSimulator.SimulateKeyUp((VirtualKeyCode)k);
            return true;
        }

        public bool KeyPress(VirtualKeyCode k)
        {
            InputSimulator.SimulateKeyPress((VirtualKeyCode)k);
            return true;
        }

        public int WaitKey(VirtualKeyCode k, int time)
        {
            throw new NotImplementedException();
        }
    }
}
