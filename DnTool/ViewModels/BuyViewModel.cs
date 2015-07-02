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
using GalaSoft.MvvmLight.CommandWpf;
using DnTool.GameTask;
using Utilities.Log;
namespace DnTool.ViewModels
{
    public class BuyViewModel:NotifyPropertyChanged
    {
        public RelayCommand<MallThing> BuyCommand { get; set; }
        private void InitThings()
        {
            this.Things.Add(new MallThing() { ID = 1, Name = "龙裔特别口袋", Description = "70龙玉,70纹章" ,CanUseLB=false,Value=30000});
            this.Things.Add(new MallThing() { ID = 2, Name = "物品保护魔法药", Description = "强化+8至+12保护",CanUseLB=true,Value=100000});
        }
        
        public BuyViewModel()
        {
            InitThings();
            this.BuyCommand = new RelayCommand<MallThing>(
                (t)=>this.Buy(t),
                (t)=>t!=null&&SoftContext.Role!=null);
        }
        private void Buy(MallThing thing)
        {
            TaskContext context = new TaskContext(SoftContext.Role);
        
            /// 任务设置，可用属性为：.Thing .Num .UseLB
            context.Settings.Thing = thing;
            context.Settings.Num = this._number;
            context.Settings.UseLB = this._useLB;
          
            TaskBase task = new BuyThingsTask(context);
            task.Name = "购买商城物品";
      
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
        
    }
}
