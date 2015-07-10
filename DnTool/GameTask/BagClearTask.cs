using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Dm;
using Utilities.Tasks;

namespace DnTool.GameTask
{
    public class BagClearTask:TaskBase
    {
        private int _beginPage;
        private int _beginItem;
        private int _stopPage;
        private int _stopItem;
        public BagClearTask(TaskContext context)
            : base(context)
        {
            _beginPage = context.Settings.BeginPage;
            _beginItem = context.Settings.BeginItem;
            _stopPage = context.Settings.StopPage;
            _stopItem = context.Settings.StopItem;
        }

        protected override void StepsInitialize(ICollection<TaskStep> steps)
        {
            steps.Add(new TaskStep { StepName = "清理背包", Order = 1, RunFunc = RunStep1 });
        }

        private TaskResult RunStep1(TaskContext context)
        {
            IRole role = context.Role;

            bool ret = role.HasBoard("拥有物品");
            if (ret != true) 
                throw new TaskInterruptException("背包未打开！");
            //第一页坐标：    第一格坐标：
            role.BagCleanup(_beginPage,_beginItem,_stopPage,_stopItem);

            return TaskResult.Finished;
        }

        protected override int GetStepIndex(TaskContext context)
        {
            return 1;
        }
    }
}
