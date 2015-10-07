using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DnTool.Utilities.MyPlugin
{
    public class KeypadByWinIO:IKeypad
    {
        private bool IsInitialize { get; set; }
        public KeypadByWinIO()
        {
            IsInitialize = WinIo.Initialize();
        }
        ~ KeypadByWinIO()
        {
            if(IsInitialize)
                 WinIo.Shutdown();
            IsInitialize = false;
        }
        public bool KeyDown(Keys k)
        {
            if (!IsInitialize)
                return false;
            
            WinIo.MykeyDown(k);
            return true;
        }

        public bool KeyUp(Keys k)
        {
            if (!IsInitialize)
                return false;

            WinIo.MykeyUp(k);
            return true;
        }

        public bool KeyPress(Keys k)
        {
            if (!IsInitialize)
                return false;
            WinIo.MykeyDown(k);
            Thread.Sleep(100);
            WinIo.MykeyUp(k);
            return true;
        }

        public int WaitKey(Keys k, int time)
        {
            throw new NotImplementedException();
        }
    }
}
