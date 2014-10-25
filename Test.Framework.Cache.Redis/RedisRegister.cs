using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Cache;

namespace Test.Framework.Caching.Redis
{
    public static class RedisRegister
    {
        public static void Initialize() 
        {
            Container.Register<IRedisFactory, RedisFactory>();
            FrameworkSettings.ContainerKeys.OutProcCacherKey = FrameworkSettings.ContainerKeys.RedisCacherKey;
            Container.Register<ICacher, RedisCacher>(FrameworkSettings.ContainerKeys.OutProcCacherKey);
        }
    }
}
