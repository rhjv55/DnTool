using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Utilities.Dm;

namespace Utilities.Tasks
{
    /// <summary>
    /// 任务上下文
    /// </summary>
    public class TaskContext
    {
        public TaskContext(IRole role,Function function)
        {
           Role = role;
           Function = function;
           Settings = new ExpandoObject();
           StepIndex = 0;
           TaskSteps = new List<TaskStep>();
        }
        #region xxx
        //private string name;
        //private string description;
        //private Type jobType;
        //private Dictionary<object, object> settings;
        //private bool shouldRecover;
     
        
        //public string Name
        //{
        //    get { return name; }
        //    set
        //    {
        //        if (value == null || value.Trim().Length == 0)
        //        {
        //            throw new ArgumentException("任务名字不能为空.");
        //        }
        //        name = value;
        //    }
        //}
        //public string Description
        //{
        //    get { return description; }
        //    set { description = value; }
        //}
        //public Type JobType
        //{
        //    get { return jobType; }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            throw new ArgumentException("任务类不能为null.");
        //        }

        //        if (!typeof (TaskBase).IsAssignableFrom(value))
        //        {
        //            throw new ArgumentException("任务类必须继承自TaskBase.");
        //        }
        //        jobType = value;
        //    }
        //}
        //public Dictionary<object, object> Settings
        //{
        //    get
        //    {
        //        if (settings == null)
        //        {
        //            settings = new Dictionary<object, object>();
        //        }
        //        return settings;
        //    }

        //    set { settings = value; }
        //}
        //public bool RequestsRecovery
        //{
        //    set { shouldRecover = value; }
        //    get { return shouldRecover; }
        //}

        #endregion
        /// <summary>
        /// 获取 功能信息
        /// </summary>
        public Function Function { get; private set; }
        /// <summary>
        /// 获取 角色信息
        /// </summary>
        public IRole Role { get;private set; }
        /// <summary>
        /// 任务设置信息
        /// </summary>
        public ExpandoObject Settings { get; set; }
        /// <summary>
        /// 即将或正在执行的任务步骤序号
        /// </summary>
        public int StepIndex { get; set; }
        /// <summary>
        /// 任务步骤集合
        /// </summary>
        public ICollection<TaskStep> TaskSteps { get; private set; }
    }

    public enum Function
    {
        fun
    }
}
