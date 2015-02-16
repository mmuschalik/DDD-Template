using Domain.Common.Infrastructure;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure
{
    public class Logger : ILogger
    {
        private static readonly NLog.Logger logger =
          LogManager.GetCurrentClassLogger();

        public void Debug(string message, params object[] format)
        {
            logger.Debug(message, format);
        }

        public void Error(string message, params object[] format)
        {
            logger.Error(message, format);
        }

        public void Info(string message, params object[] format)
        {
            logger.Info(message, format);
        }
    }
}
