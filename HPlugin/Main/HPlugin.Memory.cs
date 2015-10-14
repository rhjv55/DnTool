using IPlugin.API;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace IPlugin.Main
{
    /// <summary>
    /// 内存操作
    /// </summary>
    public partial class HPlugin
    {

        private int MAX_PATH = 260; //字符串最大长度

        #region 读内存操作
        /// <summary>
        /// 读取内存中指定字节的数据
        /// </summary>
        /// <param name="baseAddress">需要读取的地址</param>
        /// <param name="pid">进程PID</param>
        /// <param name="value">读取到的数据</param>
        /// <returns>读取成功返回true，失败返回false</returns>
        private bool ReadMemoryValue(Int32 baseAddress, int pid, int len, out byte[] value)
        {
            try
            {
                byte[] buffer = new byte[len];
                IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0); //获取缓冲区地址
                IntPtr hProcess = Win32API.OpenProcess(Win32API.ProcessAccessFlags.All, false, pid);
                if (hProcess == IntPtr.Zero)
                {
                    value = new byte[len];
                    return false;
                }
                int read;
                bool ret = Win32API.ReadProcessMemory(hProcess, (IntPtr)baseAddress, byteAddress, len, out read); //将指定内存中的值读入缓冲区
                Win32API.CloseHandle(hProcess);
                if (ret == false)
                {
                    value = new byte[len];
                    return false;
                }
                value = buffer;
                return true;
            }
            catch
            {
                value = new byte[len];
                return false;
            }
        }

       	public byte[] ReadData(int pid,string addr,int len)
		{
            byte[] finddata=new byte[len];
            if (len < 0 || len > MAX_PATH)
            {
                return finddata;
            }
			int findaddr = GetFindDataAddr(addr,pid);  //返回最后指针
            if (findaddr == -1)
            {
                return finddata;
            }
            bool ret=ReadMemoryValue(findaddr,pid,len,out finddata);
            return finddata;
		}
     
        public int ReadInt(int pid,string addr,int type)
        {
	        int findaddr=GetFindDataAddr(addr,pid);
	        if(findaddr==-1)
	     	   return -1;
            byte[] buffer = new byte[4];
            bool ret=ReadMemoryValue(findaddr,pid,4,out buffer);
            if (ret == false)
                return -1;
            if (type == 0) //0 : 32位
	        {
              return BitConverter.ToInt32(buffer,0);
	        }
            else if (type == 1)  //1 : 16 位
	        {
              return BitConverter.ToInt16(buffer,0);
	        }
            else if (type == 2) //2 : 8位
	        {
                return Convert.ToInt32(buffer[0]);
	        }
            return -1;
        }
       
        public string ReadString(int pid,string addr,int type,int len)
        {
	        if(len>MAX_PATH||len<=0)
	        	return "";
	        int findaddr=GetFindDataAddr(addr,pid);
	        if(findaddr==-1)
	        	return "";
            byte[] buffer = new byte[len];
            bool ret = ReadMemoryValue(findaddr, pid, len, out buffer);
            if (ret == false)
                return "";
	        if(type==0)//0 :ASCII字符串
	        {
               return Encoding.ASCII.GetString(buffer);
	        }
	        else if(type==1)//1 :Unicode字符串
	        {
                return Encoding.Unicode.GetString(buffer);
	        }
            else if (type == 2)//2：UTF8字符串
            {
                return Encoding.UTF8.GetString(buffer);
            }
            return "";
        }

        public float ReadFloat(int pid,string addr)
        {
	        int findaddr=GetFindDataAddr(addr,pid);
	        if(findaddr==-1)
	        	return -1;
            byte[] buffer = new byte[4];
            bool ret = ReadMemoryValue(findaddr, pid, 4, out buffer);
            if (ret == false)
                return -1;
            return BitConverter.ToSingle(buffer,0);
        }

        public double ReadDouble(int pid, string addr)
        {
            int findaddr = GetFindDataAddr(addr, pid);
            if (findaddr == -1)
                return -1;
            byte[] buffer = new byte[8];
            bool ret = ReadMemoryValue(findaddr, pid, 8, out buffer);
            if (ret == false)
                return -1;
            return BitConverter.ToDouble(buffer, 0);
        }

        public long ReadLong(int pid, string addr)
        {
            int findaddr = GetFindDataAddr(addr, pid);
            if (findaddr == -1)
                return -1;
            byte[] buffer = new byte[8];
            bool ret = ReadMemoryValue(findaddr, pid, 8, out buffer);
            if (ret == false)
                return -1;
            return BitConverter.ToInt64(buffer, 0);
        }
   
        #endregion

        #region 写内存操作
        /// <summary>
        /// 将值写入指定内存地址中
        /// </summary>
        /// <param name="baseAddress">需要写入的地址</param>
        /// <param name="processName">进程名</param>
        /// <param name="value">写入的值</param>
        private bool WriteMemoryValue(Int32 baseAddress, int pid, byte[] value)
        {
            try
            {
                IntPtr hProcess = Win32API.OpenProcess(Win32API.ProcessAccessFlags.All, false, pid); //0x1F0FFF 最高权限
                if (hProcess == IntPtr.Zero)
                    return false;
                bool ret = Win32API.WriteProcessMemory(hProcess, (IntPtr)baseAddress, value, value.Length, IntPtr.Zero);
                Win32API.CloseHandle(hProcess);
                return ret;
            }
            catch
            {
                return false;
            }
        }

        public bool WriteData(int pid,string addr,byte[] value)
        {
            int findaddr = GetFindDataAddr(addr, pid);
            if (findaddr == -1)
                return false;
           return WriteMemoryValue(findaddr, pid,value);
        }
        public bool WriteInt(int pid, string addr,int type,int value)
        {
            int findaddr = GetFindDataAddr(addr, pid);
            if (findaddr == -1)
                return false;
            byte[] buffer;
            if (type == 0)
                buffer = BitConverter.GetBytes(value);
            else if (type == 1)
                buffer = BitConverter.GetBytes(Convert.ToInt16(value));
            else if (type == 2)
                buffer = BitConverter.GetBytes(Convert.ToSByte(value));
            else
                return false;
            return WriteMemoryValue(findaddr, pid, buffer);
        }

        public bool WriteDouble(int pid, string addr, double value)
        {
            int findaddr = GetFindDataAddr(addr, pid);
            if (findaddr == -1)
                return false;
            byte[] buffer = BitConverter.GetBytes(value);

            return WriteMemoryValue(findaddr, pid, buffer);
        }
        public bool WriteFloat(int pid, string addr, float value)
        {
            int findaddr = GetFindDataAddr(addr, pid);
            if (findaddr == -1)
                return false;
            byte[] buffer = BitConverter.GetBytes(value);

            return WriteMemoryValue(findaddr, pid, buffer);
        }
        public bool WriteString(int pid, string addr,int type, string value)
        {
            int findaddr = GetFindDataAddr(addr, pid);
            if (findaddr == -1)
                return false;
            byte[] buffer;
            if (type == 0)
                buffer = Encoding.ASCII.GetBytes(value);
            else if (type == 1)
                buffer = Encoding.Unicode.GetBytes(value);
            else if (type == 2)
                buffer = Encoding.UTF8.GetBytes(value);
            else
                return false;
            return WriteMemoryValue(findaddr, pid, buffer);
        }

        #endregion
       



        /// <summary>
        /// 获取模块地址
        /// </summary>
        /// <param name="pid">目标进程PID</param>
        /// <param name="moduleName">需获取到的模块名</param>
        /// <returns>返回个int类型的吧.想怎么转换看你们自己了.</returns>
        public int GetModelBaseAddr(int pid, string moduleName)
        {
            Win32API.PROCESSENTRY32 pr = new Win32API.PROCESSENTRY32();
            Win32API.MODULEENTRY32 mo = new Win32API.MODULEENTRY32();
            IntPtr LM;
            if (string.IsNullOrEmpty(moduleName))
                return -1;
            pr.dwSize = (uint)Marshal.SizeOf(typeof(Win32API.PROCESSENTRY32));
            LM = Win32API.CreateToolhelp32Snapshot(Win32API.TH32CS_SNAPMODULE, (IntPtr)pid);
            if (LM.ToInt32() > 0)
            {
                mo.dwSize = (uint)Marshal.SizeOf(typeof(Win32API.MODULEENTRY32));
                if (Win32API.Module32First(LM, ref mo))
                {
                    do
                    {
                        if (mo.szModule == moduleName)
                        {
                            Win32API.CloseHandle(LM);
                            return mo.modBaseAddr.ToInt32();
                        }
                    }
                    while (Win32API.Module32Next(LM, ref mo));
                }
                Win32API.CloseHandle(LM);
            }
            //获取不到.或者遍历不到.都返回-1
            return -1;
        }


       /// <summary>
       /// 检查指针是否有效
       /// </summary>
       /// <param name="strs"></param>
       /// <returns>-1为无效,返回最后指针</returns>
       private int GetFindDataAddr(string strs,int pid)
	   {
		int readaddr = 0;
		int longAdd = 0;           //返回最后指针
		char[] module = new char[MAX_PATH];//记录模块的名字
		int moduleindex = 0; //记录模块长度
		int nindex = 0; //记录指针个数
		int noffset = 0;
		int index = 0;
		bool[] subindex = new bool[MAX_PATH]; //标记"-"号的位置:true,默认是"+":false
		char[,] offsetaddrstr=new char[MAX_PATH,16]; //存储偏移地址
		int[] offsetaddr = new int[MAX_PATH]; //存储指针
          
		int len = strs.Length;          
		if (len < 1)
		{
			longAdd = -1;
			return longAdd;
		}
		for (int i = 0;i < len;i++)
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
            Baseaddr = GetModelBaseAddr(pid, moduleName);
            if (Baseaddr <= 0)
            {
                longAdd = -1;
                return longAdd;
            }
        }

        if (nindex > 0) //说明不是一级指针
        {
            for (int i = 0;i < nindex;i++)
            {
                 string s="";
                  for (int j = 0; j < 16; j++)
                   {
                      if(offsetaddrstr[i,j]!='\0')
                       s = s + offsetaddrstr[i, j];
                  }
                offsetaddr[i] =Convert.ToInt32(s, 16);
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
                IntPtr hProcess = Win32API.OpenProcess(Win32API.ProcessAccessFlags.All, false, pid);
                int read = 0;
                bool ret = Win32API.ReadProcessMemory(hProcess, (IntPtr)readaddr, byteAddress, 4, out read); //将指定内存中的值读入缓冲区
                Win32API.CloseHandle(hProcess);
                if (ret == false||read<=0)
                    return -1;
                longAdd=Marshal.ReadInt32(byteAddress);

                if (longAdd== 0) //说明读取错误地址
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
            longAdd =Convert.ToInt32(strs,16);
        else if (moduleindex == -1) //<xx.exe>+1367DBC    模块地址+数值
        {
            string s="";
            for (int i = 0; i < 16; i++)
            {
                if(offsetaddrstr[0,i]!='\0')
                  s = s + offsetaddrstr[0, i];
            }
            longAdd = Convert.ToInt32(s, 16);
            longAdd = Baseaddr + longAdd;
        }
		return longAdd;            //返回最后地址;
	}


    }
      
    
}
