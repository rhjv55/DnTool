using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace IPlugin.Main
{
    public class KeypadByEvent:IKeypad
    {
        #region 引用的API
        const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        const uint KEYEVENTF_KEYUP = 0x0002;
        /// <summary>
        /// 该函数合成一次击键事件
        /// </summary>
        /// <param name="bVk">定义一个虚拟键码。键码值必须在1～254之间</param>
        /// <param name="bScan">定义该键的硬件扫描码</param>
        /// <param name="dwFlags">定义函数操作的各个方面的一个标志位集。应用程序可使用如下一些预定义常数的组合设置标志位。
　    　///  KEYEVENTF_EXTENDEDKEY：若指定该值，则扫描码前一个值为OXEO（224）的前缀字节。 
　      ///　KEYEVENTF_KEYUP：若指定该值，该键将被释放；若未指定该值，该键将被按下。</param>
        /// <param name="dwExtraInfo">定义与击键相关的附加的32位值</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern bool keybd_event(byte bVk, byte bScan, uint dwFlags,int dwExtraInfo);


        #endregion
        public bool KeyDown(VirtualKeyCode k)
        {
            return keybd_event((byte)k,0,0,0);
        }

        public bool KeyUp(VirtualKeyCode k)
        {
            throw new NotImplementedException();
        }

        public bool KeyPress(VirtualKeyCode k)
        {
            throw new NotImplementedException();
        }

        public int WaitKey(VirtualKeyCode k, int time)
        {
            throw new NotImplementedException();
        }
    }
}
