using Domain.Common.Infrastructure;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure
{
    public class Logger : ILogger
    {
        private static readonly ILog logger =
          LogManager.GetLogger(typeof(Logger));

        public void Debug(string message, params object[] format)
        {
            logger.DebugFormat(message, format);
        }

        public void Error(string message, params object[] format)
        {
            logger.ErrorFormat(message, format);
        }

        public void Info(string message, params object[] format)
        {
            logger.InfoFormat(message, format);
        }
    }
}
