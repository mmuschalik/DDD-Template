using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Infrastructure
{
    public class MemoryKeyValueStore : IKeyValueStore
    {
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public T Get<T>(string key)
        {
            return (T)_dictionary[key];
        }

        public void Set(string key, object value)
        {
            _dictionary[key] = value;
        }
    }
}
