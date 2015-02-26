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
        private PostgresqlDocumentStore<StoredDomainEvent> _repository;

        public PostgresqlEventStore(NpgsqlConnection connection)
        {
            _repository = new PostgresqlDocumentStore<StoredDomainEvent>(connection);
        }

        public IEnumerable<DomainEvent> GetAllDomainEventsForStream(string id)
        {
            return DeseralizeDomainEvents(_repository.GetById(id));
        }

        public IEnumerable<DomainEvent> GetAllDomainEventsSince(long startId)
        {
            return DeseralizeDomainEvents(_repository.GetAllSinceSurrogateId(startId));
        }

        public IEnumerable<DomainEvent> GetAllDomainEventsBetween(long startId, long endId)
        {
            return DeseralizeDomainEvents(_repository.GetAllBetweenSurrogateId(startId, endId));
        }

        public void AppendToStream(string id, IEnumerable<Domain.Common.DomainEvent> events)
        {
            _repository.Add(events.Select(e => new StoredDomainEvent(id, JsonConvert.SerializeObject(e), e.GetType().AssemblyQualifiedName, DateTime.Now)));
        }

        public void Append(IEnumerable<DomainEvent> events)
        {
            AppendToStream(string.Empty, events);
        }

        private List<DomainEvent> DeseralizeDomainEvents(IEnumerable<StoredDomainEvent> aggregates)
        {
            return aggregates.Select(a => DeseralizeDomainEvent(a)).ToList();
        }

        private DomainEvent DeseralizeDomainEvent(StoredDomainEvent aggregate)
        {
            return (DomainEvent)JsonConvert.DeserializeObject(aggregate.DomainEventBody, Type.GetType(aggregate.TypeName));
        }

        public IEnumerable<DomainEvent> GetAllDomainEvents()
        {
            return GetAllDomainEventsSince(0);
        }

        public long GetLastDomainEventId()
        {
            return _repository.GetLastSurrogateId();
        }
    }

}
