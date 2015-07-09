using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DnTool.Utilities
{
    public class INIHelper
    {
		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section,string key,string val,string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section,string key,string def, StringBuilder retVal,int size,string filePath);

	
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string defVal, Byte[] retVal, int size, string filePath);


	    /// <summary>
	    /// 写INI文件
	    /// </summary>
	    /// <param name="Section"></param>
	    /// <param name="Key"></param>
	    /// <param name="Value"></param>
	    /// <param name="path"></param>
	    /// <returns></returns>
		public static long IniWriteValue(string Section,string Key,string Value,string path)
		{
			return WritePrivateProfileString(Section,Key,Value,path);
		}

		/// <summary>
        /// 读取INI文件字符串
		/// </summary>
		/// <param name="Section"></param>
		/// <param name="Key"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string IniReadValue(string Section,string Key,string path)
		{
			StringBuilder temp = new StringBuilder(255);
			int i = GetPrivateProfileString(Section,Key,"",temp, 255, path);
			return temp.ToString();
		}
        /// <summary>
        /// 读取INI文件字节数组
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="path"></param>
        /// <returns></returns>
		public static byte[] IniReadValues(string section, string key,string path)
		{
			byte[] temp = new byte[255];
			int i = GetPrivateProfileString(section, key, "", temp, 255, path);
			return temp;

		}


		/// <summary>
        /// 删除ini文件下所有段落
		/// </summary>
		/// <param name="path"></param>
		public static void ClearAllSection(string path)
		{
			IniWriteValue(null,null,null,path);
		}
		/// <summary>
        /// 删除ini文件下personal段落下的所有键
		/// </summary>
		/// <param name="Section"></param>
		/// <param name="path"></param>
		public static void ClearSection(string Section,string path)
		{
			IniWriteValue(Section,null,null,path);
		}

    }
}
