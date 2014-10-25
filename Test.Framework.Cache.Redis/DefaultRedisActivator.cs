using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Caching.Redis
{
    public static class DefaultRedisActivator
    {
        public static void Initialize()
        {
            IWebConfiguration configuration = Container.Resolve<IWebConfiguration>();
            IRedisFactory factory = Container.Resolve<IRedisFactory>();
            if (factory != null)
            {
                var redisConfig = configuration.GetSection<RedisConfigSection>("redis");
                if (redisConfig == null) return;
                var cacheStrings = redisConfig.GetCacheStrings();
                foreach (var cacheString in cacheStrings)
                {
                    factory.CreateBasicClient(cacheString.Name, cacheString.Server, cacheString.Port, cacheString.Password);
                }
            }
        }

        public static void InitializeStackExchange()
        {
            IWebConfiguration configuration = Container.Resolve<IWebConfiguration>();
            IRedisFactory factory = Container.Resolve<IRedisFactory>();
            if (factory != null)
            {
                var redisConfig = configuration.GetSection<RedisConfigSection>("redis");
                if (redisConfig == null) return;
                var cacheStrings = redisConfig.GetCacheStrings();
                foreach (var cacheString in cacheStrings)
                {
                    factory.CreateClient(cacheString.Name, cacheString.Server, cacheString.Port, cacheString.DbValue, cacheString.Password);
                }
            }
        }
    }
}
