using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Log
{
    public class Logger
    {
        private static ILog _log = LogManager.GetLogger(Type.GetType("System.Object"));
        static Logger()
        {
            log4net.Config.XmlConfigurator.Configure();
            // log4net.Repository.Hierarchy.Logger
        }

        public static void GetLog()
        {
            ILog _ll = LogManager.GetLogger(Type.GetType(""));
        }
        public static void Info(string message,params object[] args)
        {
            _log.Info(string.Format(message,args));
        }

        public static void Info(string message)
        {
                _log.Info(message);
        }
        public static void Info(object message)
        {
            _log.Info(message.ToString());
        }
        public static void Debug(string message)
        {
                _log.Debug(message); 
        }
        public static void Error(string message)
        {
                _log.Error(message);
        }
    }
}
