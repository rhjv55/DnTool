﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPlugin.Main
{
    public class MouseByPostMsg:IMouse
    {
        private int _hwnd;

        public MouseByPostMsg(int hwnd)
        {
            this._hwnd = hwnd;
        }
        public bool LeftClick()
        {
            throw new NotImplementedException();
        }

        public bool RightClick()
        {
            throw new NotImplementedException();
        }

        public bool RightDown()
        {
            throw new NotImplementedException();
        }

        public bool RightUp()
        {
            throw new NotImplementedException();
        }

        public bool MiddleClick()
        {
            throw new NotImplementedException();
        }

        public bool MiddleDown()
        {
            throw new NotImplementedException();
        }

        public bool MiddleUp()
        {
            throw new NotImplementedException();
        }

        public bool MoveTo(int x, int y)
        {
            throw new NotImplementedException();
        }

        public bool WheelDown()
        {
            throw new NotImplementedException();
        }

        public bool WheelUp()
        {
            throw new NotImplementedException();
        }


        public bool LeftDown()
        {
            throw new NotImplementedException();
        }

        public bool LeftUp()
        {
            throw new NotImplementedException();
        }
    }
}
