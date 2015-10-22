using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPlugin.Main
{
    public partial class HPlugin
    {
        IKeypad kp;
        public bool KeyPress(VirtualKeyCode k)
        {
            return kp.KeyPress(k);
        }

        public bool KeyDown(VirtualKeyCode k)
        {
            return kp.KeyDown(k);
        }
        public bool KeyUp(VirtualKeyCode k)
        {
    //            MSG  msg;
    //int waittime=0;
    //if(delay>0)
    //    waittime=delay;
    //int time1=::GetTickCount();
    //while(true)
    //{
    //    if(PeekMessage(&msg,NULL,0,0,PM_REMOVE))
    //    {
    //        if(WM_QUIT==msg.message)
    //            break;
    //        TranslateMessage(&msg);
    //        DispatchMessage(&msg);
    //    }
    //    else
    //    {
    //        if(keycode==0)
    //        {
    //            for(int i=0;i<127;i++)
    //            {
    //                if(0x8000 & GetAsyncKeyState(keyCode[i]))
    //                {
    //                    return true;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            if(0x8000 & GetAsyncKeyState(keycode))
    //            {
    //                return true;
    //            }

    //        }
    //        if(waittime>0)
    //        {
    //            if((::GetTickCount()-time1)>=waittime)
    //                return false;
    //        }
    //    }
    //    ::Sleep(5);
    //}
    //return false;
            return kp.KeyUp(k);
        }
    }
}
