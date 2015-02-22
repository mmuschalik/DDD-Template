using Domain.Common;
using Domain.Common.Infrastructure;
using Project.Infrastructure.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Adapters.Persistance
{
    public class PostgresqlDocumentRepository<T> : RelationalDocumentRepository<T>, IRepository<T> where T : AggregateRoot
    {
        public PostgresqlDocumentRepository(string connectionString) : base(connectionString)
        {

        }
    }
}
