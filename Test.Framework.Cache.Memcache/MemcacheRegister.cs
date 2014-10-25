using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Cache.Memcache
{
    public static class MemcacheRegister
    {
        public static void Initialize()
        {
            Container.Register<ICouchbaseClientFactory, CouchbaseClientFactory>();
            FrameworkSettings.ContainerKeys.OutProcCacherKey = FrameworkSettings.ContainerKeys.MemcacheCacherKey;
            Container.Register<ICacher, MemcacheCacher>(FrameworkSettings.ContainerKeys.OutProcCacherKey);
        }
    }
}
