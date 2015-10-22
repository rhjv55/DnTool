using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace IPlugin.Main
{
    public class KeypadByInput:IKeypad
    {
        public bool KeyDown(VirtualKeyCode k)
        {
            InputSimulator.SimulateKeyDown((VirtualKeyCode)k);
            return true;
        }

        public bool KeyUp(VirtualKeyCode k)
        {
            InputSimulator.SimulateKeyUp((VirtualKeyCode)k);
            return true;
        }

        public bool KeyPress(VirtualKeyCode k)
        {
            InputSimulator.SimulateKeyPress((VirtualKeyCode)k);
            return true;
        }

        public int WaitKey(VirtualKeyCode k, int time)
        {
            throw new NotImplementedException();
        }
    //        int length = wcslen(strs);
    //for (int i = 0; i < length; ++i)
    //{
    //    INPUT   keyin;
    //    keyin.type=INPUT_KEYBOARD;
    //    keyin.ki.wVk=0;
    //    keyin.ki.wScan=strs[i];
    //    keyin.ki.time=100;
    //    keyin.ki.dwFlags=KEYEVENTF_UNICODE;
    //    keyin.ki.dwExtraInfo=GetMessageExtraInfo();
    //    ::SendInput(1,  &keyin, sizeof(INPUT));
    //    Sleep(delays);
    //}
    //return true;
    }
}
