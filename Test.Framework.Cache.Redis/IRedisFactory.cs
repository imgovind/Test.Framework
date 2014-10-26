using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Cache.Redis
{
    public interface IRedisFactory
    {
        void CreateBasicClient(string readWriteHost);
        void CreateBasicClient(string name, string server, int port, string password);
        void CreatePooledClient(string readWriteHost);
        void CreatePooledClient(string name, string server, int port, string password);

        void CreateClient(string name, string server, int port = 6379, int dbValue = 0, string password = null);
        void CreateClient(string name, ConfigurationOptions options, int dbValue = 0, object asyncStateValue = null);
    }
}
