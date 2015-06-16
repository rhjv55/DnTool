using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Tasks
{

    public class TaskResult
    {

        public static TaskResult Success=new TaskResult(TaskResultType.Success,"执行成功!");
        public static TaskResult Finished=new TaskResult(TaskResultType.Finished, "执行结束!");
        public TaskResult(TaskResultType type, string message)
        {
            this.ResultType = type;
            this.Message = message;
        }

        public TaskResult() { }

        public TaskResultType ResultType { get; set; }
        public string Message { get; set; }
    }

    public enum TaskResultType
    {
        /// <summary>
        /// 任务步骤执行成功
        /// </summary>
        Success,
        /// <summary>
        /// 任务步骤执行失败
        /// </summary>
        Failure,

        /// <summary>
        /// 任务执行完毕
        /// </summary>
        Finished,
       
    }
}
