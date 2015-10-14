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
using GalaSoft.MvvmLight.Command;
using Utilities.Tasks;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Forms;
using DnTool.Utilities.MyPlugin;

namespace DnTool.ViewModels
{
    public class TeleportViewModel:NotifyPropertyChanged
    {

        #region 命令
        public RelayCommand SetXiaohaoCommand { get; set; }
        public RelayCommand CreateCommand { get; set; }
        public RelayCommand AddCurrentPointCommand { get; set; }
        public RelayCommand AddNewPointCommand { get; set; }
        public RelayCommand<Point> TeleportCommand { get; set; }
        public RelayCommand<Point> DeleteCommand { get; set; }
        public RelayCommand ModifyCommand { get; set; }
        public RelayCommand ImportCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand SaveAsCommand { get; set; }
        public RelayCommand ClearCommand { get; set; }
        public RelayCommand SelectionChangedCommand { get; set; }
        #endregion

        public Point CurrentPoint { get; set; }
        public Point NewPoint { get; set; }
        public ObservableCollection<Point> Points { get; set; }
        
        public static DispatcherTimer timer = new DispatcherTimer();
        public ObservableCollection<File> Files { get; set; }
        private object _selectedValue;

        public object SelectedValue
        {
            get { return _selectedValue; }
            set 
            {
                _selectedValue = value;
                if (_selectedValue == null)
                    return;
                List<string> lines = FileOperateHelper.ReadFileLines(_selectedValue.ToString());
                this.Points.Clear();
                foreach (var line in lines)
                {
                    string[] temp = line.Split('#');
                    if (temp.Count() != 4)
                    {
                        Debug.WriteLine("路径:" + _selectedValue + ",格式不对");
                        Debug.WriteLine(line);
                        continue;
                    }
                    this.Points.Add(new Point(temp[0], float.Parse(temp[1]), float.Parse(temp[2]), float.Parse(temp[3])));
                }
            }
        }
        

       
        public TeleportViewModel()
        {
         

            this.CurrentPoint = new Point("当前坐标",0,0,0);
            this.NewPoint = new Point("添加坐标",0,0,0);
            this.Points = new ObservableCollection<Point>();
            this.Files = new ObservableCollection<File>();
            UpdateFiles();

            this.AddCurrentPointCommand = new RelayCommand(() => this.AddPoint(CurrentPoint));
            this.AddNewPointCommand = new RelayCommand(() => this.AddPoint(NewPoint));
            this.ClearCommand = new RelayCommand(() => this.Clear());
            this.DeleteCommand = new RelayCommand<Point>((p)=>this.DeletePoint(p));
            this.TeleportCommand = new RelayCommand<Point>((p) =>this.Teleport(p));
            this.SaveCommand = new RelayCommand(()=>this.SaveList());
            this.SaveAsCommand = new RelayCommand(()=>this.SaveAs());
            this.CreateCommand = new RelayCommand(()=>this.Create());
            this.ImportCommand = new RelayCommand(()=>this.Import());
            this.SetXiaohaoCommand = new RelayCommand(() =>
            {
               // new ViewModelLocator().SetXiaohao.IsOpen = true;
                HPlugin p = new HPlugin();
                DmPlugin dm = new DmPlugin();
                p.BindWindow(0,"","winio","winio",0);

                p.KeyPress(DnTool.Utilities.MyPlugin.Keys.K);
              // dm.MoveTo(81,40);
               // dm.LeftClick();
                //p.KeyPress(DnTool.Utilities.MyPlugin.Keys.K);
                //p.KeyPress(DnTool.Utilities.MyPlugin.Keys.K);
                //p.KeyPress(DnTool.Utilities.MyPlugin.Keys.A);
                //p.KeyPress(DnTool.Utilities.MyPlugin.Keys.B);
                //p.KeyPress(DnTool.Utilities.MyPlugin.Keys.C);
                //p.KeyDown(DnTool.Utilities.MyPlugin.Keys.D);
                //p.KeyUp(DnTool.Utilities.MyPlugin.Keys.D);
                //p.KeyDown(DnTool.Utilities.MyPlugin.Keys.M);
                //p.KeyDown(DnTool.Utilities.MyPlugin.Keys.N);
                //p.KeyUp(DnTool.Utilities.MyPlugin.Keys.N);
                //p.KeyUp(DnTool.Utilities.MyPlugin.Keys.M);
               // int x, y;
               // dm.GetCursorPos(out x,out y);
               // Debug.WriteLine("当前鼠标位置：" + x + " , " + y);
               // dm.Delay(100);
               //p.MoveTo(100,100);
               //dm.Delay(100);
               //int a, b,c,d;
               //dm.GetCursorPos(out a, out b);
               //p.GetCursorPos(out c, out d);
               //Debug.WriteLine("移动后鼠标位置：" + a + " , " + b);
               //Debug.WriteLine("2222222移动后鼠标位置：" + c + " , " + d );
               //Debug.WriteLine("两者相差："+(x-a)+"  "+(y-b));
              //  p.RightDown();
               // p.LeftClick();
                //dm.Delay(10000);
               // p.RightUp();
               // p.RightClick();
                //byte[] data = p.ReadData(4668, "0014E204", 10);
                //p.WriteInt(4668, "0014E204", 0, 1000);
                //MessageBox.Show("读取数据："+BitConverter.ToInt32(data,0).ToString());
               
            });
            timer.Tick += (s, e) =>
            {
                if (SoftContext.Role == null)
                    return;
                DmPlugin dm = SoftContext.Role.Window.Dm;
                int hwnd = SoftContext.Role.Window.Hwnd;
                if (SoftContext.Role.Window.IsAlive)
                {
                    CurrentPoint.X = IntToFloat(dm.ReadInt(hwnd, "[1221740]+a5c", 0));
                    CurrentPoint.Y = IntToFloat(dm.ReadInt(hwnd, "[1221740]+a64", 0));
                    CurrentPoint.Z = IntToFloat(dm.ReadInt(hwnd, "[1221740]+a60", 0));
                    
                }
                else
                {
                    CurrentPoint.X = 0;
                    CurrentPoint.Y = 0;
                    CurrentPoint.Z = 0;
                }

            };
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Start();
        }

