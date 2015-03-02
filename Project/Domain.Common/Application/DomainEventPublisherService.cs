using Domain.Common.Domain.Model;
using Domain.Common.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Application
{
    public class DomainEventPublisherService
    {
        private readonly IRepository<DomainEventsPublishedTracker> _repository;
        private readonly IEventStore _eventStore;
        private readonly IBus _bus;

        public DomainEventPublisherService(IRepository<DomainEventsPublishedTracker> repository, IEventStore eventStore, IBus bus)
        {
            _repository = repository;
            _eventStore = eventStore;
            _bus = bus;
        }

        public void CreateChannel(string channel)
        {
            var domainEventsPublishedTracker = new DomainEventsPublishedTracker(channel, 0);
            _repository.Save(domainEventsPublishedTracker);
        }

        private DomainEventsPublishedTracker GetTracker(string channel)
        {
            var domainEventsPublishedTracker = _repository.GetById(channel);

            if (domainEventsPublishedTracker == null)
                domainEventsPublishedTracker = new DomainEventsPublishedTracker(channel, 0);

            return domainEventsPublishedTracker;
        }

        public void Publish(string channel)
        {
            var domainEventsPublishedTracker = this.GetTracker(channel);

            long lastStoredDomainEventId = _eventStore.GetLastDomainEventId();
            long lastPublishedDomainEventId = domainEventsPublishedTracker.LastPublishedDomainEventId;

            var remainingUnpublishedEvents = _eventStore.GetAllDomainEventsBetween(lastPublishedDomainEventId, lastStoredDomainEventId);

            foreach (var @event in remainingUnpublishedEvents)
            {
                _bus.Publish<DomainEvent>(@event);
            }

            domainEventsPublishedTracker.UpdateLastPublishedDomainEvent(lastStoredDomainEventId);
            _repository.Save(domainEventsPublishedTracker);
        }
    }
}
