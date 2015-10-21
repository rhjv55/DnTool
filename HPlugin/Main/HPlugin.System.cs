using System;
using System.Collections.Generic;
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
           Windows_XP,//5.1
           WindowsXP_Professional_x64_Edition,//5.2
           Windows_Server_2003,//5.2
           Windows_Home_Server,//5.2
           Windows_Server_2003_R2,//5.2
           Windows_Vista,//6.0
           Windows_Server_2008,//6.0
           Windows_Server_2008_R2,//6.1
           Windows_7,//6.1
           Windows_Server_2012,//6.2
           Windows_8,//6.2
           Windows_Server_2012_R2,//6.3
           Windows_8_1,//6.3
           Windows_Server_2016_Technical_Preview,//10.0
           Windows_10//10.0
        }
        /// <summary>
        /// 获取系统版本
        /// </summary>
        /// <returns></returns>
        public int GetOSVersion()
        {
            Win32API.OSVERSIONINFO osvi = new Win32API.OSVERSIONINFO();
            osvi.dwOSVersionInfoSize = (uint)Marshal.SizeOf(typeof(Win32API.OSVERSIONINFO));

            if (Win32API.GetVersionEx(ref osvi))
            {
                switch (osvi.dwMajorVersion) //判断主版本号 
                {
                    case 5:
                        if (osvi.dwMinorVersion == 1)
                            return 1; //xp
                        if (osvi.dwMinorVersion == 2)
                        {
                            if (Win32API.GetSystemMetrics(Win32API.SystemMetric.SM_SERVERR2) == 0)
                                return 2;//2003
                            else
                                return 3;//2003r2
                        }
                        break;
                    case 6:
                        switch (osvi.dwMinorVersion)
                        {
                            case 0:
                                if (osvi.wProductType == Win32API.ProductTypeFlags.VER_NT_WORKSTATION)/*VER_NT_WORKSTATION是桌面系统 */
                                    return 4; //VISTA
                                else
                                    return 5; //服务器版本 win2008
                            case 1:
                                if (osvi.wProductType == Win32API.ProductTypeFlags.VER_NT_WORKSTATION)
                                    return 6;//win7
                                else
                                    return 7;//win2008r2
                        }
                        break;
                    default:
                        return 0;
                }
            }
            return -1;

        }



    }
}
