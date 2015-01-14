using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Infrastructure
{
    public interface ILogger
    {
        void Info(string message, params object[] format);
        void Debug(string message, params object[] format);
        void Error(string message, params object[] format);
    }
}
