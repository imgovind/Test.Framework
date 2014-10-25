using Enyim.Caching;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Cache;

namespace Test.Framework.Cache.Memcache
{
    public class MemcacheCacher : ICacher
    {
                #region Members/Constructors

        private IMemcachedClient cache;

        public MemcacheCacher()
        {
            cache = DefaultBucketActivator.Cache;
        }

        #endregion

        #region ICache Members

        public int Count
        {
            get { return 0; }
        }

        public void Remove(string key)
        {
            cache.Remove(key);
        }

        public bool Contains(string key)
        {
            return cache.Get(key) != null;
        }

        public void Set<T>(string key, T value)
        {
            cache.Store(StoreMode.Set, key, value, new TimeSpan(0, 0, Convert.ToInt32(FrameworkSettings.CachingIntervalInMinutes), 0, 0));
        }

        public void Set<T>(string key, T value, DateTime absoluteExpiration)
        {
            cache.Store(StoreMode.Set, key, value, absoluteExpiration);
        }

        public void Set<T>(string key, T value, TimeSpan slidingExpiration)
        {
            cache.Store(StoreMode.Set, key, value, slidingExpiration);
        }

        public bool TryGet<T>(string key, out T value)
        {
            value = default(T);
            object cached = new object();
            if (cache.TryGet(key, out cached))
            {
                value = (T)cached;
                return true;
            }
            return false;
        }

        public T Get<T>(string key)
        {
            T result = default(T);
            object cached = cache.Get(key);
            if (cached != null)
            {
                result = (T)cached;
            }
            return result;
        }

        public List<string> CachedKeys
        {
            get
            {
                return new List<string>();
            }
        }

        #endregion
    }
}
