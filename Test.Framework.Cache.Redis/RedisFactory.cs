using ServiceStack.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Extensions;

namespace Test.Framework.Caching.Redis
{
    public sealed class RedisFactory : IRedisFactory
    {
        #region IRedisClientFactory Members

        public void CreateBasicClient(string readWriteHost)
        {
            if (readWriteHost.IsNotNullOrEmpty())
                Container.RegisterInstance<IRedisClientsManager, BasicRedisClientManager>(readWriteHost, new BasicRedisClientManager(readWriteHost), ObjectLifeSpans.Transient);
        }

        public void CreateBasicClient(string name, string server, int port, string password)
        {
            if (name.IsNotNullOrEmpty() && server.IsNotNullOrEmpty())
                Container.RegisterInstance<IRedisClientsManager, BasicRedisClientManager>(name, new BasicRedisClientManager(string.Format("{0}@{1}:{2}", password, server, port)), ObjectLifeSpans.Transient);
        }

        public void CreatePooledClient(string readWriteHost)
        {
            if (readWriteHost.IsNotNullOrEmpty())
                Container.RegisterInstance<IRedisClientsManager, PooledRedisClientManager>(readWriteHost, new PooledRedisClientManager(readWriteHost), ObjectLifeSpans.Transient);
        }

        public void CreatePooledClient(string name, string server, int port, string password)
        {
            if (name.IsNotNullOrEmpty() && server.IsNotNullOrEmpty())
                Container.RegisterInstance<IRedisClientsManager, PooledRedisClientManager>(name, new PooledRedisClientManager(string.Format("{0}@{1}:{2}", password, server, port)), ObjectLifeSpans.Transient);
        }

        public void CreateClient(string name, string endpoint, int port = 6379, int dbValue = 0, string password = null)
        {
            var options = new ConfigurationOptions();
            options.EndPoints.Add(endpoint);
            options.Password = password;
            CreateClient(name, options, dbValue);
        }

        public void CreateClient(string name, ConfigurationOptions options, int dbValue = 0, object asyncStateValue = null)
        {
            var connection = ConnectionMultiplexer.Connect(options);
            Container.RegisterInstance<IDatabase>(name, connection.GetDatabase(dbValue, asyncStateValue));
        }
        #endregion
    }
}
