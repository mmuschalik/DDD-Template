using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Domain.Model
{
    public class DomainEventAggregate : AggregateRoot
    {
        public string DomainEventBody { get; private set; }

        public string TypeName { get; private set; }

        public DateTime Occurred { get; private set; }

        public DomainEventAggregate(string id, string domainEventBody, string typeName, DateTime occurred) : base(id)
        {
            DomainEventBody = domainEventBody;
            TypeName = typeName;
            Occurred = occurred;
        }
    }
}
