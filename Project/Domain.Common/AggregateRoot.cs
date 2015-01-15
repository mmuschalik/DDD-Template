﻿using System;
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

        public AggregateRoot(object id)
        {
            Id = id;
            uncommittedEvents = new List<DomainEvent>();
        }

        public AggregateRoot()
        {

        }

        public object Id { get; private set; }

        public int Version { get; internal set; }

        public ICollection<DomainEvent> GetUncommittedEvents()
        {
            return uncommittedEvents.ToList().AsReadOnly();
        }

        protected void RaiseEvent(DomainEvent domainEvent)
        {
            uncommittedEvents.Add(domainEvent);
        }

        public void MarkChangesAsCommitted()
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
            t.Apply(@event);
            if (isNew) uncommittedEvents.Add(@event);
        }

        public bool Equals(AggregateRoot other)
        {
            return Id.Equals(other.Id) && other.GetType() == this.GetType();
        }
    }
}