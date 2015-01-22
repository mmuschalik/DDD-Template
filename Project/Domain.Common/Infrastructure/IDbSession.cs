using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Infrastructure
{
    public interface IDbSession : IDisposable
    {
        void Add<T>(T item);
        void Remove<T>(T item);
        IQueryable<T> Query<T>();
        void Commit();
    }

}
