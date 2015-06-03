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

namespace DnTool.ViewModels
{
    public class MainViewModel:ViewModelBase
    {

       //  private int roleBaseAddress = 0x1221740;//游戏内存基址
       // private int moneyBaseAddress = 0x16D1E50;//背包金钱基址
        private string processName = "DragonNest";//游戏进程名字
        private DispatcherTimer timer = new DispatcherTimer();
        private DmPlugin dm = new DmPlugin();
        public MainViewModel()
        {
           
            this.ClosedCommand = new RelayCommand(() =>
            {
                timer.Stop();
            });
            this.SavePointCommand = new RelayCommand(() =>
            {
               this.InfoList.Add(
                  new InfoViewModel() { Name = "坐标", PID = 111, CurrentPoint = new Point(X, Y, Z) }
               );
            });
            this.MouseDoubleClickCommand = new RelayCommand(() =>
            {
                Logger.Info("双击了");
                if (SelectedPoint == null)
                {
                    Logger.Info("未选择任何项");
                    return;
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
                  
                    //long address = m.ReadMemoryValue(roleBaseAddress, processName);
                    //Logger.Debug(Convert.ToString(address, 16));
                    
                    //address = address + 0xa5c;
                    //Logger.Debug(Convert.ToString(address, 16));

                    //address = m.ReadMemoryValue(address, processName);
                    //Logger.Debug(Convert.ToString(address, 10));
                    //dm.Delay(2000);
                    // = address.ToString("F2");
                    //this.CurrentPoint.Y = address.ToString("F2");
                    //this.CurrentPoint.Z = address.ToString("F2");
                    int address = dm.ReadInt(201936, "[1221740]+a5c", 0);
                    X = (BitConverter.ToSingle(BitConverter.GetBytes(address), 0)).ToString("F2");
                    float v = dm.ReadFloat(201936, "[1221740]+a5c");

                    Logger.Debug(Convert.ToString(address, 16));
                    Logger.Debug(v.ToString());
                    this.X = new Random().Next(100).ToString();
                    this.Y = new Random().Next(10000).ToString();
                    this.Z = new Random().Next(1000).ToString();
                    dm.Delay(200);
                };
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Start();
           
           

           // MemoryHelper.WriteMemoryValue(address,processName,0x1868F);
           
        }
      
       
        private ObservableCollection<InfoViewModel> _infoList=new ObservableCollection<InfoViewModel>();

        public ObservableCollection<InfoViewModel> InfoList
        {
            get { return _infoList; }
            set
            {
                _infoList = value;
                this.OnPropertyChanged("InfoList");
            }
        }
        #region 方法
        /// <summary>
        /// 实现瞬移
        /// </summary>
        /// <param name="point">XYZ坐标</param>
        /// <returns></returns>
        private bool Shunyi(Point point)
        {
            int address = dm.ReadInt(201936, "[1221740]+a5c", 0);
            X = (BitConverter.ToSingle(BitConverter.GetBytes(address), 0)).ToString("F2");
            float v = dm.ReadFloat(201936, "[1221740]+a5c");

            Logger.Debug(Convert.ToString(address, 16));
            Logger.Debug(v.ToString());
            return false;
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
        
        #endregion

        #region 数据
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
