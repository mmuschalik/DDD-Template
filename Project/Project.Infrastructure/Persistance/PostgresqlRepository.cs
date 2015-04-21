using Domain.Common;
using Domain.Common.Adapters;
using Domain.Common.Domain.Model;
using Domain.Common.Infrastructure;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Project.Adapters.Persistance
{
    public class PostgresqlRepository<T> : RepositoryBase, IRepository<T> where T : AggregateRoot
    {
        private string _connectionString;

        public PostgresqlRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public T GetById(string key)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                var repo = new PostgresqlDocumentStore<T>(conn);
                var agg = repo.GetById(key).FirstOrDefault();
                return agg;
            }
                
        }

        public void Save(T item)
        {
            bool isNew = item.IsNew();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    var repo = new PostgresqlDocumentStore<T>(conn);

                    var eventStore = new PostgresqlDocumentStore<StoredDomainEvent>(conn);
                    eventStore.Add(item.GetUncommittedEvents().Select(e => 
                        new StoredDomainEvent(item.GetType().Name + "-" + item.Id, 
                            JsonConvert.SerializeObject(e), 
                            e.GetType().AssemblyQualifiedName, DateTime.Now)));

                    if (isNew)
                        repo.Add(item);
                    else
                    {
                        int rowsupdated = repo.Update(item);

                        if (rowsupdated == 0)
                            throw new DBConcurrencyException();
                    }
                    
                    tran.Commit();
                }
            }

            this.EventsCommitted(item);

            if(!isNew)
                this.SetVersion(item, item.Version + 1);
        }
    }

}
