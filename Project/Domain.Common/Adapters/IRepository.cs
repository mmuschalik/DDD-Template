using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Infrastructure
{
    public interface IRepository<T> where T : AggregateRoot
    {
        T GetById(string key);

        void Save(T item);
    }
}
