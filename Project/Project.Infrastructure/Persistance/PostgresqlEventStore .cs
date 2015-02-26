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
        private string _connectionString;
        public PostgresqlEventStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public NpgsqlConnection PrepareConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        public IEnumerable<DomainEvent> GetAllDomainEventsForStream(string id)
        {
            using(var con = PrepareConnection())
            {
                var repo = new PostgresqlDocumentStore<StoredDomainEvent>(con);
                return DeseralizeDomainEvents(repo.GetById(id));
            }
        }

        public IEnumerable<DomainEvent> GetAllDomainEventsSince(long startId)
        {
            using (var con = PrepareConnection())
            {
                var repo = new PostgresqlDocumentStore<StoredDomainEvent>(con);
                return DeseralizeDomainEvents(repo.GetAllSinceSurrogateId(startId));
            }
        }

        public IEnumerable<DomainEvent> GetAllDomainEventsBetween(long startId, long endId)
        {
            using (var con = PrepareConnection())
            {
                var repo = new PostgresqlDocumentStore<StoredDomainEvent>(con);
                return DeseralizeDomainEvents(repo.GetAllBetweenSurrogateId(startId, endId));
            }
        }

        public void AppendToStream(string id, IEnumerable<Domain.Common.DomainEvent> events)
        {
            using (var con = PrepareConnection())
            {
                var repo = new PostgresqlDocumentStore<StoredDomainEvent>(con);
                repo.Add(events.Select(e => new StoredDomainEvent(id, JsonConvert.SerializeObject(e), e.GetType().AssemblyQualifiedName, DateTime.Now)));
            }
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
            using (var con = PrepareConnection())
            {
                var repo = new PostgresqlDocumentStore<StoredDomainEvent>(con);
                return repo.GetLastSurrogateId();
            }
        }
    }

}
