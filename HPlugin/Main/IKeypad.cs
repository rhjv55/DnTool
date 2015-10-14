using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPlugin.Main
{
    public interface IKeypad
    {
        bool KeyDown(VirtualKeyCode k);
        bool KeyUp(VirtualKeyCode k);
        bool KeyPress(VirtualKeyCode k);
        int WaitKey(VirtualKeyCode k, int time);
        
    }
}
