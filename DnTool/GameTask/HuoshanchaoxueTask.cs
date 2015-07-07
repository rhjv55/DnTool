using DnTool.Models;
using DnTool.ViewModels;
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
    public class HuoshanchaoxueTask:TaskBase
    {
        public HuoshanchaoxueTask(TaskContext context)
            : base(context)
        {

        }

        protected override void StepsInitialize(ICollection<TaskStep> steps)
        {
            steps.Add(new TaskStep { StepName ="刷火山巢穴", Order = 1, RunFunc = RunStep1 });
        }
        ViewModelLocator Locator = new ViewModelLocator();
        private TaskResult RunStep1(TaskContext context)
        {
            IRole role = context.Role;
            DmPlugin dm = role.Window.Dm;
            int hwnd = role.Window.Hwnd;
            dm.MoveToClick(97,829);
            for (int i = 0; i < 50; i++)
            {
                dm.MoveTo(512+i,416);
            }
            //Locator.Teleport.Teleport(new Point() { Name = "进图入口", X = -1356.5f, Y = -1791.6f, Z = 15.5f });
            //dm.Delay(2000);
            //dm.MoveToClick(910, 701); //入场
            //dm.Delay(10000);
            //Locator.Teleport.Teleport(new Point() { Name = "第四关入口", X = 4661.5f, Y = 4338.5f, Z = -110.6f });
            //dm.MoveToRightClick(335, 825); //月光碎片
            //dm.Delay(14000);
            //Locator.Teleport.Teleport(new Point() { Name = "Boss输出坐标", X = 1793f, Y = 2434f, Z = -1012f });
            //dm.MoveToRightClick(224, 825); //致命劈砍
            //dm.Delay(14000);
            //dm.MoveToRightClick(722, 825); //水月神舞
            //dm.Delay(2000);
            //Locator.Teleport.Teleport(new Point() { Name = "安全坐标", X = 50000f, Y = 50000f, Z = 0f });
            //dm.MoveToRightClick(832, 825);
            //Delegater.WaitTrue(() => dm.FindStr(229, 175, 1103, 511, "箱子1", "ffffff-222222"), () => dm.Delay(1000), 120);
            //dm.Delay(18000);
            //Locator.Teleport.Teleport(new Point() { Name = "箱子坐标", X = 1641.3f, Y = 2322.4f, Z = -1012 });
            //dm.WriteFloat(hwnd, "440404B4",10f);
            //dm.LeftClick();
            //dm.Delay(5000);
            return TaskResult.Finished;
        }

        protected override int GetStepIndex(TaskContext context)
        {
            return 1;
        }
    }
}
