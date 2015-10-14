using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPlugin.Main
{
    public class KeypadByMsg:IKeypad
    {
        public bool KeyDown(VirtualKeyCode k)
        {
            throw new NotImplementedException();
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
