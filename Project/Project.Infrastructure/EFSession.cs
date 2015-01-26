using Domain.Common.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure
{
    public class EFSession : IDbSession
    {
        protected readonly DbContext Context;

        public EFSession(DbContext context)
        {
            Context = context;
        }

        public void Add<T>(T item) where T : class
        {
            Context.Set<T>().Add(item);
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return Context.Set<T>().AsQueryable();
        }

        public void Remove<T>(T item) where T : class
        {
            Context.Set<T>().Remove(item);
        }

        public T Load<T>(object id) where T : class
        {
            return Context.Set<T>().Find(id);
        }
    }
}
