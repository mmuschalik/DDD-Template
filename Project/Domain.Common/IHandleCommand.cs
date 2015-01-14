using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    // Basic interface for any classes that handle commands
    public interface IHandleCommand<T> where T : ICommand
    {
        void Handle(T command);
    }
}
