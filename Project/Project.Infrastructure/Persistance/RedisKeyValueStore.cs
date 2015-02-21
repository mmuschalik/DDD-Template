using Domain.Common.Infrastructure;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure
{
    public class RedisKeyValueStore : IKeyValueStore
    {
        private string _host;

        public RedisKeyValueStore(string host)
        {
            _host = host;
        }

        public T Get<T>(string key)
        {
            using (RedisClient redisClient = new RedisClient(_host))
            {
                return redisClient.Get<T>(key);
            }
        }

        public bool ContainsKey(string key)
        {
            using (RedisClient redisClient = new RedisClient(_host))
            {
                return redisClient.ContainsKey(key);
            }
        }

        public void Set(string key, object value)
        {
            using (RedisClient redisClient = new RedisClient(_host))
            {
                redisClient.Set<object>(key, value);
            }
        }
    }
}
