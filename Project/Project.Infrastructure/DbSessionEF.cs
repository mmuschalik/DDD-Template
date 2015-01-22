using Domain.Common.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure
{
    public class DbSessionEF : DbContext, IDbSession
    {
        public void Add<T>(T item) where T : class
        {
            this.Set<T>().Add(item);
        }

        public void Commit()
        {
            this.SaveChanges();
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return this.Set<T>().AsQueryable();
        }

        public void Remove<T>(T item) where T : class
        {
            this.Set<T>().Remove(item);
        }
    }
}
