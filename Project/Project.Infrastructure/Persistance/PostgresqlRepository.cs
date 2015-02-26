using Domain.Common;
using Domain.Common.Adapters;
using Domain.Common.Domain.Model;
using Domain.Common.Infrastructure;
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
                return repo.GetById(key).FirstOrDefault();
            }
                
        }

        public void Save(T item)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    var repo = new PostgresqlDocumentStore<T>(conn);

                    if (item.IsNew())
                        repo.Add(item);
                    else
                    {
                        int rowsupdated = repo.Update(item);

                        if (rowsupdated == 0)
                            throw new DBConcurrencyException();
                    }

                    var eventStore = new PostgresqlEventStore(conn);
                    eventStore.AppendToStream(item.GetType().Name + "-" + item.Id, item.GetUncommittedEvents());

                    tran.Commit();
                }
            }

            this.EventsCommitted(item);
            this.SetVersion(item, item.Version + 1);   // increase the version for the item, in case it is used again
        }
    }

}
