using DnTool;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utilities.Dm;
using Utilities.Log;
using DnTool.Utilities.Tasks;

namespace Utilities.Tasks
{
  
     
    public delegate void TaskEventHandler(object sender,TaskEventArg e);
    public delegate void OutMessageHandler(string message);
    public class TaskEventArg:EventArgs
    {
        public TaskContext Context { get; set; }
        public TaskEventArg(TaskContext context)
        {
            this.Context=context;
        }
        public TaskEventArg()
        {
        }
    }
    public class TaskEngine
    {

        public TaskRunState TaskRunState { get; set; }
        private TaskBase _task;
        private TaskEventArg _taskEventArg;
        private Thread _workThread;
        private Thread _detectThread;
        private List<TaskBase> _multiTask=new List<TaskBase>();
        public OutMessageHandler OutMessage;
        public DmWindow Window { get; set; }
       // public bool IsWorking { get; }
        public string CurrentTask { get{return _task.Name;}}
       
        #region 事件
        //事件，就是为了在某个合适的时机，让类的内部能调用类外部定义的功能

        public event TaskEventHandler OnStarted;
        public event TaskEventHandler OnStarting;
        public event TaskEventHandler OnPausing;
        public event TaskEventHandler OnPaused;
        public event TaskEventHandler OnContinuing;
        public event TaskEventHandler OnContinued;
        public event TaskEventHandler OnStopping;
        public event TaskEventHandler OnStopped;
        public event TaskEventHandler OnStateChanged;
        protected void DoEventHandler(TaskEventHandler handler, TaskEventArg e)
        {
            if (handler != null)
            {
                handler.Invoke(this, e);
                //hander.BeginInvoke(this);多线程
            }
        }
        #endregion
        /// <summary>
        /// 任务开始运行
        /// </summary>
        /// <param name="task">任务基类</param>
        public void Start(TaskBase task)
        {
            if (task == null)
                return;           //如果任务为空则返回
            _task = task;         //否则当前任务等于task
            _taskEventArg = new TaskEventArg{ Context=task.Context};
           // Window = task.Context.Role.Window;  //当前窗口等于当前角色所在窗口

            if (_workThread == null)           //没有在执行任务,则获取任务线程
            {
                _workThread = GetWorkThread();
            }
            if (!CheckTaskRunState(TaskRunState.Starting))
            {
                return;
            }
            //if (IsWorking)                      //如果正在工作，则返回
            //{
            //    return;
            //}
            Debug.WriteLine(string.Format("任务“{0}”正在启动.",_task.Name));
           
            TaskRunState = TaskRunState.Starting;  //任务状态改为正在启动
            DoEventHandler(OnStateChanged,_taskEventArg);
            DoEventHandler(OnStarting,_taskEventArg);
            _workThread.Start();                  //任务开始
        }


        public void Pause()
        {
           
            if (_task == null)
            {
                Logger.Info("当前无任务，无法暂停！");
                return;
            }
            TaskRunState = TaskRunState.Pausing;
            _workThread.Suspend();
            TaskRunState = TaskRunState.Paused;
            Logger.Info("任务线程已暂停.");
         
        }
        public void Continue()
        {
            if (_task == null)
            {
                Logger.Info("当前无任务，无法继续！");  
            }
            if (_workThread.ThreadState != System.Threading.ThreadState.Suspended)
            {
                Logger.Info("任务未暂停，无法继续！"); 
            }
            TaskRunState = TaskRunState.Continuing;
            _workThread.Resume();
            TaskRunState = TaskRunState.Continued;
            Logger.Info("任务线程已恢复.");
        }
        public void Stop()
        {
            if (_task==null)
            {
                Logger.Info("当前无任务，无法停止！");
                return;
            }
            if (!CheckTaskRunState(TaskRunState.Stopping))
            {
                return;
            }
            Logger.Info("任务正在停止："+_task.Name);
            if (_workThread!=null)
            {
                try
                {
                    _workThread.Abort();
                    _workThread.Join();
                }
                catch (ThreadStateException)
                {
                    _workThread.Resume();
                    _workThread.Abort();
                    _workThread.Join();
                }
                _workThread = null;
                  
            }
            Logger.Info("任务已停止："+_task.Name);
        }

       

       
        private Thread GetWorkThread()
        {
            return new Thread(() =>
            {
                try
                {
                    TaskStart();
                    TaskStop();
                    Window.FlashWindow();
                    _workThread = null;
                }
                catch (ThreadAbortException)
                {
                    TaskStop();
                }
                catch (Exception ex)
                {
                    TaskStop();
                    Window.FlashWindow();
                    Logger.Error("任务执行失败，"+ex.Message);
                    _workThread = null;
                }
             
            }) { IsBackground=true};
        }

        private void TaskStart()
        {
            DmPlugin dm = Window.Dm;
            bool flag = Delegater.WaitTrue(()=>Window.BindHalfBackground(),()=>dm.Delay(1000),10);
            if (!flag)
            {
                throw new Exception(string.Format("窗口“{0}”绑定失败.",Window.Title));
            }
            TaskRunState = TaskRunState.Running;
            DoEventHandler(OnStateChanged,_taskEventArg);
            DoEventHandler(OnStarted,_taskEventArg);
            Logger.Info("任务启动成功:"+_task.Name);
            TaskResult result = null;
            try
            {
                result = _task.Run();
            }
            catch (TaskRestartException ex)
            {
                Logger.Error("任务执行错误,正在重新启动:" + ex.Message);
                TaskStop();
                TaskStart();
            }
            catch (TaskInterruptException ex)
            {
                TaskStop();
                Window.FlashWindow();
                _workThread = null;
                Logger.Error("任务执行中断："+ex.Message);
            }
            if (result == null)
            {
                return;
            }
            if (result.ResultType == TaskResultType.Success)
            {
                Logger.Info("任务执行完毕"+_task.Name);
                return;
            }
            if (result.ResultType == TaskResultType.Finished)
            {
                Logger.Info("任务执行结束"+_task.Name+result.Message);
            }
        }
        private void TaskStop()
        {

        }
        private bool CheckTaskRunState(TaskRunState taskRunState)
        {
            return true;
        }
        private bool WaitForUnBind()
        {
            return true;
        }


        public void WaitStop()
        {
            while (!_workThread.IsAlive)
            {
            }
            Thread.Sleep(1);
        }

       
    }
}
