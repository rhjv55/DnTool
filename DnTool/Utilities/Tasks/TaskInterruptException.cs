using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Tasks
{
    public class TaskInterruptException : Exception
    {
        public TaskInterruptException()
        {
        }
        public TaskInterruptException(string message)
            :base(message)
        {
        }
        public TaskInterruptException(string message, Exception inner) 
            :base(message, inner)
        {
        }
    }
}
