using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    /// <summary>
    /// Interface for classes that want to react to domain events in the system
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHandleDomainEvent<T> where T : DomainEvent
    {
        void Handle(T @event);
    }
}
