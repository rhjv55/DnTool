using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPlugin.Main
{
    public partial class HPlugin
    {
        IKeypad kp;
        public bool KeyPress(VirtualKeyCode k)
        {
            return kp.KeyPress(k);
        }

        public bool KeyDown(VirtualKeyCode k)
        {
            return kp.KeyDown(k);
        }
        public bool KeyUp(VirtualKeyCode k)
        {
            return kp.KeyUp(k);
        }
    }
}
