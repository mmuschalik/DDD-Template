using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Infrastructure
{
    public interface IDbSession : IDisposable
    {
        void Add<T>(T item) where T : class;
        void Remove<T>(T item) where T : class;
        IQueryable<T> Query<T>() where T : class;
        void Commit();
    }

}
