using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DnTool.Utilities
{
    public class MemHelper
    {
        #region API
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok);

        [DllImportAttribute("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        public static extern bool ReadProcessMemory
        (
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            IntPtr lpBuffer,
            int nSize,
            out int lpNumberOfBytesRead
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

        [DllImportAttribute("kernel32.dll", EntryPoint = "OpenProcess")]
        public static extern IntPtr OpenProcess
        (
            int dwDesiredAccess,
            bool bInheritHandle,
            int dwProcessId
        );


        /// <summary>
        ///把dest所指内存区域的前count个字节设置成字符c。
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="c"></param>
        /// <param name="count"></param>
        /// <returns>返回指向dest的指针</returns>
        [DllImport("msvcrt.dll", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr MemSet(IntPtr dest, int c, int count);

        public const int TOKEN_QUERY = 0x00000008;

        public const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
        public const uint TH32CS_SNAPPROCESS = 0x00000002;
        public const uint TH32CS_SNAPNOHEAPS = 0x40000000;
        public const uint TH32CS_SNAPHEAPLIST = 0x00000001;
        public const uint TH32CS_SNAPTHREAD = 0x00000004;
        public const uint TH32CS_SNAPMODULE = 0x00000008;
        public const uint TH32CS_SNAPALL = (TH32CS_SNAPHEAPLIST | TH32CS_SNAPPROCESS | TH32CS_SNAPTHREAD | TH32CS_SNAPMODULE);
        public const uint TH32CS_GETALLMODS = 0x80000000;

        [DllImport("kernel32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle([In] IntPtr hObject);

        [DllImport("Kernel32.dll")]
        public static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, IntPtr th32ProcessID);

        [DllImport("kernel32.dll")]
        public static extern bool Module32First(IntPtr hSnapshot, ref MODULEENTRY32 lpme);

        [DllImport("kernel32.dll")]
        public static extern bool Module32Next(IntPtr hSnapshot, ref MODULEENTRY32 lpme);

        public struct MODULEENTRY32
        {
            private const int MAX_PATH = 255;
            internal uint dwSize;
            internal uint th32ModuleID;
            internal uint th32ProcessID;
            internal uint GlblcntUsage;
            internal uint ProccntUsage;
            internal IntPtr modBaseAddr;
            internal uint modBaseSize;
            internal IntPtr hModule;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH + 1)]
            internal string szModule;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH + 5)]
            internal string szExePath;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct PROCESSENTRY32
        {
            const int MAX_PATH = 260;
            internal UInt32 dwSize;
            internal UInt32 cntUsage;
            internal UInt32 th32ProcessID;
            internal IntPtr th32DefaultHeapID;
            internal UInt32 th32ModuleID;
            internal UInt32 cntThreads;
            internal UInt32 th32ParentProcessID;
            internal Int32 pcPriClassBase;
            internal UInt32 dwFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            internal string szExeFile;
        }
        #endregion

        /// <summary>
        /// 读取内存中的值4字节
        /// </summary>
        /// <param name="baseAddress">需要读取的地址</param>
        /// <returns>返回 值</returns>
        public Int32 ReadMemory32(int pid,Int32 baseAddress)
        {
            try
            {
                byte[] buffer = new byte[4];
                IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0); //获取缓冲区地址
                IntPtr hProcess = OpenProcess(0x1F0FFF, false, pid);
                int read;
                bool ret = ReadProcessMemory(hProcess, (IntPtr)baseAddress, byteAddress, 4, out read); //将指定内存中的值读入缓冲区
                CloseHandle(hProcess);
                return ret ? Marshal.ReadInt32(byteAddress) : 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取模块地址
        /// </summary>
        /// <param name="PID">目标进程PID</param>
        /// <param name="ModuleName">需获取到的模块名</param>
        /// <returns>返回个int类型的吧.想怎么转换看你们自己了.</returns>
        private int GetModelAddress(IntPtr PID, string ModuleName)
        {
            PROCESSENTRY32 pr = new PROCESSENTRY32();
            MODULEENTRY32 mo = new MODULEENTRY32();
            IntPtr LM;
            if (ModuleName == "")
            {
                //如果模块空,直接88 返回-2 因为2..
                return -2;
            }
            pr.dwSize = (uint)Marshal.SizeOf(typeof(PROCESSENTRY32));
            LM = CreateToolhelp32Snapshot(TH32CS_SNAPMODULE, PID);
            if (LM.ToInt32() > 0)
            {
                mo.dwSize = (uint)Marshal.SizeOf(typeof(MODULEENTRY32));
                if (Module32First(LM, ref mo))
                {
                    do
                    {
                        if (mo.szModule == ModuleName)
                        {
                            CloseHandle(LM);
                            return mo.modBaseAddr.ToInt32();
                        }
                    }
                    while (Module32Next(LM, ref mo));
                }
                CloseHandle(LM);
            }
            //获取不到.或者遍历不到.都返回-1
            return -1;
        }

        /// <summary>
        /// 读取内存中的值8字节
        /// </summary>
        /// <param name="baseAddress">需要读取的地址</param>
        /// <returns>返回 值</returns>
        public Int64 ReadMemory64(int pid,Int32 baseAddress)
        {
            try
            {
                byte[] buffer = new byte[8];
                IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0); //获取缓冲区地址
                IntPtr hProcess = OpenProcess(0x1F0FFF, false, pid);
                int read = 0;
                bool ret = ReadProcessMemory(hProcess, (IntPtr)baseAddress, byteAddress, 8, out read); //将制定内存中的值读入缓冲区
                CloseHandle(hProcess);
                return ret ? Marshal.ReadInt64(byteAddress) : 0;
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 读取内存中的值
        /// </summary>
        /// <param name="baseAddress">需要读取的地址[[[1711110]+a]+b]+c</param>
        /// <returns>返回 值</returns>
        public Int64 ReadMemoryValue(string address)
        {
            try
            {

                return 1;
            }
            catch
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
        public int WriteMemoryValue(int pid,Int64 baseAddress, int value)
        {
            try
            {
                IntPtr hProcess = OpenProcess(0x1F0FFF, false, pid); //0x1F0FFF 最高权限
                WriteProcessMemory(hProcess, (IntPtr)baseAddress, new int[] { value }, 4, IntPtr.Zero);
                CloseHandle(hProcess);
                return 1;
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return 0;
            }
        }

        private int MAX_PATH = 260; //字符串最大长度

        /// <summary>
        /// 检查指针是否有效
        /// </summary>
        /// <param name="strs"></param>
        /// <returns>-1为无效,返回最后指针</returns>
        public int GetFindDataAddr(string strs, int pid)
        {
            int readaddr = 0;
            int longAdd = 0;           //返回最后指针
            char[] module = new char[MAX_PATH];//记录模块的名字
            int moduleindex = 0; //记录模块长度
            int nindex = 0; //记录指针个数
            int noffset = 0;
            int index = 0;
            bool[] subindex = new bool[MAX_PATH]; //标记"-"号的位置:true,默认是"+":false
            char[,] offsetaddrstr = new char[MAX_PATH, 16]; //存储偏移地址
            int[] offsetaddr = new int[MAX_PATH]; //存储指针

            int len = strs.Length;
            if (len < 1)
            {
                longAdd = -1;
                return longAdd;
            }
            for (int i = 0; i < len; i++)
            {
                if (strs[i] == '<' || moduleindex > 0)         //获取模块名字
                {
                    if (strs[i] == '>')
                    {
                        moduleindex = -1;
                    }
                    else if (strs[i + 1] != '>')
                    {
                        module[moduleindex] = strs[i + 1];
                        moduleindex++;
                    }
                }
                if (strs[i] == '>' && moduleindex == 0)        //模块名字错误
                {
                    longAdd = -1;
                    return longAdd;
                }
                if (strs[i] == '[')
                    nindex++;                                  //记录指针个数
                else if (((strs[i] == '+' || index > 0) || (strs[i] == '-' || index > 0) || moduleindex == 0))
                {
                    if (strs[i] == '-')                       //记录-号的下标
                        subindex[noffset] = true;
                    if (strs[i] == ']')
                    {
                        index = 0;
                        noffset++;
                    }
                    else if (strs[i] != ']' && (strs[i] == '+' || strs[i] == '-'))
                    {
                        offsetaddrstr[noffset, index] = strs[i + 1];
                        index++;
                        i++;
                    }
                    else if (strs[i] != ']')
                    {
                        offsetaddrstr[noffset, index] = strs[i];
                        index++;
                    }
                }
                else if ((noffset == nindex)) //记录最后一个偏移地址
                {
                    if (strs[i] == '-') //记录-号的下标
                        subindex[noffset] = true;
                    if (strs[i] == '+' || strs[i] == '-')
                    {
                        offsetaddrstr[noffset, index] = strs[i + 1];
                        index++;
                    }
                    else if (index > 0)
                    {
                        offsetaddrstr[noffset, index] = strs[i + 1];
                        index++;
                    }

                }
            }

            int Baseaddr = 0;
            if (moduleindex == -1) //说明有传入<modulename>
            {
                String moduleName = new String(module);
                moduleName = moduleName.Replace(new String(new Char[] { '\0' }), "");
                Baseaddr = GetModelAddress((IntPtr)pid, moduleName);
                if (Baseaddr <= 0)
                {
                    longAdd = -1;
                    return longAdd;
                }
            }

            if (nindex > 0) //说明不是一级指针
            {
                for (int i = 0; i < nindex; i++)
                {
                    string s = "";
                    for (int j = 0; j < 16; j++)
                    {
                        if (offsetaddrstr[i, j] != '\0')
                            s = s + offsetaddrstr[i, j];
                    }
                    offsetaddr[i] = Convert.ToInt32(s, 16);
                    if (i == 0 && Baseaddr > 0)
                        readaddr = Baseaddr + offsetaddr[i];
                    else if (i == 0)
                        readaddr = offsetaddr[i];
                    else if (readaddr != 0)
                    {
                        if (subindex[i])
                            readaddr = longAdd - offsetaddr[i];
                        else
                            readaddr = longAdd + offsetaddr[i];
                    }

                    byte[] buffer = new byte[4];
                    IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0); //获取缓冲区地址
                    IntPtr hProcess = OpenProcess(0x1F0FFF, false, pid);
                    int read = 0;
                    bool ret = ReadProcessMemory(hProcess, (IntPtr)readaddr, byteAddress, 4, out read); //将指定内存中的值读入缓冲区
                    CloseHandle(hProcess);
                    if (ret == false || read <= 0)
                        return -1;
                    longAdd = Marshal.ReadInt32(byteAddress);

                    if (longAdd == 0) //说明读取错误地址
                    {
                        longAdd = -1;
                        return longAdd;
                    }

                }
                string st = "";
                for (int j = 0; j < 16; j++)
                {
                    if (offsetaddrstr[nindex, j] != '\0')
                        st = st + offsetaddrstr[nindex, j];
                }
                offsetaddr[nindex] = Convert.ToInt32(st, 16); //最后一个偏移
                if (subindex[nindex])
                    longAdd = longAdd - offsetaddr[nindex];
                else
                    longAdd = longAdd + offsetaddr[nindex];

            }
            else if (moduleindex != -1) //1367DBC   都是数值
                longAdd = Convert.ToInt32(strs, 16);
            else if (moduleindex == -1) //<xx.exe>+1367DBC    模块地址+数值
            {
                string s = "";
                for (int i = 0; i < 16; i++)
                {
                    if (offsetaddrstr[0, i] != '\0')
                        s = s + offsetaddrstr[0, i];
                }
                longAdd = Convert.ToInt32(s, 16);
                longAdd = Baseaddr + longAdd;
            }
            return longAdd;            //返回最后地址;
        }



        /// <summary>
        /// 读取指定字节的数据
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="addr"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public byte[] ReadData(int pid, string addr, int len)
        {
            if (len < 0 || len > MAX_PATH)
                return null;
            int findaddr = GetFindDataAddr(addr, pid);  //返回最后指针
            if (findaddr == -1)
                return null;
            byte[] finddata = new byte[len];
            int dwread = 0;
            IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(finddata, 0); //获取缓冲区地址
            IntPtr hProcess = OpenProcess(0x1F0FFF, false, pid);
            bool ret = ReadProcessMemory(hProcess, (IntPtr)findaddr, byteAddress, len, out dwread); //将制定内存中的值读入缓冲区
            CloseHandle(hProcess);
            return finddata;

        }

        /// <summary>
        /// 读取指定地址的整数数值，类型可以是8位，16位 或者 32位
        /// </summary>
        /// <param name="pid">进程ID</param>
        /// <param name="addr">
        /// 用字符串来描述地址，类似于CE的地址描述，数值必须是16进制,里面可以用[ ] + -这些符号来描述一个地址。+表示地址加，-表示地址减
        /// 模块名必须用<>符号来圈起来
        /// 例如:
        /// "4DA678" 最简单的方式，用绝对数值来表示地址
        /// "<GAME.exe>+DA678" 相对简单的方式，只是这里用模块名来决定模块基址，后面的是偏移
        /// "[4DA678]+3A" 用绝对数值加偏移，相当于一级指针
        /// "[<GAME.exe>+DA678]+3A" 用模块定基址的方式，也是一级指针
        /// "[[[<GAME.exe>+DA678]+3A]+5B]+8" 这个是一个三级指针
        /// </param>
        /// <param name="type"> 0 : 32位   1 : 16 位  2 : 8位</param>
        /// <returns>读取到的数值,注意这里无法判断读取是否成功</returns>
        public int ReadInt(int pid, string addr, int type)
        {
            int findaddr = GetFindDataAddr(addr, pid);
            if (findaddr == -1)
                return -1;
            int dwread = 0;
            byte[] buffer = new byte[4];
            IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0); //获取缓冲区地址
            IntPtr hProcess = OpenProcess(0x1F0FFF, false, pid);
            bool ret = ReadProcessMemory(hProcess, (IntPtr)findaddr, byteAddress, 4, out dwread); //将指定内存中的值读入缓冲区
            CloseHandle(hProcess);
            if (type == 0) //0 : 32位
            {
                return BitConverter.ToInt32(buffer, 0);
            }
            else if (type == 1)  //1 : 16 位
            {
                return BitConverter.ToInt16(buffer, 0);
            }
            else if (type == 2) //2 : 8位
            {
                return Convert.ToInt32(buffer[0]);
            }
            return -1;
        }
        /// <summary>
        /// 读取指定地址的字符串，可以是GBK字符串或者是Unicode字符串.(必须事先知道内存区的字符串编码方式)
        /// </summary>
        /// <param name="pid">进程ID</param>
        /// <param name="addr">
        /// 用字符串来描述地址，类似于CE的地址描述，数值必须是16进制,里面可以用[ ] + -这些符号来描述一个地址。+表示地址加，-表示地址减
        /// 模块名必须用<>符号来圈起来
        /// 例如:
        /// "4DA678" 最简单的方式，用绝对数值来表示地址
        /// "<GAME.exe>+DA678" 相对简单的方式，只是这里用模块名来决定模块基址，后面的是偏移
        /// "[4DA678]+3A" 用绝对数值加偏移，相当于一级指针
        /// "[<GAME.exe>+DA678]+3A" 用模块定基址的方式，也是一级指针
        /// "[[[<GAME.exe>+DA678]+3A]+5B]+8" 这个是一个三级指针
        /// </param>
        /// <param name="type">整型数: 字符串类型,取值如下 0 : ASCII字符串 1 : Unicode字符串 2：UTF8字符串</param>
        /// <param name="len">需要读取的字节数目</param>
        /// <returns>读取到的字符串,注意这里无法判断读取是否成功</returns>
        public string ReadString(int pid, string addr, int type, int len)
        {
            if (len > MAX_PATH || len <= 0)
                return "";
            int findaddr = GetFindDataAddr(addr, pid);
            if (findaddr == -1)
                return "";
            int dwread = 0;
            byte[] buffer = new byte[len];
            IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0); //获取缓冲区地址
            IntPtr hProcess = OpenProcess(0x1F0FFF, false, pid);
            bool ret = ReadProcessMemory(hProcess, (IntPtr)findaddr, byteAddress, len, out dwread); //将指定内存中的值读入缓冲区
            CloseHandle(hProcess);
            if (type == 0)//0 :ASCII字符串
            {
                return Encoding.ASCII.GetString(buffer);
            }
            else if (type == 1)//1 :Unicode字符串
            {
                return Encoding.Unicode.GetString(buffer);
            }
            else if (type == 2)//2：UTF8字符串
            {
                return Encoding.UTF8.GetString(buffer);
            }
            return "";
        }
        /// <summary>
        /// 读取指定地址的单精度浮点数
        /// </summary>
        /// <param name="pid">进程ID</param>
        /// <param name="addr">
        /// 用字符串来描述地址，类似于CE的地址描述，数值必须是16进制,里面可以用[ ] + -这些符号来描述一个地址。+表示地址加，-表示地址减
        /// 模块名必须用<>符号来圈起来
        /// 例如:
        /// "4DA678" 最简单的方式，用绝对数值来表示地址
        /// "<GAME.exe>+DA678" 相对简单的方式，只是这里用模块名来决定模块基址，后面的是偏移
        /// "[4DA678]+3A" 用绝对数值加偏移，相当于一级指针
        /// "[<GAME.exe>+DA678]+3A" 用模块定基址的方式，也是一级指针
        /// "[[[<GAME.exe>+DA678]+3A]+5B]+8" 这个是一个三级指针
        /// </param>
        /// <returns>读取到的数值,注意这里无法判断读取是否成功</returns>
        public float ReadFloat(int pid, string addr)
        {
            int findaddr = GetFindDataAddr(addr, pid);
            if (findaddr == -1)
                return 0;
            int dwread = 0;
            byte[] buffer = new byte[4];
            IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0); //获取缓冲区地址
            IntPtr hProcess = OpenProcess(0x1F0FFF, false, pid);
            bool ret = ReadProcessMemory(hProcess, (IntPtr)findaddr, byteAddress, 4, out dwread); //将指定内存中的值读入缓冲区
            CloseHandle(hProcess);
            return BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>
        /// 读取指定地址的双精度浮点数
        /// </summary>
        /// <param name="pid">进程ID</param>
        /// <param name="addr">
        /// 用字符串来描述地址，类似于CE的地址描述，数值必须是16进制,里面可以用[ ] + -这些符号来描述一个地址。+表示地址加，-表示地址减
        /// 模块名必须用<>符号来圈起来
        /// 例如:
        /// "4DA678" 最简单的方式，用绝对数值来表示地址
        /// "<GAME.exe>+DA678" 相对简单的方式，只是这里用模块名来决定模块基址，后面的是偏移
        /// "[4DA678]+3A" 用绝对数值加偏移，相当于一级指针
        /// "[<GAME.exe>+DA678]+3A" 用模块定基址的方式，也是一级指针
        /// "[[[<GAME.exe>+DA678]+3A]+5B]+8" 这个是一个三级指针
        /// </param>
        /// <returns>读取到的数值,注意这里无法判断读取是否成功</returns>
        public double ReadDouble(int pid, string addr)
        {
            int findaddr = GetFindDataAddr(addr, pid);
            if (findaddr == -1)
                return 0;
            int dwread = 0;
            byte[] buffer = new byte[8];
            IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0); //获取缓冲区地址
            IntPtr hProcess = OpenProcess(0x1F0FFF, false, pid);
            bool ret = ReadProcessMemory(hProcess, (IntPtr)findaddr, byteAddress, 8, out dwread); //将指定内存中的值读入缓冲区
            CloseHandle(hProcess);
            return BitConverter.ToDouble(buffer, 0);
        }

        public long ReadLong(int pid, string addr)
        {
            int findaddr = GetFindDataAddr(addr, pid);
            if (findaddr == -1)
                return 0;
            int dwread = 0;
            byte[] buffer = new byte[8];
            IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0); //获取缓冲区地址
            IntPtr hProcess = OpenProcess(0x1F0FFF, false, pid);
            bool ret = ReadProcessMemory(hProcess, (IntPtr)findaddr, byteAddress, 8, out dwread); //将指定内存中的值读入缓冲区
            CloseHandle(hProcess);
            return BitConverter.ToInt64(buffer, 0);
        }
    }
}
