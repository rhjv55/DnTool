using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Tasks
{
    public class NoOpTask:TaskBase
    {
        public NoOpTask(TaskContext context)
            : base(context)
        {
        }
        protected override void StepsInitialize(ICollection<TaskStep> steps)
        {
            
        }

        protected override int GetStepIndex()
        {
            return 1;
        }
    }
}
