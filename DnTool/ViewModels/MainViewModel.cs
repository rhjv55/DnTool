using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnTool.Models;
using DnTool.Utilities;
namespace DnTool.ViewModels
{
    public class MainViewModel
    {
      
        private int roleBaseAddress = 0x00000000;//游戏内存基址
        private int moneyBaseAddress = 0x0000000;//背包金钱基址
        private string processName = "DragonNest";//游戏进程名字
        
        public MainViewModel()
        {
            this.CurrentPoint = new Point("0","0","0");
            float x, y, z;
            int address = MemoryHelper.ReadMemoryValue(roleBaseAddress,processName);
            address = address + 0xa6c;
            address = MemoryHelper.ReadMemoryValue(address,processName);

            this.CurrentPoint.X = address.ToString("F2");
            this.CurrentPoint.Y = address.ToString("F2");
            this.CurrentPoint.Z = address.ToString("F2");

            MemoryHelper.WriteMemoryValue(address,processName,0x1868F);
           
        }
        //读取制定内存中的值
        public int ReadMemoryValue(int baseAdd)
        {
            return MemoryHelper.ReadMemoryValue(baseAdd, processName);
        }

        //将值写入指定内存中
        public void WriteMemory(int baseAdd, int value)
        {
            MemoryHelper.WriteMemoryValue(baseAdd, processName, value);
        }

        public Point CurrentPoint { get; set; }
    }
}
