using Domain.Common;
using Domain.Common.Domain.Model;
using Domain.Common.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Project.Adapters.Persistance
{
    public class PostgresqlRepository<T> : IRepository<T>, IDisposable where T : AggregateRoot
    {
        private PostgresqlDocumentStore<T> _repository;
        private PostgresqlEventStore _eventStore;

        public PostgresqlRepository(string connectionString)
        {
            _repository = new PostgresqlDocumentStore<T>(connectionString);
            _eventStore = new PostgresqlEventStore(connectionString);
        }

        public T GetById(string key)
        {
            return _repository.GetById(key).FirstOrDefault();
        }

        public void Save(T item)
        {
            using (var scope = new TransactionScope())
            {

                if (item.IsNew())
                    _repository.Add(item);
                else
                {
                    int rowsupdated = _repository.Update(item);

                    if (rowsupdated == 0)
                        throw new DBConcurrencyException();
                }

                _eventStore.Append(item.GetType().Name + "-" + item.Id, item.GetUncommittedEvents());
                scope.Complete();
            }

            item.MarkChangesAsCommitted();
            item.Version(item.Version() + 1);   // increase the version for the item, in case it is used again
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }

}
