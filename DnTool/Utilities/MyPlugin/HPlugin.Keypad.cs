using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DnTool.Utilities.MyPlugin
{
    public partial class HPlugin
    {
       
        public bool KeyPress11(Keys k)
        {
            IKeypad kp = new KeypadByWinIO();
            return kp.KeyPress(k);
        }
    }
}
