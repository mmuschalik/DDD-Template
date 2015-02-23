using Domain.Common.Domain.Model;
using Domain.Common.Infrastructure;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Adapters.Persistance
{
    public class PostgresqlEventStore : IEventStore
    {
        private PostgresqlDocumentStore<DomainEventAggregate> _repository;

        public PostgresqlEventStore(string connectionString)
        {
            _repository = new PostgresqlDocumentStore<DomainEventAggregate>(connectionString);
        }

        public IEnumerable<Domain.Common.DomainEvent> GetDomainEvents(string id)
        {
            return _repository.GetById(id).Select(a => a.DomainEvent).ToList();
        }

        public void Append(string id, IEnumerable<Domain.Common.DomainEvent> events)
        {
            foreach (var e in events)   // may need to add a param with list
                _repository.Add(new DomainEventAggregate(id, e));
        }
    }

}
