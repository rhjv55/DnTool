using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace IPlugin.Main
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
        public bool KeyDown(VirtualKeyCode k)
        {
            if (!IsInitialize)
                return false;
            
            WinIo.MykeyDown(k);
            return true;
        }

        public bool KeyUp(VirtualKeyCode k)
        {
            if (!IsInitialize)
                return false;

            WinIo.MykeyUp(k);
            return true;
        }

        public bool KeyPress(VirtualKeyCode k)
        {
            if (!IsInitialize)
                return false;
            WinIo.MykeyDown(k);
            Thread.Sleep(100);
            WinIo.MykeyUp(k);
            return true;
        }

        public int WaitKey(VirtualKeyCode k, int time)
        {
            throw new NotImplementedException();
        }

       
    }
}
