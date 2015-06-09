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
using System.Runtime.InteropServices;
using System.Threading;
using DnTool.Utilities.Keypad;

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
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes("sssse");
            foreach (var item in bytes)
            {
                Debug.Write(item.ToString("x8"));
            }
          

            this.TestCommand = new RelayCommand(() =>
            {
             
                int ret = dm.BindWindowEx(CurrentHwnd, "normal", "normal", "dx.keypad.input.lock.api|dx.keypad.state.api|dx.keypad.api", "", 0);
                Debug.WriteLine(ret);
                dm.Delay(1000);
              //  dm.SetWindowState(CurrentHwnd,1);
                dm.KeyPress(DmKeys.Escape);
                dm.KeyPress(DmKeys.Escape);
                dm.KeyPress(DmKeys.Escape);
            });

            this.UnBindCommand = new RelayCommand(() =>
            {

                dm.SetWindowState(CurrentHwnd,1);
                dm.Delay(2000);
                dm.MoveTo(200,200);
                dm.LeftClick();

       int i = 0;  
       while (i < 5)  
       {
           WinIo.MykeyDown(VKKey.VK_ESCAPE);
           Thread.Sleep(100);
           WinIo.MykeyUp(VKKey.VK_ESCAPE);
           Thread.Sleep(100);
           WinIo.MykeyDown(VKKey.VK_SPACE);  
           Thread.Sleep(100);  
           WinIo.MykeyUp(VKKey.VK_SPACE);  
           Thread.Sleep(100);  
           WinIo.MykeyDown(VKKey.VK_S);  
           Thread.Sleep(100);  
           WinIo.MykeyUp(VKKey.VK_S);  
           Thread.Sleep(500);  
           WinIo.MykeyDown(VKKey.VK_D);  
           Thread.Sleep(100);  
           WinIo.MykeyUp(VKKey.VK_D);  
           Thread.Sleep(500);  
           WinIo.MykeyDown(VKKey.VK_A);  
           Thread.Sleep(100);  
           WinIo.MykeyUp(VKKey.VK_A);  
           i++;  
       }  

                dm.UnBindWindow();
            });
            
            this.OpenCommand = new RelayCommand(() =>
            {
                try
                {
                    Process[] all = Process.GetProcessesByName("DragonNest");

                    if (all != null)
                    {
                        if (all.Length == 0)
                        {
                            return;
                        }

                        foreach (Process process in all)
                        {
                            var handles = Win32Processes.GetHandles(process, "Mutant", "\\BaseNamedObjects\\MutexDragonNest");

                            if (handles.Count == 0)
                            {
                                continue;
                            }

                            foreach (var handle in handles)
                            {
                                IntPtr ipHandle = IntPtr.Zero;
                                if (!MutexCloseHelper.DuplicateHandle(Process.GetProcessById(handle.ProcessID).Handle,
                                    handle.Handle, MutexCloseHelper.GetCurrentProcess(), out ipHandle, 0, false, MutexCloseHelper.DUPLICATE_CLOSE_SOURCE))
                                {
                                    // richTextBox1.AppendText("DuplicateHandle() failed, error =" + Marshal.GetLastWin32Error() + Environment.NewLine);


                                }
                                else
                                {
                                    MutexCloseHelper.CloseHandle(ipHandle);
                                    Debug.WriteLine("进程[" + handle.ProcessID + "]的互斥体句柄关闭成功");
                                }
                            }

                        }
                    }
                    else
                    {
                        // richTextBox1.AppendText("没有找到运行的程序" + Environment.NewLine);
                    }
                }
                catch (Exception ex)
                {
                   Debug.WriteLine(ex.Message);

                }
            });
           
            this.ClosedCommand = new RelayCommand(() =>
            {
              //  WinIo.Shutdown();
                timer.Stop();
            });
            this.LoadedCommand = new RelayCommand(() =>
            {
                List<string> list = FileOperateHelper.GetFiles(startupPath+"\\data", "*.txt");
                list.ForEach(x => _fileNames.Add(new FilePath() { Path=x,Name=Path.GetFileNameWithoutExtension(x)}));

              //
                WinIo.Initialize();
               
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
            this.MoveCommand = new RelayCommand(() =>
            {
                if (SelectedPoint == null)
                    return;
                this.Move(CurrentHwnd,SelectedPoint.CurrentPoint);
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
                    if (IsAlive(CurrentHwnd))
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
        /// 瞬移
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <returns></returns>
        public bool Move(int hwnd,Point point)
        {
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
                    int a = dm.WriteFloat(hwnd, "[1221740]+a5c", float.Parse(point.X));
                    int b = dm.WriteFloat(hwnd, "[1221740]+a64", float.Parse(point.Y));
                    int c = dm.WriteFloat(hwnd, "[1221740]+a60", float.Parse(point.Z));
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

        /// <summary>
        /// 判断窗口是否存在
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public bool IsAlive(int hwnd) 
        {
            return dm.GetWindowState(hwnd, 0) == 1; 
        }

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
        public RelayCommand OpenCommand { get; set; }
        public RelayCommand UnBindCommand { get; set; }
        
        public RelayCommand TestCommand { get; set; }
        public RelayCommand ClosedCommand { get; set; }
        public RelayCommand SavePointCommand { get; set; }
        public RelayCommand MoveCommand { get; set; }
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
                                new InfoViewModel() { Name = temp[0], CurrentPoint = new Point(temp[1], temp[2], temp[3]),ID=this.InfoList.Count() }
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
