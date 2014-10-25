using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Api
{
    public class PersistentCacheKey
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public byte[] Hash { get; set; }

        public string RoutePattern { get; set; }

        public string ResourceUri { get; set; }

        public string ETag { get; set; }

        public DateTimeOffset LastModified { get; set; }
    }
}
