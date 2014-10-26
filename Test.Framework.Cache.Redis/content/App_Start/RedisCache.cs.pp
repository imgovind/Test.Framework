using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;
using Test.Framework.Cache;
using Test.Framework.Cache.Redis;

namespace $rootnamespace$
{
    public partial class AppInitializer
    {
        public partial class RedisCache
        {
            public static void Register()
            {
                Container.Register<IRedisFactory, RedisFactory>();
                Container.Register<ICacher, RedisCacher>(FrameworkSettings.IocKeys.RedisCacherKey);
            }

            public static void Initialize()
            {
                Cacher.InitializeOutProc(Container.Resolve<ICacher>(FrameworkSettings.IocKeys.RedisCacherKey));
            }
        }
    }
}
