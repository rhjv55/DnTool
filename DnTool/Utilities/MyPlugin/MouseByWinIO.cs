using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DnTool.Utilities.MyPlugin
{
    public class MouseByWinIO:IMouse
    {

        public bool LeftClick()
        {
            WinIo.MyMouseKey(9,0,0,0);
            Thread.Sleep(100);
            WinIo.MyMouseKey(8, 0, 0, 0);
            return true;
        }

        public bool RightClick()
        {
            WinIo.MyMouseKey(10, 0, 0, 0);
            Thread.Sleep(100);
            WinIo.MyMouseKey(8, 0, 0, 0);
            return true;
        }

        public bool RightDown()
        {
            WinIo.MyMouseKey(10, 0, 0, 0);
            return true;
        }

        public bool RightUp()
        {
            WinIo.MyMouseKey(8, 0, 0, 0);
            return true;
        }

        public bool MiddleClick()
        {
            WinIo.MyMouseKey(12, 0, 0, 0);
            Thread.Sleep(100);
            WinIo.MyMouseKey(8, 0, 0, 0);
            return true;
        }

        public bool MiddleDown()
        {
            WinIo.MyMouseKey(12, 0, 0, 0);
            return true;
        }

        public bool MiddleUp()
        {
            WinIo.MyMouseKey(8, 0, 0, 0);
            return true;
        }

        public bool MoveTo(int x, int y)
        {
         // MyMouseKey 40, 0,(255 xor 5),0   '下移5象素
//MyMouseKey 24,(255 xor 5), 0, 0  '左移5象素
//MyMouseKey 8, 5, 0, 0            '右移5象素
            WinIo.MyMouseKey(8,0,100,0);
            return true;
        }

        public bool WheelDown()
        {
            throw new NotImplementedException();
        }

        public bool WheelUp()
        {
            throw new NotImplementedException();
        }
    }
}
