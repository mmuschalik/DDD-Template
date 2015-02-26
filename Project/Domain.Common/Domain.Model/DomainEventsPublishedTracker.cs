using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Domain.Model
{
    public class DomainEventsPublishedTracker : AggregateRoot
    {
        public long LastPublishedDomainEventId { get; private set; }

        public DomainEventsPublishedTracker(string id, long lastPublishedDomainEventId)
            : base(id)
        {
            LastPublishedDomainEventId = lastPublishedDomainEventId;
        }

        public void UpdateLastPublishedDomainEvent(long latestid)
        {
            LastPublishedDomainEventId = latestid;
        }

    }
}
