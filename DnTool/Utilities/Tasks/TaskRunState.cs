using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Tasks
{
    public enum TaskRunState
    {
      
        Initialize,
        Starting,
        Started,
        Pausing,
        Paused,
        Continuing,
        Continued,
        Stopped,
        Stopping,
        Running
    }
}
