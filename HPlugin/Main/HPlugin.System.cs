using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using IPlugin.API;
using System.Runtime.InteropServices;
namespace IPlugin.Main
{
    public partial class HPlugin
    {
        /// <summary>
        /// 获取CPU个数 ,CPU核数决定线程个数
        /// </summary>
        /// <returns></returns>
        public int GetProcessNumber()
        {
            Win32API.SYSTEM_INFO info;
            Win32API.GetSystemInfo(out info);
            return (int)info.NumberOfProcessors;
        }

        public enum OSFlags : int
        {
           UnknownOs,
           Windows_XP,//5.1
           //WindowsXP_Professional_x64_Edition,//5.2
           Windows_Server_2003,//5.2
           //Windows_Home_Server,//5.2
           Windows_Server_2003_R2,//5.2
           Windows_Vista,//6.0
           Windows_Server_2008,//6.0
           Windows_Server_2008_R2,//6.1
           Windows_7,//6.1
           Windows_Server_2012,//6.2
           Windows_8,//6.2
           Windows_Server_2012_R2,//6.3
           Windows_8_1,//6.3
          // Windows_Server_2016_Technical_Preview,//10.0
           Windows_10//10.0
        }
        /// <summary>
        /// 获取系统版本
        /// </summary>
        /// <returns></returns>
        public int GetOsVersion()
        {
            var osvi = new Win32API.OSVERSIONINFO
            {
                dwOSVersionInfoSize = (uint) Marshal.SizeOf(typeof (Win32API.OSVERSIONINFO))
            };

            if (!Win32API.GetVersionEx(ref osvi)) return -1;

            const int MaxPath = 260;
            StringBuilder sb = new StringBuilder(MaxPath);
            int ret=Win32API.SHGetFolderPath(IntPtr.Zero, (int)Win32API.SpecialFolderCSIDL.CSIDL_SYSTEM, IntPtr.Zero, Win32API.SHGFP_TYPE.SHGFP_TYPE_CURRENT, sb);

            if (ret != 0) return -1;
            
            string path = Path.Combine(sb.ToString(), "kernel32.dll");
            FileVersionInfo info=FileVersionInfo.GetVersionInfo(path);
            var nMajorVersion = info.ProductMajorPart;
            var nMinorVersion = info.ProductMinorPart;
            var nBuildVersion = info.ProductBuildPart;
            var nReviVersion = info.ProductPrivatePart;

            switch (nMajorVersion) //判断主版本号 
            {
                case 5:
                    switch (nMinorVersion)  //判断此版本号
                    {
                        case 1:
                            return (int) OSFlags.Windows_XP; 
                        case 2:
                            if (Win32API.GetSystemMetrics(Win32API.SystemMetric.SM_SERVERR2) == 0)
                                return (int)OSFlags.Windows_Server_2003;
                            if (Win32API.GetSystemMetrics(Win32API.SystemMetric.SM_SERVERR2) != 0)
                                return (int)OSFlags.Windows_Server_2003_R2;
                            break;
                    }
                    break;
                case 6:
                    switch (nMinorVersion)
                    {
                        case 0:
                            if (osvi.wProductType == Win32API.ProductTypeFlags.VER_NT_WORKSTATION)
                                return (int)OSFlags.Windows_Vista;
                            else
                                return (int)OSFlags.Windows_Server_2008;
                        case 1:
                            if (osvi.wProductType == Win32API.ProductTypeFlags.VER_NT_WORKSTATION)
                                return (int)OSFlags.Windows_7;
                            else
                                return (int)OSFlags.Windows_Server_2008_R2;
                        case 2:
                            if (osvi.wProductType == Win32API.ProductTypeFlags.VER_NT_WORKSTATION)
                                return (int)OSFlags.Windows_8;
                            else
                                return (int)OSFlags.Windows_Server_2012;
                        case 3:
                            if (osvi.wProductType == Win32API.ProductTypeFlags.VER_NT_WORKSTATION)
                                return (int)OSFlags.Windows_8_1;
                            else
                                return (int)OSFlags.Windows_Server_2012_R2;
                    }
                    break;
                case 10:
                    if (osvi.wProductType == Win32API.ProductTypeFlags.VER_NT_WORKSTATION)
                        return (int)OSFlags.Windows_10;
                    break;
                default:
                    return (int)OSFlags.UnknownOs;
            }
            return -1;
        }



    }
}
