using Couchbase.Configuration;
using Enyim.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Cache.Memcache
{
    public interface ICouchbaseClientFactory
    {
        IMemcachedClient Create(ICouchbaseClientConfiguration config);
    }
}
