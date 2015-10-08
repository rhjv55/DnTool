using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DnTool.Utilities.MyPlugin
{
    public class WinIo  
    {
        public const int KBC_KEY_CMD = 0x64;  //输入键盘按下消息的端口
        public const int KBC_KEY_DATA = 0x60;  //输入键盘弹起消息的端口
          
        [DllImport("WinIo32.dll")]  
        public static extern bool InitializeWinIo();  
  
        [DllImport("WinIo32.dll")]  
        public static extern bool GetPortVal(IntPtr wPortAddr, out int pdwPortVal,byte bSize);  
  
        [DllImport("WinIo32.dll")]  
        public static extern bool SetPortVal(uint wPortAddr, IntPtr dwPortVal,byte bSize);  
  
        [DllImport("WinIo32.dll")]  
        public static extern byte MapPhysToLin(byte pbPhysAddr, uint dwPhysSize,IntPtr PhysicalMemoryHandle);  
  
        [DllImport("WinIo32.dll")]  
        public static extern bool UnmapPhysicalMemory(IntPtr PhysicalMemoryHandle,byte pbLinAddr);  
  
        [DllImport("WinIo32.dll")]  
        public static extern bool GetPhysLong(IntPtr pbPhysAddr, byte pdwPhysVal);  
  
        [DllImport("WinIo32.dll")]  
        public static extern bool SetPhysLong(IntPtr pbPhysAddr, byte dwPhysVal);  
  
        [DllImport("WinIo32.dll")]  
        public static extern void ShutdownWinIo();  
  
        [DllImport("user32.dll")]  
        public static extern int MapVirtualKey(uint Ucode, uint uMapType);  
  
        private static bool IsInitialize { get; set; }  
  
        public static bool Initialize()  
        {
            if (IsInitialize == true)
            {
                return true;
                Debug.WriteLine("已经初始化！");
            }
            if (InitializeWinIo())
            {
                KBCWait4IBE();
                IsInitialize = true;
                Debug.WriteLine("WinIO初始化成功！");
            }
            else
            {
                IsInitialize = false;
                Debug.WriteLine("WinIO初始化失败！");
            }
            return IsInitialize;
        }  
        public static void Shutdown()  
        {
            if (IsInitialize)
            {
                ShutdownWinIo();
                Debug.WriteLine("WinIO已卸载！");
            }
            IsInitialize = false;  
        }
        /// <summary>
        /// 等待键盘缓冲区为空
        /// </summary>
        private static void KBCWait4IBE()  
        {  
            int dwVal = 0;  
            do  
            {  
                bool flag = GetPortVal((IntPtr)0x64, out dwVal, 1);  
            }  
            while ((dwVal & 0x2) > 0);  
        }  
        /// key down  
        public static void MykeyDown(Keys vKeyCoad)  
        {  
            if (!IsInitialize) return;  //未初始化直接返回
  
            int btScancode = 0;  
            btScancode = MapVirtualKey((uint)vKeyCoad, 0);  
            KBCWait4IBE();  
            SetPortVal(KBC_KEY_CMD, (IntPtr)0xD2, 1);  
            KBCWait4IBE();  
            SetPortVal(KBC_KEY_DATA, (IntPtr)0x60, 1);  
            KBCWait4IBE();  
            SetPortVal(KBC_KEY_CMD, (IntPtr)0xD2, 1);  
            KBCWait4IBE();  
            SetPortVal(KBC_KEY_DATA, (IntPtr)btScancode, 1);  
        }  
        /// Key up  
        public static void MykeyUp(Keys vKeyCoad)  
        {  
            if (!IsInitialize) return;  
  
            int btScancode = 0;  
            btScancode = MapVirtualKey((uint)vKeyCoad, 0);  
            KBCWait4IBE();  
            SetPortVal(KBC_KEY_CMD, (IntPtr)0xD2, 1);  
            KBCWait4IBE();  
            SetPortVal(KBC_KEY_DATA, (IntPtr)0x60, 1);  
            KBCWait4IBE();  
            SetPortVal(KBC_KEY_CMD, (IntPtr)0xD2, 1);  
            KBCWait4IBE();  
            SetPortVal(KBC_KEY_DATA, (IntPtr)(btScancode | 0x80), 1);  
        }  
  
        /// Simulate mouse down  
        public static void MyMouseDown(int vKeyCoad)  
        {  
            int btScancode = 0;  
            btScancode = MapVirtualKey((byte)vKeyCoad, 0);  
            KBCWait4IBE(); // 'wait for buffer gets empty  
            SetPortVal(KBC_KEY_CMD, (IntPtr)0xD3, 1);// 'send write command  
            KBCWait4IBE();  
            SetPortVal(KBC_KEY_DATA, (IntPtr)(btScancode | 0x80), 1);// 'write in io  
        }  
        /// Simulate mouse up  
        public static void MyMouseUp(int vKeyCoad)  
        {  
            int btScancode = 0;  
            btScancode = MapVirtualKey((byte)vKeyCoad, 0);  
            KBCWait4IBE(); // 'wait for buffer gets empty  
            SetPortVal(KBC_KEY_CMD, (IntPtr)0xD3, 1); //'send write command  
            KBCWait4IBE();  
            SetPortVal(KBC_KEY_DATA, (IntPtr)(btScancode | 0x80), 1);// 'write in io  
        }  
//        ' 左键按下(MouseFun=9)。MyMouseX、MyMouseY、MyMouseZ 为0
//' 右键按下(MouseFun=10)。MyMouseX、MyMouseY、MyMouseZ 为0
//' 中键按下(MouseFun=12)。MyMouseX、MyMouseY、MyMouseZ 为0
//' 任意键放开(MouseFun=8)。MyMouseX、MyMouseY、MyMouseZ 为0
//' ------------------------------------
//' 鼠标上移(MouseFun=8)。MyMouseY为移动距离，最大为255，最小为1。MyMouseX、MyMouseZ 为0
//' 鼠标下移(MouseFun=40)。MyMouseY为移动距离，最大为1，最小为255。MyMouseX、MyMouseZ 为0
//' 鼠标左移(MouseFun=24)。MyMouseX为移动距离，最大为1，最小为255。MyMouseY、MyMouseZ 为0
//' 鼠标右移(MouseFun=8)。MyMouseX为移动距离，最大为255，最小为1。MyMouseY、MyMouseZ 为0
        public static void MyMouseKey(int fun,int x,int y,int z)
        {
            KBCWait4IBE();      //'等待缓冲区为空
            SetPortVal(100, (IntPtr)211, 1);             //'发送鼠标写入命令
            KBCWait4IBE();
            SetPortVal(96, (IntPtr)fun, 1);               //'发送鼠标动作命令

//KBCWait4IBE();    
//SetPortVal 100, 211, 1                // '发送鼠标写入命令
//KBCWait4IBE       
//SetPortVal 96, MyMouseX, 1               //  '发送鼠标动作命令

//KBCWait4IBE();     
//SetPortVal 100, 211, 1                 //'发送鼠标写入命令
//KBCWait4IBE();     
//SetPortVal 96, MyMouseY, 1               //  '发送鼠标动作命令

//KBCWait4IBE();     
//SetPortVal 100, 211, 1                // '发送鼠标写入命令
//KBCWait4IBE();    
//SetPortVal 96, MyMouseZ, 1               //  '发送鼠标动作命令
        }
    }  

}
