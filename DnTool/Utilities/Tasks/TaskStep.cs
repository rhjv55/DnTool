using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Tasks
{
    /// <summary>
    /// 任务执行步骤
    /// </summary>
    public  class TaskStep
    {
       /// <summary>
       /// 获取或设置步骤名称
       /// </summary>
       public string StepName { get; set; }
       /// <summary>
       /// 获取或设置步骤序号
       /// </summary>
       public int Order { get; set; }
       /// <summary>
       /// 获取或设置步骤执行方法
       /// </summary>
       public Func<TaskContext,TaskResult> RunFunc { get; set; }
    }
}
