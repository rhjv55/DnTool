using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DnTool.Utilities.MyPlugin
{
    public interface IKeypad
    {
        bool KeyDown(Keys k);
        bool KeyUp(Keys k);
        bool KeyPress(Keys k);
        int WaitKey(Keys k, int time);
    }
}
