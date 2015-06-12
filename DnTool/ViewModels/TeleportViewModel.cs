using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnTool.Utilities;
using DnTool.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Utilities.Dm;
using System.Windows.Threading;
namespace DnTool.ViewModels
{
    public class TeleportViewModel:NotifyPropertyChanged
    {

        #region 命令
        public RelayCommand AddCurrentPointCommand { get; set; }
        public RelayCommand AddNewPointCommand { get; set; }
        public RelayCommand<Point> TeleportCommand { get; set; }
        public RelayCommand<Point> DeleteCommand { get; set; }
        public RelayCommand ModifyCommand { get; set; }
        public RelayCommand ImportListCommand { get; set; }
        public RelayCommand SaveListCommand { get; set; }
        public RelayCommand Redo { get; set; }
        public RelayCommand Undo { get; set; }
        public RelayCommand ClearCommand { get; set; }
        #endregion

        public Point CurrentPoint { get; set; }
        public Point NewPoint { get; set; }
        public ObservableCollection<Point> Points { get; set; }
        private DmPlugin dm=new DmPlugin();
        private DispatcherTimer timer = new DispatcherTimer();

        public TeleportViewModel()
        {
            this.CurrentPoint = new Point("当前坐标",0,0,0);
            this.NewPoint = new Point("添加坐标",0,0,0);
            this.Points = new ObservableCollection<Point>();

            this.AddCurrentPointCommand = new RelayCommand(() => this.AddPoint(CurrentPoint));
            this.AddNewPointCommand = new RelayCommand(() => this.AddPoint(NewPoint));
            this.ClearCommand = new RelayCommand(() => this.Clear());
            this.DeleteCommand = new RelayCommand<Point>((p)=>this.DeletePoint(p));
            this.TeleportCommand = new RelayCommand<Point>((p) => Debug.WriteLine(p.ToString()));
            timer.Tick += (s, e) =>
            {
                if (IsAlive(11))
                {
                    CurrentPoint.X = IntToFloat(dm.ReadInt(11, "[1221740]+a5c", 0));
                    CurrentPoint.Y = IntToFloat(dm.ReadInt(11, "[1221740]+a64", 0));
                    CurrentPoint.Z = IntToFloat(dm.ReadInt(11, "[1221740]+a60", 0));
                    string ret = dm.FindPicE(0, 0, 2000, 2000, "跳过了.bmp");
                    if (ret != "")
                    {
                        Debug.WriteLine("正在动画，快跳过~~~");
                        Debug.Write(ret);
                    }

                }

            };
            timer.Interval = TimeSpan.FromMilliseconds(500);
        }
        private float IntToFloat(int val)
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(val), 0);
        }
        private bool IsAlive(int hwnd) 
        {
            return dm.GetWindowState(hwnd, 0) == 1; 
        }
        private bool Teleport(Point point)
        {
            int hwnd=0;
            if (point == null)
            {
                Debug.WriteLine("坐标不能为null");
                return false;
            }
            else
            {
                if (IsAlive(hwnd))
                {

                    if (point.X == null || point.Y == null || point.Z == null)
                    {
                        Debug.WriteLine("坐标XYZ不能为null");
                        return false;
                    }
                    Debug.WriteLine(point.ToString());
                    int a = dm.WriteFloat(hwnd, "[1221740]+a5c", point.X);
                    int b = dm.WriteFloat(hwnd, "[1221740]+a64", point.Y);
                    int c = dm.WriteFloat(hwnd, "[1221740]+a60", point.Z);
                    if (a == 1 && b == 1 && c == 1)
                    {
                        Debug.WriteLine("瞬移成功");
                        return true;
                    }
                    else
                    {
                        Debug.WriteLine("瞬移失败，写入X:{0}，Y:{1}，Z:{2}",a,b,c);
                        return false;
                    }
                }
                else
                {
                    Debug.WriteLine("窗口不存在:" + hwnd);
                    return false;
                }
            }
        }

        private void DeletePoint(Point p)
        {
            this.Points.Remove(p);
        }

        private void Clear()
        {
            this.Points.Clear();
        }

        private void AddPoint(Point point)
        {
            if (point == null)
                return;
            this.Points.Add(new Point(point.Name,point.X,point.Y,point.Z));
        }

      



      
        
        
    }
}
