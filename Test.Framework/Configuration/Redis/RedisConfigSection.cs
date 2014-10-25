using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework
{
    public class RedisConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("cacheStrings", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(CacheStrings),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public CacheStrings CacheStrings
        {
            get
            {
                return (CacheStrings)base["cacheStrings"];
            }
        }

        public IEnumerable<CacheString> GetCacheStrings()
        {
            var cacheStrings = this.CacheStrings;
            if (cacheStrings == null) yield return null;
            for (int i = 0; i < cacheStrings.Count; i++)
            {
                yield return cacheStrings[i];
            }
        }
    }
}
