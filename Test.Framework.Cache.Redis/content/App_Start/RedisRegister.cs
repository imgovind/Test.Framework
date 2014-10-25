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
            FrameworkSettings.IocKeys.OutProcCacherKey = FrameworkSettings.IocKeys.RedisCacherKey;
            Container.Register<ICacher, RedisCacher>(FrameworkSettings.IocKeys.OutProcCacherKey);
        }
    }
}
