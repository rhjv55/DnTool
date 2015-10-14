﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DnTool.Utilities.MyPlugin
{
    public partial class HPlugin
    {
        /// <summary>
        /// 绑定窗口
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="display"></param>
        /// <param name="mouse">1.input,2.msg,3.winio,4.event</param>
        /// <param name="keypad">1.input,2.msg,3.winio,4.event</param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public bool BindWindow(int hwnd,string display,string mouse,string keypad,int mode)
        {
            switch (mouse)
            {
                case "input":
                    m = new MouseByInput();
                    break;
                case "msg":
                    m = new MouseByMsg();
                    break;
                case "winio":
                    m = new MouseByWinIO();
                    break;
                case "event":
                    m = new MouseByEvent();
                    break;
            }
            switch(keypad)
            {
                case "input":
                   kp=new KeypadByInput() ;
                   break;
                case "msg":
                   kp=new KeypadByMsg() ;
                   break;
                case "winio":
                   kp=new KeypadByWinIO() ;
                   break;
                case "event":
                   kp=new KeypadByEvent() ;
                   break;
            }
            return true;
        }
    }
}
