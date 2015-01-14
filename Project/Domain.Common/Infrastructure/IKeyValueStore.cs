using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Infrastructure
{
    public interface IKeyValueStore
    {
        T Get<T>(string key);

        void Set(string key, object value);
    }
}
