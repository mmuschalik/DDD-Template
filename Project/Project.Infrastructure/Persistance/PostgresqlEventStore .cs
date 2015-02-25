using Domain.Common.Domain.Model;
using Domain.Common.Infrastructure;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Project.Adapters.Persistance
{
    public class PostgresqlEventStore : IEventStore
    {
        private PostgresqlDocumentStore<DomainEventAggregate> _repository;

        public PostgresqlEventStore(string connectionString)
        {
            _repository = new PostgresqlDocumentStore<DomainEventAggregate>(connectionString);
        }

        public IEnumerable<Domain.Common.DomainEvent> GetAllDomainEventsForStream(string id)
        {
            return DeseralizeDomainEvents(_repository.GetById(id));
        }

        public IEnumerable<Domain.Common.DomainEvent> GetAllDomainEventsSince(long startId)
        {
            return DeseralizeDomainEvents(_repository.GetAllSinceSurrogateId(startId));
        }

        public IEnumerable<Domain.Common.DomainEvent> GetAllDomainEventsBetween(long startId, long endId)
        {
            return DeseralizeDomainEvents(_repository.GetAllBetweenSurrogateId(startId, endId));
        }

        public void AppendToStream(string id, IEnumerable<Domain.Common.DomainEvent> events)
        {
            foreach (var e in events)   // may need to add a param with list
                _repository.Add(new DomainEventAggregate(id, JsonConvert.SerializeObject(e), e.GetType().AssemblyQualifiedName, DateTime.Now));
        }

        public void Append(IEnumerable<Domain.Common.DomainEvent> events)
        {
            AppendToStream("", events);
        }

        private List<Domain.Common.DomainEvent> DeseralizeDomainEvents(IEnumerable<DomainEventAggregate> aggs)
        {
            return aggs.Select(a => (Domain.Common.DomainEvent)JsonConvert.DeserializeObject(a.DomainEventBody, Type.GetType(a.TypeName))).ToList();
        }

        public IEnumerable<DomainEvent> GetAllDomainEvents()
        {
            return GetAllDomainEventsSince(0);
        }
    }

}
