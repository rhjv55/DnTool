using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DnTool.Utilities.MyPlugin
{
    public interface IMouse
    {
        bool LeftClick();
        bool RightClick();
        bool RightDown();
        bool RightUp();
        bool MiddleClick();
        bool MiddleDown();
        bool MiddleUp();
        bool MoveTo(int x, int y);
        bool WheelDown();
        bool WheelUp();
    }
}
