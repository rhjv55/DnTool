using DnTool.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnTool.Models
{
    /// <summary>
    /// 坐标类
    /// </summary>
    public class Point:ViewModelBase
    {
        public string X { get; set; }
        public string Y { get; set; }
        public string Z { get; set; }

        public Point()
        { 
        }
        public Point(string x, string y, string z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public override string ToString()
        {
            return string.Format("X坐标:{0},Y坐标:{1},Z坐标:{2}",X,Y,Z);
        }
    }
}
