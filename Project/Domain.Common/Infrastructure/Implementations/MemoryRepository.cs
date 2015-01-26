using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Infrastructure
{
    public class MemoryRepository<T> : IRepository<T> where T : AggregateRoot
    {
        private readonly Dictionary<string, T> _dictionary = new Dictionary<string, T>();

        public T GetById(object key)
        {
            if (!_dictionary.ContainsKey(key.ToString()))
                return null;

            return _dictionary[key.ToString()];
        }

        public void Save(T item)
        {
            _dictionary[item.Id.ToString()] = item;
        }
    }
}
