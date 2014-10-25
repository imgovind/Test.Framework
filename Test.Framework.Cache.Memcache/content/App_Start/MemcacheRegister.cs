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
            FrameworkSettings.IocKeys.OutProcCacherKey = FrameworkSettings.IocKeys.MemcacheCacherKey;
            Container.Register<ICacher, MemcacheCacher>(FrameworkSettings.IocKeys.OutProcCacherKey);
        }
    }
}
