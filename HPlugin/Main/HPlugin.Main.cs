using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace IPlugin.Main
{
    public partial class HPlugin
    {
        public HPlugin()
        {
           
        }
   

        public void Delay(int time)
        {
            Thread.Sleep(time);
        }
    
    }
}
