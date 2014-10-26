using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Cache;

namespace Test.Framework
{
    public static class InProcCacher
    {
        public static ICacher cacher;

        public static void InitializeWith(ICacher cacher)
        {
            Ensure.Argument.IsNotNull(cacher, "cacher");
            InProcCacher.cacher = cacher;
        }

        public static bool Contains(string key)
        {
            Ensure.Argument.IsNotEmpty(key, "key");

            return cacher.Contains(key);
        }

        public static bool TryGet<T>(string key, out T value)
        {
            Ensure.Argument.IsNotEmpty(key, "key");

            return cacher.TryGet<T>(key, out value);
        }

        public static T Get<T>(string key)
        {
            Ensure.Argument.IsNotEmpty(key, "key");

            return cacher.Get<T>(key);
        }

        public static void Set<T>(string key, T value)
        {
            Ensure.Argument.IsNotEmpty(key, "key");

            RemoveIfExists(key);

            cacher.Set(key, value);
        }

        public static void Set<T>(string key, T value, DateTime absoluteExpiration)
        {
            Ensure.Argument.IsNotEmpty(key, "key");
            Ensure.Argument.IsNotInPast(absoluteExpiration, "absoluteExpiration");

            RemoveIfExists(key);

            cacher.Set(key, value, absoluteExpiration);
        }

        public static void Set<T>(string key, T value, TimeSpan slidingExpiration)
        {
            Ensure.Argument.IsNotEmpty(key, "key");
            Ensure.Argument.IsNotNegativeOrZero(slidingExpiration, "slidingExpiration");

            RemoveIfExists(key);

            cacher.Set(key, value, slidingExpiration);
        }

        public static void Remove(string key)
        {
            Ensure.Argument.IsNotEmpty(key, "key");

            cacher.Remove(key);
        }

        internal static void RemoveIfExists(string key)
        {
            if (cacher.Contains(key))
            {
                cacher.Remove(key);
            }
        }

        public static List<string> GetKeys()
        {
            return cacher.CachedKeys;
        }
    }
}
