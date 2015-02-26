using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Infrastructure
{
    public interface IEventStore
    {
        void Append(IEnumerable<DomainEvent> events);

        long GetLastDomainEventId();

        IEnumerable<DomainEvent> GetAllDomainEvents();

        IEnumerable<DomainEvent> GetAllDomainEventsSince(long startId);

        IEnumerable<DomainEvent> GetAllDomainEventsBetween(long startId, long endId);

        IEnumerable<DomainEvent> GetAllDomainEventsForStream(string streamId);

        void AppendToStream(string id, IEnumerable<DomainEvent> events);
    }
}
