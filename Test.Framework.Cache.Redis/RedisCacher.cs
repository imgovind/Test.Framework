using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Cache;

namespace Test.Framework.Caching.Redis
{
    public sealed class RedisCacher : ICacher
    {
        private IRedisClientsManager clientsManager;
        public RedisCacher()
        {
            DefaultRedisActivator.Initialize();
            this.clientsManager = Container.Resolve<IRedisClientsManager>("Test");
        }

        public int Count
        {
            get
            {
                return 0;
            }
        }

        public T Get<T>(string key)
        {
            using (var client = clientsManager.GetClient())
            {
                return client.Get<T>(key);
            }
        }

        public void Remove(string key)
        {
            using (var client = clientsManager.GetClient())
            {
                client.Remove(key);
            }
        }

        public bool Contains(string key)
        {
            using (var client = clientsManager.GetClient())
            {
                return client.ContainsKey(key);
            }
        }

        public List<string> CachedKeys
        {
            get
            {
                using (var client = clientsManager.GetClient())
                {
                    return client.GetAllKeys();
                }
            }
        }

        public void Set<T>(string key, T value)
        {
            using (var client = clientsManager.GetClient())
            {
                client.Add<T>(key, value);
            }
        }

        public void Set<T>(string key, T value, DateTime absoluteExpiration)
        {
            using (var client = clientsManager.GetClient())
            {
                client.Add<T>(key, value, absoluteExpiration);
            }
        }

        public void Set<T>(string key, T value, TimeSpan slidingExpiration)
        {
            using (var client = clientsManager.GetClient())
            {
                client.Add<T>(key, value, slidingExpiration);
            }
        }

        public bool TryGet<T>(string key, out T value)
        {
            using (var client = clientsManager.GetClient())
            {
                value = default(T);
                object cached = new object();
                if (!Contains(key)) return false;
                value = Get<T>(key);
                return true;
            }
        }
    }
}
