using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Dm;
using Utilities.Log;

namespace Utilities.Tasks
{
    public class TaskBuilder
    {
        private string name;
        private string description;
        private Type jobType = typeof (NoOpTask);
        private bool shouldRecover;
        private Dictionary<object, object> setting = new Dictionary<object, object>();

        protected TaskBuilder()
        {
        }
        public static TaskBuilder Create<T>() where T : TaskBase
        {
            TaskBuilder b = new TaskBuilder();
            b.OfType(typeof(T));
            return b;
        }

        public TaskBase NewJob(TaskContext context)
        {

            try
            {
                //Logger.Debug(string.Format("Producing instance of Job '{0}', class={1}", name, jobType.FullName));
                return ObjectUtils.InstantiateType<TaskBase>(jobType,context);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Problem instantiating class '{0}':{1}", jobType.FullName,e.Message));
               
            }
        }

        public TaskBase Build(DmWindow window)
        {
            if (window == null)
            {
                throw new Exception("window不能为空");
            }
            TaskContext context = new TaskContext(window,Function.fun);
            //context.JobType = jobType;
            //context.Description = description;
            //if (name == null)
            //{
            //    name = "default";
            //}
            //context.Name = name;
            //context.RequestsRecovery = shouldRecover;

            //if (setting.Count!=0)
            //{
            //    context.Settings = setting;
            //}
            return this.NewJob(context);;
           // return job;
        }

        public TaskBuilder WithIdentity(string name)
        {
            this.name = name;
            return this;
        }

        public TaskBuilder WithDescription(string description)
        {
            this.description = description;
            return this;
        }

        public TaskBuilder OfType<T>()
        {
            return OfType(typeof(T));
        }

        public TaskBuilder OfType(Type type)
        {
            jobType = type;
            return this;
        }

        public TaskBuilder RequestRecovery()
        {
            this.shouldRecover = true;
            return this;
        }

        public TaskBuilder RequestRecovery(bool shouldRecover)
        {
            this.shouldRecover = shouldRecover;
            return this;
        }

        #region 设置任务参数
        public TaskBuilder UsingJobData(string key, string value)
        {
            setting.Add(key, value);
            return this;
        }

        public TaskBuilder UsingJobData(string key, int value)
        {
            setting.Add(key, value);
            return this;
        }

        public TaskBuilder UsingJobData(string key, long value)
        {
            setting.Add(key, value);
            return this;
        }

        public TaskBuilder UsingJobData(string key, float value)
        {
            setting.Add(key, value);
            return this;
        }


        public TaskBuilder UsingJobData(string key, double value)
        {
            setting.Add(key, value);
            return this;
        }


        public TaskBuilder UsingJobData(string key, bool value)
        {
            setting.Add(key, value);
            return this;
        }

        //public TaskBuilder UsingJobData(object key, object value)
        //{
        //    setting.Add(key, value);
        //    return this;
        //}
      
        #endregion
       
    }
}
