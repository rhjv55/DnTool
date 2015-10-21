using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace IPlugin.API
{
    public partial class Win32API
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetCurrentProcess();
        /// <summary>
        /// 读取进程的内存数据
        /// </summary>
        /// <param name="hProcess">为远程进程的句柄</param>
        /// <param name="lpBaseAddress">用于指明远程进程中的地址,从具体何处读取</param>
        /// <param name="lpBuffer">是本地进程中的内存地址, 函数将读取的内容写入此处</param>
        /// <param name="nSize">是需要传送的字节数</param>
        /// <param name="lpNumberOfBytesRead">用于指明实际传送的字节数</param>
        /// <returns></returns>
        [DllImportAttribute("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        public static extern bool ReadProcessMemory
        (
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            IntPtr lpBuffer,
            int nSize,
            out int lpNumberOfBytesRead
        );

        /// <summary>
        /// 写入某一进程的内存区域。入口区必须可以访问，否则操作将失败
        /// </summary>
        /// <param name="hProcess">由OpenProcess返回的进程句柄</param>
        /// <param name="lpBaseAddress">要写的内存首地址,再写入之前，此函数将先检查目标地址是否可用，并能容纳待写入的数据</param>
        /// <param name="lpBuffer">指向要写的数据的指针</param>
        /// <param name="nSize">要写入的字节数</param>
        /// <param name="lpNumberOfBytesWritten">实际数据的长度</param>
        /// <returns>非零值代表成功</returns>
        [DllImportAttribute("kernel32.dll", EntryPoint = "WriteProcessMemory")]
        public static extern bool WriteProcessMemory
        (
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            int nSize,
            IntPtr lpNumberOfBytesWritten
        );
        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }
        /// <summary>
        /// 用来打开一个已存在的进程对象，并返回进程的句柄。
        /// </summary>
        /// <param name="processAccess">渴望得到的访问权限（标志）</param>
        /// <param name="bInheritHandle">是否继承句柄</param>
        /// <param name="dwProcessId">进程标示符</param>
        /// <returns>
        /// 如成功，返回值为指定进程的句柄。
        /// 如失败，返回值为空，可调用GetLastError获得错误代码。
        /// </returns>
        [DllImportAttribute("kernel32.dll", EntryPoint = "OpenProcess")]
        public static extern IntPtr OpenProcess
        (
            ProcessAccessFlags processAccess,
            bool bInheritHandle,
            int dwProcessId
        );


       

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

        [DllImport("kernel32.dll")]
        public static extern uint GetTickCount();

        [DllImport("kernel32.dll", SetLastError = false)]
        public static extern void GetSystemInfo(out SYSTEM_INFO Info);

        [StructLayout(LayoutKind.Explicit)]
        public struct SYSTEM_INFO_UNION
        {
            [FieldOffset(0)]
            public UInt32 OemId;
            [FieldOffset(0)]
            public UInt16 ProcessorArchitecture;
            [FieldOffset(2)]
            public UInt16 Reserved;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SYSTEM_INFO
        {
            public SYSTEM_INFO_UNION CpuInfo;
            /// <summary>
            /// 分页大小
            /// </summary>
            public UInt32 PageSize;
            /// <summary>
            /// 最小寻址空间
            /// </summary>
            public UInt32 MinimumApplicationAddress;
            /// <summary>
            /// 最大寻址空间
            /// </summary>
            public UInt32 MaximumApplicationAddress;
            /// <summary>
            /// 处理器掩码; 0..31 表示不同的处理器
            /// </summary>
            public UInt32 ActiveProcessorMask;
            /// <summary>
            /// 处理器数目
            /// </summary>
            public UInt32 NumberOfProcessors;
            /// <summary>
            /// 处理器类型
            /// </summary>
            public UInt32 ProcessorType;
            /// <summary>
            /// 虚拟内存空间的粒度
            /// </summary>
            public UInt32 AllocationGranularity;
            /// <summary>
            /// 处理器等级
            /// </summary>
            public UInt16 ProcessorLevel;
            /// <summary>
            /// 处理器版本
            /// </summary>
            public UInt16 ProcessorRevision;
        }

        [DllImport("kernel32")]
        public static extern bool GetVersionEx(ref OSVERSIONINFO osvi);
        public struct OSVERSIONINFO
        {
            /// <summary>
            /// 初始化为结构的大小
            /// </summary>
            public uint dwOSVersionInfoSize;
            /// <summary>
            /// 系统主版本号
            /// </summary>
            public uint dwMajorVersion;   //受程序兼容性影响
            /// <summary>
            /// 系统次版本号
            /// </summary>
            public uint dwMinorVersion;
            /// <summary>
            /// 系统构建号
            /// </summary>
            public uint dwBuildNumber;
            /// <summary>
            /// 系统支持的平台
            /// </summary>
            public uint dwPlatformId;
            /// <summary>
            /// 系统补丁包的名称
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
            /// <summary>
            /// 系统补丁包的主版本
            /// </summary>
            public Int16 wServicePackMajor;
            /// <summary>
            /// 系统补丁包的次版本
            /// </summary>
            public Int16 wServicePackMinor;
            /// <summary>
            /// 标识系统上的程序组
            /// </summary>
            public Int16 wSuiteMask;
            /// <summary>
            /// 标识系统类型
            /// </summary>
            public ProductTypeFlags wProductType;  //不受兼容性影响，工作站不能变成服务器
            /// <summary>
            /// 保留,未使用
            /// </summary>
            public Byte wReserved;
        }

        public enum ProductTypeFlags:byte
        {
            VER_NT_DOMAIN_CONTROLLER=0x2,
            VER_NT_SERVER = 0x3,  //Note that a server that is also a domain controller is reported as VER_NT_DOMAIN_CONTROLLER, not VER_NT_SERVER.
            VER_NT_WORKSTATION=0x1
        }
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
