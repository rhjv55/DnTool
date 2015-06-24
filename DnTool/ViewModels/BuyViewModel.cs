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
namespace DnTool.ViewModels
{
    public class BuyViewModel:NotifyPropertyChanged
    {
        public RelayCommand<Thing> BuyCommand { get; set; }
        private void InitThings()
        {
            this.Things.Add(new Thing() { ID = 1, Name = "龙裔特别口袋", Description = "70龙玉,70纹章" });
            this.Things.Add(new Thing() { ID = 2, Name = "物品保护魔法药", Description = "强化+8至+12保护" });
        }
        
        public BuyViewModel()
        {
            InitThings();
            this.BuyCommand = new RelayCommand<Thing>((t)=>this.Buy(t));
        }
        private void Buy(Thing thing)
        {
            TaskContext context = new TaskContext(SoftContext.Role);
            /// 任务设置，可用属性为：.Thing .Num .UseLB
            context.Settings.Thing = thing;
            context.Settings.Num = int.Parse(Number);
            context.Settings.UseLB = false;
            SoftContext.TaskEngine.Start(new BuyThingsTask(context));
        }

   
        private List<Thing> _things=new List<Thing>();

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
    }
}
