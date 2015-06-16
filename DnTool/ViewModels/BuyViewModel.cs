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
namespace DnTool.ViewModels
{
    public class BuyViewModel:NotifyPropertyChanged
    {
       
        private List<Thing> _things;

        public List<Thing> Things
        {
            get { return _things; }
            set 
            {
                _things = value;
                this.OnPropertyChanged("Things");
            }
        }

        private string _number;

        public string Number
        {
            get { return _number; }
            set { _number = value; }
        }

        private void InitThings()
        {
            this.Things.Add(new Thing() { ID = 1, Name = "龙裔特别口袋", Description = "70龙玉,70纹章" });
            this.Things.Add(new Thing() { ID = 2, Name = "物品保护魔法药", Description = "强化+8至+12保护" });
        }
        
        public BuyViewModel()
        {
            this._things = new List<Thing>();
            InitThings();
            this.BuyCommand = new RelayCommand<Thing>((t)=>this.Buy(t));
            dm.SetPath(AppDomain.CurrentDomain.BaseDirectory);

            //TaskEngine engine = new TaskEngine();

            //var task1 = TaskBuilder.Create<KejuzhuangyuanTask>()
            //                             .WithIdentity("科举状元")
            //                              .UsingJobData(KejuzhuangyuanTask.DelayTime, 1)
            //                              .UsingJobData(KejuzhuangyuanTask.IsAuto, true)
            //                              .UsingJobData(KejuzhuangyuanTask.IsUseTransfer, false)
            //                              .UsingJobData(KejuzhuangyuanTask.IsUseRandomAnswer, false)
            //                              .Build(window);
            //var task2 = TaskBuilder.Create<MeirifubenTask>()
            //                             .WithIdentity("每日副本")
            //                             .RequestRecovery()
            //                             .UsingJobData(MeirifubenTask.Test, 11111)
            //                             .Build(role);

            //engine.MultiTask.Add(task1);
            //// engine.MultiTask.Add(task2);
            //engine.Start();
        }
        DmPlugin dm = new DmPlugin();
        private void Buy(Thing thing)
        {
            if (thing == null)
                return;
            int number;
            if (!int.TryParse(Number, out number))
                return;
            if (MainViewModel.Hwnd == 0)
                return;
            int ret=dm.BindWindow(MainViewModel.Hwnd,DmBindDisplay.dx,DmBindMouse.windows,DmBindKeypad.windows,0);
            if (ret == 1)
                Debug.WriteLine("绑定成功");
            else
                return;
            for (int i = 0; i < number; i++)
            {
                dm.FindPicE_LeftClick(0, 0, 2000, 2000, "搜索.bmp", 45, 7);
                dm.SendString(MainViewModel.Hwnd, thing.Name.ToString());
                dm.FindPicE_LeftClick(0, 0, 2000, 2000, "搜索btn.bmp", 5, 5);
                dm.FindPicE_LeftClick(0, 0, 2000, 2000, "搜索btn.bmp", 5, 5);
                this.WaitTrue(() => { return dm.FindPicE_LeftClick(0, 0, 2000, 2000, thing.Name+".bmp", 35, 85); }, () => dm.Delay(1000), 60);
                this.WaitTrue(() => { return dm.FindPicE_LeftClick(0, 0, 2000, 2000, "取消.bmp", -140, 0); }, () => dm.Delay(1000), 60);
                this.WaitTrue(() => { return dm.FindPicE_LeftClick(0, 0, 2000, 2000, "是.bmp", 0, 0); }, () => dm.Delay(1000), 60);
                this.WaitTrue(() => { return dm.FindPicE_LeftClick(0, 0, 2000, 2000, "确认.bmp", 0, 0); }, () => dm.Delay(1000), 60);
                dm.Delay(1000);
            }
            
            dm.UnBindWindow();
            

        }
        /// <summary>
        /// 重复执行指定代码直到成功
        /// </summary>
        /// <param name="trueFunc">要执行的返回成功的操作</param>
        /// <param name="failAction">每次执行失败时，执行的动作</param>
        /// <param name="maxCount">重试的最大次数，0为一直重试</param>
        /// <returns>是否成功</returns>
        public bool WaitTrue(Func<bool> trueFunc, Action failAction = null, int maxCount = 0)
        {
            failAction = failAction ?? (() => { });

            if (maxCount == 0)
            {
                while (!trueFunc())
                {
                    failAction();
                }
                return true;
            }
            int count = 0;
            while (!trueFunc() && count < maxCount)
            {

                failAction();
                count++;
            }
            return count < maxCount;
        }
        public RelayCommand<Thing> BuyCommand { get; set; }
    }
}
