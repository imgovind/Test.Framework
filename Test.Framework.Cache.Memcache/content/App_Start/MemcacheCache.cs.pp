using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;
using Test.Framework.Cache;
using Test.Framework.Cache.Memcache;

namespace $rootnamespace$
{
    public partial class AppInitializer
    {
        public partial class MemcacheCache
        {
            public static void Register()
            {
                Container.Register<ICouchbaseClientFactory, CouchbaseClientFactory>();
                FrameworkSettings.IocKeys.OutProcCacherKey = FrameworkSettings.IocKeys.MemcacheCacherKey;
                Container.Register<ICacher, MemcacheCacher>(FrameworkSettings.IocKeys.MemcacheCacherKey);
            }

            public static void Initialize()
            {
                Cacher.InitializeOutProc(Container.Resolve<ICacher>(FrameworkSettings.IocKeys.MemcacheCacherKey));
            }
        }
    }
}
