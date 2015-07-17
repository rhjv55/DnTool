using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnTool.Utilities;
using Utilities.Dm;
using System.Diagnostics;
using DnTool.Models;
using Utilities.Tasks;
using GalaSoft.MvvmLight.Command;
using DnTool.GameTask;
using Utilities.Log;
using MahApps.Metro.Controls.Dialogs;
namespace DnTool.ViewModels
{
    public class ToolsViewModel:NotifyPropertyChanged
    {
        ViewModelLocator Locator=new ViewModelLocator();

        public RelayCommand AutoOpenEggCommand { get; set; }
        public RelayCommand BeginBagClearCommand { get; set; }
        public RelayCommand StopBagClearCommand { get; set; }
        public RelayCommand ShuaHuoshanCommand { get; set; }
        public RelayCommand DetectCommand { get; set; }
        public RelayCommand OpenCommand { get; set; }
        public RelayCommand StopCommand { get; set; }
        public RelayCommand<MallThing> BuyCommand { get; set; }
        private void InitThings()
        {
            this.Things.Add(new MallThing() { ID = 1, Name = "物品保护魔法药", Description = "强化+9至+12保护",CanUseLB=true,Value=100000});
            this.Things.Add(new MallThing() { ID = 2, Name = "钻石潘多拉之心", Description = "随机获得各种物品", CanUseLB = true, Value = 40000 });
            this.Things.Add(new MallThing() { ID = 3, Name = "钻石潘多拉火种", Description = "打开钻石潘多拉之心", CanUseLB = true, Value = 1 });
            this.Things.Add(new MallThing() { ID = 4, Name = "阿尔杰塔的礼物", Description = "随机获得各种物品", CanUseLB = false, Value = 328000 });
            this.Things.Add(new MallThing() { ID = 5, Name = "10000龙币商品券", Description = "10000龙币", CanUseLB = true, Value = 100000 });
            this.Things.Add(new MallThing() { ID = 6, Name = "柏林的感谢口袋", Description = "最高级阿尔泰丶最高级钻石丶生命的精髓各100个", CanUseLB = true, Value = 80000 });
            this.Things.Add(new MallThing() { ID = 7, Name = "龙裔特别口袋", Description = "70龙玉,70纹章，女神的叹息", CanUseLB = true, Value = 30000 });
            this.Things.Add(new MallThing() { ID = 8, Name = "装有富饶护符的箱子", Description = "额外金币加成", CanUseLB = false, Value = 60000 });
        }
        
        public ToolsViewModel()
        {
            InitThings();
            this.BuyCommand = new RelayCommand<MallThing>(
                (t)=>this.Buy(t),
                (t)=>t!=null&&SoftContext.Role!=null);

            this.StopCommand = new RelayCommand(() =>
            {
                SoftContext.TaskEngine.Stop();
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
                            SoftContext.MainWindow.ShowMessageAsync("多开失败", "没有找到游戏进程"); 
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
                                    SoftContext.MainWindow.ShowMessageAsync("多开成功", "进程[" + handle.ProcessID + "]的互斥体句柄关闭成功"); 
                                }
                            }
                        }
                    }
                    else
                    {
                        SoftContext.MainWindow.ShowMessageAsync("多开失败", "没有找到游戏进程"); 
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            });

            this.BeginBagClearCommand = new RelayCommand(() =>
            {
                TaskContext context = new TaskContext(SoftContext.Role);

                /// 任务设置，可用属性为：.Thing .Num .UseLB
                context.Settings.BeginPage = BeginPage;
                context.Settings.BeginItem = BeginItem;
                context.Settings.StopPage = StopPage;
                context.Settings.StopItem=StopItem;

                TaskBase task = new BagClearTask(context);
                task.Name = "清理背包";
                SoftContext.TaskEngine.Start(task);
            },()=>SoftContext.Role!=null);
            this.AutoOpenEggCommand = new RelayCommand(() =>
            {
                TaskContext context = new TaskContext(SoftContext.Role);

                TaskBase task = new ZidongkaidanTask(context);
                task.Name = "自动开蛋";
                SoftContext.TaskEngine.Start(task);
            },()=>SoftContext.Role!=null);
            this.StopBagClearCommand = new RelayCommand(() =>
            {
                SoftContext.TaskEngine.Stop();
            });
        }
        private async void Buy(MallThing thing)
        {
            TaskContext context = new TaskContext(SoftContext.Role);
        
            /// 任务设置，可用属性为：.Thing .Num .UseLB
            context.Settings.Thing = thing;
            context.Settings.Num = this._number;
            context.Settings.UseLB = this._useLB;
            
            TaskBase task = new BuyThingsTask(context);
            task.Name = "购买商城物品";
            if(this._number<=0)
            {
                await SoftContext.MainWindow.ShowMessageAsync("购买失败", "请检查物品数量！");
                return;
            }
            SoftContext.TaskEngine.Start(task);
        }

   
        private List<MallThing> _things=new List<MallThing>();

        public List<MallThing> Things
        {
            get { return _things; }
            set
            {
                _things = value;
                this.OnPropertyChanged("Things");
            }
        }

        private int _number;

        public int Number
        {
            get { return _number; }
            set { _number = value; }
        }

        private bool _useLB;

        public bool UseLB
        {
            get { return _useLB; }
            set
            {
                base.SetProperty(ref _useLB, value, () => this.UseLB);
            }
        }

        private int _beginPage;

        public int BeginPage
        {
            get { return _beginPage; }
            set
            {
                base.SetProperty(ref _beginPage, value, () => this.BeginPage);
            }
        }
        private int _beginItem;

        public int BeginItem
        {
            get { return _beginItem; }
            set
            {
                base.SetProperty(ref _beginItem, value, () => this.BeginItem);
            }
        }

        private int _stopPage;

        public int StopPage
        {
            get { return _stopPage; }
            set
            {
                base.SetProperty(ref _stopPage, value, () => this.StopPage);
            }
        }
        private int _stopItem;

        public int StopItem
        {
            get { return _stopItem; }
            set
            {
                base.SetProperty(ref _stopItem, value, () => this.StopItem);
            }
        }
        
        
    }
}
