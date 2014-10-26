using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Cache
{
    public class NullCacher : ICacher
    {
        public int Count
        {
            get { return 0; }
        }

        public T Get<T>(string key)
        {
            return default(T);
        }

        public void Remove(string key)
        {
        }

        public bool Contains(string key)
        {
            return false;
        }

        public List<string> CachedKeys
        {
            get { return null;}
        }

        public void Set<T>(string key, T value)
        {
        }

        public void Set<T>(string key, T value, DateTime absoluteExpiration)
        {
        }

        public void Set<T>(string key, T value, TimeSpan slidingExpiration)
        {
        }

        public bool TryGet<T>(string key, out T value)
        {
            value = default(T);
            return false;
        }
    }
}
