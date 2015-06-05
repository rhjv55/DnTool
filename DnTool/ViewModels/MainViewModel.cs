using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnTool.Models;
using DnTool.Utilities;
using System.Windows.Threading;
using Utilities.Dm;
using Utilities.Log;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace DnTool.ViewModels
{
    public class MainViewModel:ViewModelBase
    {

       //  private int roleBaseAddress = 0x1221740;//游戏内存基址
       // private int moneyBaseAddress = 0x16D1E50;//背包金钱基址
        private string processName = "DragonNest";//游戏进程名字
        private DispatcherTimer timer = new DispatcherTimer();
        private DmPlugin dm = new DmPlugin();
        private string startupPath = AppDomain.CurrentDomain.BaseDirectory;

        public MainViewModel()
        {
           
            this.ClosedCommand = new RelayCommand(() =>
            {
                timer.Stop();
            });
            this.LoadedCommand = new RelayCommand(() =>
            {
                List<string> list = FileOperateHelper.GetFiles(startupPath+"\\data", "*.txt");
                list.ForEach(x => _fileNames.Add(new FilePath() { Path=x,Name=Path.GetFileNameWithoutExtension(x)}));
            });

            this.SavePointCommand = new RelayCommand(() =>
            {
                if (X != null && Y != null && Z != null)
                {
                    this.InfoList.Add(
                         new InfoViewModel() { Name = "坐标", CurrentPoint = new Point(X, Y, Z) }
                     );
                }
            });
            this.MouseDoubleClickCommand = new RelayCommand(() =>
            {
                Logger.Info("双击了");
                if (SelectedPoint == null)
                {
                    Logger.Info("未选择任何项");
                    return;
                }
                else
                {
                    if (IsAlive)
                    {
                        if (X == null || Y == null || Z == null)
                            return;
                        int a=dm.WriteFloat(CurrentHwnd, "[1221740]+a5c", float.Parse(SelectedPoint.CurrentPoint.X));
                        int b=dm.WriteFloat(CurrentHwnd, "[1221740]+a64", float.Parse(SelectedPoint.CurrentPoint.Y));
                        int c=dm.WriteFloat(CurrentHwnd, "[1221740]+a60", float.Parse(SelectedPoint.CurrentPoint.Z));
                        if (a == 1 && b == 1 && c == 1)
                            Debug.WriteLine("瞬移成功");
                    }
                }
                Logger.Info( SelectedPoint.CurrentPoint.ToString());
            });
          
            Memory64Helper m = new Memory64Helper();

            //List<int> moduleAddrs = new List<int>();
            //string hwndString=dm.EnumWindowByProcess("DragonNest.exe","","DRAGONNEST",2);
            //List<int> hwndList=dm.GetHwnds(hwndString);
            //foreach (var hwnd in hwndList)
            //{
            //    int base_addr=dm.GetModuleBaseAddr(hwnd, "DragonNest.exe");
            //    if (base_addr > 0)
            //        moduleAddrs.Add(base_addr);
            //}
            
            timer.Tick += (s, e) =>
                {
                    if (IsAlive)
                    {
                        X = IntToFloatString(dm.ReadInt(CurrentHwnd, "[1221740]+a5c", 0));
                        Y = IntToFloatString(dm.ReadInt(CurrentHwnd, "[1221740]+a64", 0));
                        Z = IntToFloatString(dm.ReadInt(CurrentHwnd, "[1221740]+a60", 0));
                    }
                   
                };
            timer.Interval = TimeSpan.FromMilliseconds(500);


          

        }
      
       
        
        #region 方法
        /// <summary>
        /// 获取 窗口是否存在
        /// </summary>
        public bool IsAlive { get { return dm.GetWindowState(_currentHwnd, 0) == 1; } }

        private string IntToFloatString(int val)
        {
            return (BitConverter.ToSingle(BitConverter.GetBytes(val), 0)).ToString("F1");
        }

        private int StringToFloatInt(string val)
        {
            return 0;
        }
        //读取制定内存中的值
        public Int64 ReadMemoryValue(int baseAdd)
        {
            return MemoryHelper.ReadMemoryValue(baseAdd, processName);
        }

        //将值写入指定内存中
        public void WriteMemory(int baseAdd, int value)
        {
            MemoryHelper.WriteMemoryValue(baseAdd, processName, value);
        }
        #endregion

        #region 命令
        public RelayCommand ClosedCommand { get; set; }
        public RelayCommand SavePointCommand { get; set; }
        public RelayCommand MouseDoubleClickCommand { get; set; }
        public RelayCommand LoadedCommand { get; set; }
        #endregion

        #region 数据
        private ObservableCollection<InfoViewModel> _infoList = new ObservableCollection<InfoViewModel>();

        public ObservableCollection<InfoViewModel> InfoList
        {
            get { return _infoList; }
            set
            {
                _infoList = value;
                this.OnPropertyChanged("InfoList");
            }
        }

        private object _selectedPath;

        public object SelectedPath
        {
            get { return _selectedPath; }
            set
            {
                if (this._selectedPath!=value)
                {
                    _selectedPath = value;
                    this.OnPropertyChanged("SelectedFile");
                    List<string> lines=FileOperateHelper.ReadFileLines(value.ToString());
                    this.InfoList.Clear();
                    foreach (var line in lines)
                    {
                       string[] temp = line.Split('#');
                       if (temp.Count() != 4)
                       {
                           Debug.WriteLine("路径:"+value.ToString()+",格式不对");
                           continue;
                       }
                        this.InfoList.Add(
                                new InfoViewModel() { Name = temp[0], CurrentPoint = new Point(temp[1], temp[2], temp[3]) }
                            );
                    }
                }
               
            }
        }

        private ObservableCollection<FilePath> _fileNames = new ObservableCollection<FilePath>();

        public ObservableCollection<FilePath> FileNames
        {
            get { return _fileNames; }
            set
            {
                _fileNames = value;
               // this.OnPropertyChanged("FileNames");
            }
        }
        

         private int _currentHwnd;

         public int CurrentHwnd
         {
             get { return _currentHwnd; }
             set
             {
                if (_currentHwnd != value)
                {
                    _currentHwnd = value;
                    timer.Stop();
                    timer.Start();  
                }
             }
         }
         

         private InfoViewModel _selectedPoint;

         public InfoViewModel SelectedPoint
         {
             get { return _selectedPoint; }
             set
             {
                 _selectedPoint = value;
                 this.OnPropertyChanged("SelectedPoint");
             }
         }
         

        private string _y;

        public string Y
        {
            get { return _y; }
            set
            {
                _y = value;
                this.OnPropertyChanged("Y");
            }
        }

        private string _z;

        public string Z
        {
            get { return _z; }
            set
            {
                _z = value;
                this.OnPropertyChanged("Z");
            }
        }
        
        private string _x;

        public string X
        {
            get { return _x; }
            set { _x = value;
            this.OnPropertyChanged("X");
            }
        }
        #endregion
    }
}
