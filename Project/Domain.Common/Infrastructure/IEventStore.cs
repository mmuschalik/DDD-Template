using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Infrastructure
{
    public interface IEventStore
    {
        IEnumerable<DomainEvent> GetDomainEvents(string id);
    }
}
