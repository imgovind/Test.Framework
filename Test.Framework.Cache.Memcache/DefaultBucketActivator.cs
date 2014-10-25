using Couchbase.Configuration;
using Enyim.Caching;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Cache.Memcache
{
    public static class DefaultBucketActivator
    {
        #region Members

        private static IMemcachedClient memcachedClient;

        public static IMemcachedClient Cache
        {
            get
            {
                if (memcachedClient == null)
                {
                    Initialize();
                }
                return memcachedClient;
            }
        }

        private static void Initialize()
        {
            ICouchbaseClientFactory factory = Container.Resolve<ICouchbaseClientFactory>();
            if (factory != null)
            {
                memcachedClient = factory.Create((ICouchbaseClientConfiguration)ConfigurationManager.GetSection("default-bucket"));
            }
        }

        #endregion
    }
}
