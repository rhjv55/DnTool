using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Tasks
{
    public class TaskRestartException : Exception
    {
         public TaskRestartException()
        {
        }
        public TaskRestartException(string message)
            :base(message)
        {
        }
        public TaskRestartException(string message, Exception inner) 
            :base(message, inner)
        {
        }
    }
}
