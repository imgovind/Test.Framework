using Couchbase;
using Couchbase.Configuration;
using Enyim.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Cache.Memcache
{
    public sealed class CouchbaseClientFactory : ICouchbaseClientFactory
    {
        public IMemcachedClient Create(ICouchbaseClientConfiguration config)
        {
            if (config == null)
                throw new InvalidOperationException("Invalid Couchbase Configuration section: ");
            return new CouchbaseClient(config);
        }
    }
}
