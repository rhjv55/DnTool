using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Dm;
using Utilities.Tasks;
namespace DnTool.GameTask
{
    public class BuyThingsTask:TaskBase
    {
        /// <summary>
        /// 任务设置，可用属性为：.Map .ZiYuans  .RightSkill  .UpLevelEnabled
        /// </summary>
        private readonly dynamic _setting;
        private int _leftSkillId;

        public BuyThingsTask(TaskContext context)
            : base(context)
        {
            _setting = context.Settings;
        }
        protected override void StepsInitialize(ICollection<TaskStep> steps)
        {
            steps.Add(new TaskStep { StepName = "", Order = 1, RunFunc = RunStep1 });
            steps.Add(new TaskStep { StepName = "", Order = 2, RunFunc = RunStep2 });
            steps.Add(new TaskStep { StepName = "", Order = 3, RunFunc = RunStep3 });
        }

        private TaskResult RunStep3(TaskContext context)
        {
            throw new NotImplementedException();
        }

        private TaskResult RunStep2(TaskContext context)
        {
            IRole role = context.Role;
            DmPlugin dm = role.Window.Dm;
            Delegater.WaitTrue(()=>role.FindNpc("",""),()=>dm.Delay(1000));
            role.FindDialogButtonAndClick("");
            Delegater.WaitTrue(()=>role.HasDialogButton(""),()=>dm.MoveToClick(400,250));
            role.FindDialogButtonAndClick("");
            role.CloseDialogBoard();
            //TaskItem task = role.GetTaskItem(Name);
            //return task!=null&&task.Step.Contains("你需要收集")?TaskResult.Success:RunStep2(context);
            return TaskResult.Success;
        }

        private TaskResult RunStep1(TaskContext context)
        {
            IRole role = context.Role;
            DmPlugin dm = role.Window.Dm;
            Delegater.WaitTrue(()=>role.FindNpc("",""),()=>dm.Delay(1000));
            role.FindDialogButtonAndClick("");
            role.FindDialogButtonAndClick("");
            role.CloseDialogBoard();
            //TaskItem task = role.GetTaskItem(Name);
            //return task!=null&&task.Step.Contains("答应再次营救武吉")?TaskResult.Success:RunStep1(context);
            return new TaskResult(TaskResultType.Failure,"角色声望不足50点，无法兌换道具时空符");

        }

        protected override int GetStepIndex(TaskContext context)
        {
            IRole role = context.Role;
            if (role.Empirical <= 0)
            {
                return 4;
            }
           //有是空腹 返回1
            //快捷键 无 返回2
            //防御》=20 返回3
            //返回4
            return 4;
        }
    }
}