        private void Import()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //dlg.FileName = "User.txt"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
            if (!System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "data"))
                System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "data");
            dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "data";

            // Show save file dialog box
            var result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                try
                {
                    this.SelectedIndex = -1;
                    List<string> lines = FileOperateHelper.ReadFileLines(dlg.FileName);
                    this.Points.Clear();
                    foreach (var line in lines)
                    {
                        string[] temp = line.Split('#');
                        if (temp.Count() != 4)
                        {
                            Debug.WriteLine("路径:" + _selectedValue + ",格式不对");
                            Debug.WriteLine(line);
                            continue;
                        }
                        this.Points.Add(new Point(temp[0], float.Parse(temp[1]), float.Parse(temp[2]), float.Parse(temp[3])));
                    }

                }
                catch (Exception ex)
                {
                    SoftContext.MainWindow.ShowMessageAsync("导入失败", ex.Message);
                }
            }
        }
        private void UpdateFiles()
        {
            Files.Clear();
            List<string> list = FileOperateHelper.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\data", "*.txt");
            list.ForEach(x => Files.Add(new File() { Path = x, Name = System.IO.Path.GetFileNameWithoutExtension(x) }));
        }
        private void SaveAs()
        {
            //if (FileOperateHelper.IsExists(_selectedValue.ToString()))
            //{
            //    var result = System.Windows.MessageBox.Show("文件已存在是否覆盖？", "提示", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
            //    if (result == System.Windows.MessageBoxResult.No)
            //        return;
            //}
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            //dlg.FileName = "User.txt"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
            if (!System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "data"))
                System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "data");
            dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "data";

            // Show save file dialog box
            var result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                try
                {
                    string content = "";
                    foreach (var p in Points)
                    {
                        content += p.Name + "#" + p.X + "#" + p.Y + "#" + p.Z + "\r\n";
                    }
                    FileOperateHelper.WriteFile(dlg.FileName, content, true);
                    UpdateFiles();
                }
                catch (Exception ex)
                {
                    SoftContext.MainWindow.ShowMessageAsync("保存失败", ex.Message);
                }
            }

        }

        private void Create()
        {
            this.SelectedIndex =-1;
            this.Points.Clear();
        }

        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                base.SetProperty(ref _selectedIndex, value, () => this.SelectedIndex);
            }
        }
        
        

        private void SaveList()
        {
            try
            {
                if (_selectedValue == null)
                {
                    SaveAs();
                    return;
                }
               
                string content = "";
                foreach (var p in Points)
                {
                    content += p.Name + "#" + p.X + "#" + p.Y + "#" + p.Z + "\r\n";
                }
                FileOperateHelper.WriteFile(_selectedValue.ToString(), content,true);
                SoftContext.MainWindow.ShowMessageAsync("保存成功", "路径："+_selectedValue.ToString());
            }catch(Exception ex)
            {
                SoftContext.MainWindow.ShowMessageAsync("保存失败",ex.Message);
            }
        }
        private float IntToFloat(int val)
        {
            return float.Parse(BitConverter.ToSingle(BitConverter.GetBytes(val), 0).ToString("F1"));
        }
        public bool TeleportCurrentPoint()
        {
            return this.Teleport(this.CurrentPoint);
        }
   
        public bool Teleport(Point point)
        {
            IRole role = SoftContext.Role;
            if (role == null)
                return false;
            DmPlugin dm = role.Window.Dm;
            int hwnd = role.Window.Hwnd;
            if (point == null)
            {
                Debug.WriteLine("坐标不能为null");
                return false;
            }
            else
            {
                if (role.Window.IsAlive)
                {
                    this.TeleportByHwnd(dm,hwnd,point);      //大号瞬移

                    ObservableCollection<RoleInfo> roleList = new ViewModelLocator().SetXiaohao.GameRoleList;
                    foreach (var roleInfo in roleList)
	                {
                        if (dm.GetWindowState(roleInfo.Hwnd, 0) != 1)  //判断窗口是否存在
                            continue;
                        if (roleInfo.IsTogether)
                        {
                            this.TeleportByHwnd(dm, roleInfo.Hwnd, point);
                        }
                        if (roleInfo.IsMove)                         //如果移动则写内存
                        {
                            dm.WriteInt(roleInfo.Hwnd, "[1221740]+2320", 0, 131072);
                            dm.Delay(200);
                            dm.WriteInt(roleInfo.Hwnd, "[1221740]+2320", 0, 0);
                            dm.Delay(100);
                        }

	                }
                 
                           
                           
                    return true;
                }
                else
                {
                    SoftContext.MainWindow.ShowMessageAsync("瞬移失败","窗口不存在:" + hwnd);
                    return false;
                }
            }
        }
        /// <summary>
        /// 指定窗口句柄瞬移
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="hwnd"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private bool TeleportByHwnd(DmPlugin dm,int hwnd,Point point)
        {
            int a = dm.WriteFloat(hwnd, "[1221740]+a5c", point.X);
            int b = dm.WriteFloat(hwnd, "[1221740]+a64", point.Y);
            int c = dm.WriteFloat(hwnd, "[1221740]+a60", point.Z);
            if (a == 1 && b == 1 && c == 1)
                return true;
            else
            {
                Debug.WriteLine("瞬移失败，写入X:{0}，Y:{1}，Z:{2},句柄：{3}", a, b, c, hwnd);
                return false;
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
