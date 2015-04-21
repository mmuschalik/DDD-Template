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
        private string _channel = "channel";

        public DomainEventPublisherService(IRepository<DomainEventsPublishedTracker> repository, IEventStore eventStore, IBus bus)
        {
            _repository = repository;
            _eventStore = eventStore;
            _bus = bus;
        }

        public void Initialise(string channel)
        {
            _channel = channel;
        }

        public void CreateChannel()
        {
            var domainEventsPublishedTracker = new DomainEventsPublishedTracker(_channel, 0);
            _repository.Save(domainEventsPublishedTracker);
        }

        private DomainEventsPublishedTracker GetTracker()
        {
            var domainEventsPublishedTracker = _repository.GetById(_channel);

            if (domainEventsPublishedTracker == null)
                domainEventsPublishedTracker = new DomainEventsPublishedTracker(_channel, 0);

            return domainEventsPublishedTracker;
        }

        public long Publish()
        {
            var domainEventsPublishedTracker = this.GetTracker();

            long lastStoredDomainEventId = _eventStore.GetLastDomainEventId();
            long lastPublishedDomainEventId = domainEventsPublishedTracker.LastPublishedDomainEventId;
            long messagesSent = 0;

            if (lastStoredDomainEventId > lastPublishedDomainEventId)
            {
                var remainingUnpublishedEvents = _eventStore.GetAllDomainEventsBetween(lastPublishedDomainEventId, lastStoredDomainEventId);

                foreach (var @event in remainingUnpublishedEvents)
                {
                    _bus.Publish<DomainEvent>(@event);
                    messagesSent++;
                }
            }

            domainEventsPublishedTracker.UpdateLastPublishedDomainEvent(lastStoredDomainEventId);
            _repository.Save(domainEventsPublishedTracker);

            return messagesSent;
        }
    }
}
