using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Domain.Model
{
    public class StoredDomainEvent : AggregateRoot
    {
        public string DomainEventBody { get; private set; }

        public string TypeName { get; private set; }

        public DateTime Occurred { get; private set; }

        public StoredDomainEvent(string id, string domainEventBody, string typeName, DateTime occurred) : base(id)
        {
            DomainEventBody = domainEventBody;
            TypeName = typeName;
            Occurred = occurred;
        }
    }
}
