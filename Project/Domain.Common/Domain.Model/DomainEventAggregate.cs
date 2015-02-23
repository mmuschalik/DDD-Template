using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Domain.Model
{
    public class DomainEventAggregate : AggregateRoot
    {
        public DomainEvent DomainEvent { get; private set; }

        public DomainEventAggregate(string id, DomainEvent @event) : base(id)
        {
            DomainEvent = @event;
        }
    }
}
