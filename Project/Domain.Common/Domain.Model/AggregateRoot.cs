using Domain.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    /// <summary>
    /// Abstract aggregate root base class that can hold domain events stemming from aggregate state changes
    /// </summary>
    public abstract class AggregateRoot : IEquatable<AggregateRoot>
    {
        private ICollection<DomainEvent> uncommittedEvents;

        public AggregateRoot(string id)
        {
            Id = id;
            uncommittedEvents = new List<DomainEvent>();
            SurrogateId = 0;
            Version = 0;
        }

        protected AggregateRoot()
        {
            Id = string.Empty;
            uncommittedEvents = new List<DomainEvent>();
            SurrogateId = 0;
            Version = 0;
        }

        public string Id { get; protected internal set; }

        public long SurrogateId { get; internal set; }

        public int Version { get; internal set; }

        public ICollection<DomainEvent> GetUncommittedEvents()
        {
            SaveCreatedEventIfNew();
            return uncommittedEvents.ToList().AsReadOnly();
        }

        protected virtual DomainEvent CreateAggregateDomainEvent()
        {
            return null;
        }

        public bool IsNew()
        {
            return SurrogateId <= 0 && Version <= 0;
        }

        private void SaveCreatedEventIfNew()
        {
            if (uncommittedEvents.Count == 0 && IsNew())
            {
                var createdEvent = CreateAggregateDomainEvent();

                if (createdEvent != null)
                {
                    uncommittedEvents.Add(createdEvent);
                }
            }
        }

        protected void RaiseEvent(DomainEvent domainEvent)
        {
            SaveCreatedEventIfNew();
            uncommittedEvents.Add(domainEvent);
        }

        internal void EventsCommitted()
        {
            uncommittedEvents.Clear();
        }

        public void LoadFromHistory(IEnumerable<DomainEvent> history)
        {
            foreach (var e in history) ApplyChange(e, false);
        }

        protected void ApplyChange(DomainEvent @event)
        {
            ApplyChange(@event, true);
        }

        private void ApplyChange(DomainEvent @event, bool isNew)
        {
            dynamic t = this;
            dynamic e = @event;
            t.Apply(e);
            if (isNew)
                uncommittedEvents.Add(@event);

            Version++;
        }

        public bool Equals(AggregateRoot other)
        {
            return Id.Equals(other.Id) && other.GetType() == this.GetType();
        }
    }
}
