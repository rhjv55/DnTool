using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DnTool.Utilities
{
    public class MemoryHelper
    {
       
        [DllImportAttribute("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        public static extern bool ReadProcessMemory
            (
                IntPtr hProcess,
                IntPtr lpBaseAddress,
                IntPtr lpBuffer,
                int nSize,
                IntPtr lpNumberOfBytesRead
            );

        [DllImportAttribute("kernel32.dll", EntryPoint = "OpenProcess")]
        public static extern IntPtr OpenProcess
            (
                int dwDesiredAccess, 
                bool bInheritHandle, 
                int dwProcessId
            );

        [DllImport("kernel32.dll")]
        private static extern void CloseHandle
            (
                IntPtr hObject
            );

       /// <summary>
       /// 写内存
       /// </summary>
       /// <param name="hProcess"></param>
       /// <param name="lpBaseAddress"></param>
       /// <param name="lpBuffer"></param>
       /// <param name="nSize"></param>
       /// <param name="lpNumberOfBytesWritten"></param>
       /// <returns></returns>
        [DllImportAttribute("kernel32.dll", EntryPoint = "WriteProcessMemory")]
        public static extern bool WriteProcessMemory
            (
                IntPtr hProcess, 
                IntPtr lpBaseAddress, 
                int[] lpBuffer, 
                int nSize, 
                IntPtr lpNumberOfBytesWritten
            );

        /// <summary>
        /// 获取窗体的进程标识ID
        /// </summary>
        /// <param name="windowTitle"></param>
        /// <returns></returns>
        public static int GetPid(string windowTitle)
        {
            int rs = 0;
            Process[] arrayProcess = Process.GetProcesses();
            foreach (Process p in arrayProcess)
            {
                if (p.MainWindowTitle.IndexOf(windowTitle) != -1)
                {
                    rs = p.Id;
                    break;
                }
            }

            return rs;
        }

        /// <summary>
        /// 根据进程名获取PID
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static int GetPidByProcessName(string processName)
        {
            Process[] arrayProcess = Process.GetProcessesByName(processName);

            foreach (Process p in arrayProcess)
            {
                return p.Id;
            }
            return 0;
        }

        /// <summary>
        /// 根据窗体标题查找窗口句柄（支持模糊匹配）
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static IntPtr FindWindow(string title)
        {
            Process[] ps = Process.GetProcesses();
            foreach (Process p in ps)
            {
                if (p.MainWindowTitle.IndexOf(title) != -1)
                {
                    return p.MainWindowHandle;
                }
            }
            return IntPtr.Zero;
        }

        /// <summary>
        /// 读取内存中的值
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static int ReadMemoryValue(int baseAddress,string processName)
        {
            try
            {
                byte[] buffer = new byte[4];
                IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0); //获取缓冲区地址
                IntPtr hProcess = OpenProcess(0x1F0FFF, false, GetPidByProcessName(processName));
                ReadProcessMemory(hProcess, (IntPtr)baseAddress, byteAddress, 4, IntPtr.Zero); //将制定内存中的值读入缓冲区
                CloseHandle(hProcess);
                return Marshal.ReadInt32(byteAddress);
            }
            catch 
            {
                return 0;
            }
        }

        /// <summary>
        /// 将值写入指定内存地址中
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <param name="processName"></param>
        /// <param name="value"></param>
        public static void WriteMemoryValue(int baseAddress, string processName, int value)
        {
            IntPtr hProcess = OpenProcess(0x1F0FFF, false, GetPidByProcessName(processName)); //0x1F0FFF 最高权限
            WriteProcessMemory(hProcess, (IntPtr)baseAddress, new int[] { value }, 4, IntPtr.Zero);
            CloseHandle(hProcess);
        }
   

    }
}
