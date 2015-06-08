using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DnTool.Utilities.Keypad
{
    public class WinIo  
    {  
        public const int KBC_KEY_CMD = 0x64;  
        public const int KBC_KEY_DATA = 0x60;  
          
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
  
        public static void Initialize()  
        {  
            if (InitializeWinIo())  
            {  
                KBCWait4IBE(); 
                IsInitialize = true;  
            }  
            else
                System.Windows.Forms.MessageBox.Show("WinIo Initialize failed");  
  
        }  
        public static void Shutdown()  
        {  
            if (IsInitialize)  
                ShutdownWinIo();  
            IsInitialize = false;  
        }  
        ///Wait for Buffer gets empty  
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
        public static void MykeyDown(VKKey vKeyCoad)  
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
            SetPortVal(KBC_KEY_DATA, (IntPtr)btScancode, 1);  
        }  
        /// Key up  
        public static void MykeyUp(VKKey vKeyCoad)  
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
    }  

}
