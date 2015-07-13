using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Dm;
using Utilities.Tasks;

namespace DnTool.GameTask
{
    public class ZidongkaidanTask:TaskBase
    {
        public ZidongkaidanTask(TaskContext context)
            : base(context)
        {
          
        }

        protected override void StepsInitialize(ICollection<TaskStep> steps)
        {
            steps.Add(new TaskStep { StepName = "清理背包", Order = 1, RunFunc = RunStep1 });
        }

        private TaskResult RunStep1(TaskContext context)
        {
            IRole role = context.Role;
            DmPlugin dm=role.Window.Dm;
            int hwnd = role.Window.Hwnd;

            Delegater.WaitTrue(() =>
                {
                    return false;
                    
                }, () =>
                {
                    bool ret=role.FindControlTextAndClick(569, 685, "重新开启", true);
                    if (ret == true)
                        Delegater.WaitTrue(() =>
                            {
                                return role.FindControlTextAndClick(566, 496, "确认", true) || role.FindControlTextAndClick(572, 649, "确认", true); 
                            },()=>dm.Delay(500));
                    dm.Delay(1000);
                });
            return TaskResult.Finished;
        }

        protected override int GetStepIndex(TaskContext context)
        {
            return 1;
        }
    }
}
