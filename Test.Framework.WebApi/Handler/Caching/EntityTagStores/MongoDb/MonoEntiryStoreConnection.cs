using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.WebApi
{
    public class MongoEntiryStoreConnection : IDisposable
    {
        private readonly MongoClient client;
        private readonly MongoServer server;

        private readonly MongoDatabase database;

        public MongoEntiryStoreConnection(string connectionString)
        {
            this.client = new MongoClient(connectionString);
            this.server = client.GetServer();
            this.database = server.GetDatabase("EntityTagStore");
        }

        public MongoCollection<PersistentCacheKey> DocumentStore
        {
            get
            {
                return this.database.GetCollection<PersistentCacheKey>("Keys");
            }
        }

        public MongoDatabase Database
        {
            get
            {
                return this.database;
            }
        }

        public void Dispose()
        {
            // After writing this I realised actually the c# driver takes care of everything for us. /Roysvork
        }
    }
}
