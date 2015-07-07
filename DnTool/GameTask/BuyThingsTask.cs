using DnTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Dm;
using Utilities.Log;
using Utilities.Tasks;
namespace DnTool.GameTask
{
    public class BuyThingsTask:TaskBase
    {
        /// <summary>
        /// 任务设置，可用属性为：.Thing .Num .UseLB
        /// </summary>
        private MallThing _thing;
        private int _num;
        private bool _useLB;
        public BuyThingsTask(TaskContext context)
            : base(context)
        {
            Logger.Debug("1");
           _thing = context.Settings.Thing;
           _num = context.Settings.Num;
           _useLB = context.Settings.UseLB;
           Logger.Debug("2");
        }
        protected override void StepsInitialize(ICollection<TaskStep> steps)
        {
            steps.Add(new TaskStep { StepName = "购买物品“{0}”".FormatWith(_thing.Name), Order = 1, RunFunc = RunStep1 });
        }

        private TaskResult RunStep1(TaskContext context)
        {
            
            IRole role = context.Role;
            Role r = (Role)role;
            DmPlugin dm = role.Window.Dm;
            int hwnd=role.Window.Hwnd;

           // if(!role.HasButton("搜索"))  //商城界面是否打开
            //    throw new TaskInterruptException("请先打开商城界面.");

            if (_useLB)
            {
                if (!_thing.CanUseLB)
                    throw new TaskInterruptException("“{0}”无法使用龙币购买.".FormatWith(_thing.Name));
                if (r.MallLB < _thing.Value)
                    throw new TaskInterruptException("龙币不足,无法购买物品“{0}”.".FormatWith(_thing.Name));
            }
            else
            {
                if(r.MallVolume<_thing.Value)
                    throw new TaskInterruptException("点卷不足,无法购买物品“{0}”.".FormatWith(_thing.Name));
            }

            bool ret = Delegater.WaitTrue(() =>
            {
                dm.MoveToClick(567, 43);
                dm.SendString(hwnd, _thing.Name);
                dm.MoveToClick(766, 43);
                return true;
               // return role.HasButton("购买") ? true : false;
            },()=>dm.Delay(1000),10);
            if (ret == false) 
                return new TaskResult(TaskResultType.Failure, "无法找到该商品“{0}”.".FormatWith(_thing.Name));
            role.FindButtonAndClick("购买");
            dm.Delay(500);
          //  if (role.HasDialogBoard("结算"))
           // { 
            if (_useLB)
                dm.MoveToClick(937, 572);
            dm.Delay(500);
              // Delegater.WaitTrue(() => role.HasDialogButton("是"),()=>dm.MoveToClick(608,718));
              // Delegater.WaitTrue(() => !role.HasDialogButton("是"),()=>dm.MoveToClick(508,537));
               dm.MoveToClick(608,718);
                   dm.Delay(500);
               dm.MoveToClick(508, 537);
               Delegater.WaitTrue(() => dm.FindStr(629, 459, 850, 560,"确认","BEBEBE-414141",0.9), () => dm.Delay(100));
              // Delegater.WaitTrue(() => !role.HasDialogButton("确认"), () => dm.MoveToClick(719,506));
               dm.MoveToClick(719, 506);
          //  }
            _num--;
            return _num<=0? TaskResult.Finished : RunStep1(context); 
        }

        protected override int GetStepIndex(TaskContext context)
        {
            return 1;
        }
    }
}
