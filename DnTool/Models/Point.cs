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
    public class Point:NotifyPropertyChanged,ICloneable
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                base.SetProperty(ref _name, value, () => this.Name);
            }
        }
        
        private float _x;

        public float X
        {
            get { return _x; }
            set
            {
                base.SetProperty(ref _x,value,()=>this.X);
            }
        }

        private float _y;

        public float Y
        {
            get { return _y; }
            set
            {
                base.SetProperty(ref _y, value, () => this.Y);
            }
        }

        private float _z;

        public float Z
        {
            get { return _z; }
            set
            {
                base.SetProperty(ref _z, value, () => this.Z);
            }
        }
        

        public Point()
        { 
        }
        public Point(string name,float x, float y, float z)
        {
            this.Name = name;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public override string ToString()
        {
            return string.Format("名称：{0},X坐标:{1},Y坐标:{2},Z坐标:{3}",Name,X,Y,Z);
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
