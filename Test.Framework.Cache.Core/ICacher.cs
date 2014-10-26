using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Cache
{
    public interface ICacher
    {
        int Count { get; }
        T Get<T>(string key);
        void Remove(string key);
        bool Contains(string key);
        List<string> CachedKeys { get; }
        void Set<T>(string key, T value);
        void Set<T>(string key, T value, DateTime absoluteExpiration);
        void Set<T>(string key, T value, TimeSpan slidingExpiration);
        bool TryGet<T>(string key, out T value);
    }
}
