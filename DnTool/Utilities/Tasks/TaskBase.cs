
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities.Dm;
using Utilities.Log;


namespace Utilities.Tasks
{
    /// <summary>
    /// 任务基类
    /// </summary>
    public abstract class TaskBase
    {
   
        /// <summary>
        /// 任务的上下文
        /// </summary>
        private readonly TaskContext _context;

        protected TaskBase(TaskContext context)
        {
            _context = context;
         
        }
        public TaskContext Context { get { return _context; } }
        public string Name { get; set; }
        /// <summary>
        /// 重写任务停止时需要做的操作
        /// </summary>
        /// <param name="role"></param>
        protected virtual void OnStopping(IRole role) { }
        /// <summary>
        /// 重写任务开始时需要做的操作
        /// </summary>
        /// <param name="role"></param>
        protected virtual void OnStarting(IRole role) { }

        protected abstract void StepsInitialize(ICollection<TaskStep> steps);
        protected abstract int GetStepIndex(TaskContext context);

        public TaskResult Run()
        {
            TaskResult result = CanRun(_context);                //检查任务是否可运行
            if (result.ResultType != TaskResultType.Success)     //如果失败，返回
            {
                return result;
            }
            OnStarting(_context.Role);                            //任务开始时操作
            _context.TaskSteps.Clear();                           //清空任务上下文中的步骤
            StepsInitialize(_context.TaskSteps);                  //初始化任务步骤
            //获取任务执行到哪个步骤
            _context.StepIndex = GetStepIndex(_context);

            //循环执行每个步骤，出现步骤失败则返回任务失败
            while (true)
            {
                foreach (TaskStep taskStep in _context.TaskSteps)
                {
                    if (_context.StepIndex > taskStep.Order)
                    {
                    continue;
                    }
                    result = taskStep.RunFunc(_context);
                    if (result.ResultType != TaskResultType.Success)
                    {
                        return result;
                    }
                }
                _context.StepIndex = 1;
            }
           
            
            return TaskResult.Finished;
        }
        /// <summary>
        /// 检查任务是否满足运行条件，如等级，物品等
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual TaskResult CanRun(TaskContext context)
        {
            return new TaskResult() { ResultType=TaskResultType.Failure,Message=""};
        }
       







    }
}
