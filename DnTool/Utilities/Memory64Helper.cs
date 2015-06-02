using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DnTool.Utilities
{
    public class Memory64Helper
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok);

        private const int TOKEN_QUERY = 0x00000008;

        private const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;

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

        //写内存
        [DllImportAttribute("kernel32.dll", EntryPoint = "WriteProcessMemory")]
        public static extern bool WriteProcessMemory
        (
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            int[] lpBuffer,
            int nSize,
            IntPtr lpNumberOfBytesWritten
        );

        //获取窗体的进程标识ID
        public int GetPid(string windowTitle)
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

        //根据进程名获取PID
        public int GetPidByProcessName(string processName)
        {
            Process[] arrayProcess = Process.GetProcessesByName(processName);

            foreach (Process p in arrayProcess)
            {
                return p.Id;
            }
            return 0;
        }

        //根据窗体标题查找窗口句柄（支持模糊匹配）
        public IntPtr FindWindow(string title)
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
        /// <param name="baseAddress">需要读取的地址</param>
        /// <param name="processName">进程名</param>
        /// <returns>返回 值</returns>
        public Int64 ReadMemoryValue(Int64 baseAddress, string processName)
        {
            try
            {
                byte[] buffer = new byte[4];
                IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0); //获取缓冲区地址
                int pid=GetPidByProcessName(processName);
                if(pid==0)
                    throw new Exception("");
                IntPtr hProcess = OpenProcess(0x1F0FFF, false,pid);
                ReadProcessMemory(hProcess, (IntPtr)baseAddress, byteAddress, 4, IntPtr.Zero); //将制定内存中的值读入缓冲区
                CloseHandle(hProcess);
                return Marshal.ReadInt64(byteAddress);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// 读取内存中的值
        /// </summary>
        /// <param name="baseAddress">需要读取的地址</param>
        /// <param name="Pid">进程PID</param>
        /// <returns>返回 值</returns>
        public Int64 ReadMemoryValue(Int64 baseAddress, int Pid)
        {
            try
            {
                byte[] buffer = new byte[4];
                IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0); //获取缓冲区地址
                IntPtr hProcess = OpenProcess(0x1F0FFF, false, Pid);
                ReadProcessMemory(hProcess, (IntPtr)baseAddress, byteAddress, 4, IntPtr.Zero); //将制定内存中的值读入缓冲区
                CloseHandle(hProcess);
                return Marshal.ReadInt64(byteAddress);
            }
            catch (System.Exception ex)
            {

                return 0;
            }
        }

        /// <summary>
        /// 将值写入指定内存地址中
        /// </summary>
        /// <param name="baseAddress">需要写入的地址</param>
        /// <param name="processName">进程名</param>
        /// <param name="value">写入值</param>
        public void WriteMemoryValue(Int64 baseAddress, string processName, int value)
        {
            try
            {
                IntPtr hProcess = OpenProcess(0x1F0FFF, false, GetPidByProcessName(processName)); //0x1F0FFF 最高权限
                WriteProcessMemory(hProcess, (IntPtr)baseAddress, new int[] { value }, 4, IntPtr.Zero);
                CloseHandle(hProcess);
            }
            catch (System.Exception ex)
            {

            }
        }

        /// <summary>
        /// 将值写入指定内存地址中
        /// </summary>
        /// <param name="baseAddress">需要写入的地址</param>
        /// <param name="Pid">进程PID</param>
        /// <param name="value">写入值</param>
        public void WriteMemoryValue(Int64 baseAddress, int Pid, int value)
        {
            try
            {
                IntPtr hProcess = OpenProcess(0x1F0FFF, false, Pid); //0x1F0FFF 最高权限
                WriteProcessMemory(hProcess, (IntPtr)baseAddress, new int[] { value }, 4, IntPtr.Zero);
                CloseHandle(hProcess);
            }
            catch (System.Exception ex)
            {

            }
        }

    }
}
