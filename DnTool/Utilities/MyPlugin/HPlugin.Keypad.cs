using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DnTool.Utilities.MyPlugin
{
    public partial class HPlugin
    {
        IKeypad kp = new KeypadByWinIO();
        public bool KeyPress(Keys k)
        {
            return kp.KeyPress(k);
        }

        public bool KeyDown(Keys k)
        {
            return kp.KeyDown(k);
        }
        public bool KeyUp(Keys k)
        {
            return kp.KeyUp(k);
        }
    }
}
