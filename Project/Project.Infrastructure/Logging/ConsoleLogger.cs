using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Infrastructure
{
    public class ConsoleLogger : ILogger
    {
        public void Debug(string message, params object[] format)
        {
            Console.WriteLine("DEBUG: ");
            Console.WriteLine(message, format);
        }

        public void Error(string message, params object[] format)
        {
            Console.WriteLine("ERROR: ");
            Console.WriteLine(message, format);
        }

        public void Info(string message, params object[] format)
        {
            Console.WriteLine("INFO: ");
            Console.WriteLine(message, format);
        }
    }
}
