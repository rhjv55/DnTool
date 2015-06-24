using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DnTool.Utilities.HYX
{
    public class HYXPlugin
    {
        public HYXPlugin()
        {
           
        }
        /// <summary>
        /// 根据进程名获得窗口句柄 
        /// </summary>
        /// <param name="ProssName">进程名,区分大小写，不需要带上进程后缀</param>
        /// <returns></returns>
        public static List<IntPtr> EnumWindowByProcessName(string ProssName)
        {
            List<IntPtr> list = new List<IntPtr>();
            try
            {
                Process[] pp = Process.GetProcessesByName(ProssName);
                for (int i = 0; i < pp.Length; i++)
                {
                    if (pp[i].ProcessName == ProssName)
                    {
                        list.Add(pp[i].MainWindowHandle);
                    }
                }
            }
            catch
            {  
            }
            return list;
        }

 
    
    }
}
