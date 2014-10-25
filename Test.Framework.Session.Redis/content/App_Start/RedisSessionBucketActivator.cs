using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Session.Redis
{
    public class RedisSessionBucketActivator
    {

        public void Initialize()
        {
            this.clientManager = new PooledRedisClientManager("localhost:6379");
            RedisSessionStateStoreProvider.SetClientManager(this.clientManager);
            RedisSessionStateStoreProvider.SetOptions(new RedisSessionStateStoreOptions()
            {
                KeySeparator = ":",
                OnDistributedLockNotAcquired = sessionId =>
                {
                    Console.WriteLine("Session \"{0}\" could not establish distributed lock. " +
                                      "This most likely means you have to increase the " +
                                      "DistributedLockAcquireSeconds/DistributedLockTimeoutSeconds.", sessionId);
                }
            });
        }


        public PooledRedisClientManager clientManager { get; set; }
    }
}
