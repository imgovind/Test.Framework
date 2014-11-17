using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Extensions;

namespace Test.Framework.Identity.Data.Extensions
{
    public static class LoginShardExtensions
    {
        public static Dictionary<string, int> ToDictionary(this IEnumerable<LoginShardElement> shards)
        {
            if (shards.IsNullOrEmpty())
                return null;

            return shards.ToDictionary(x => x.StartsWith, x => x.ClusterId);
        }

        public static Dictionary<string, int> ToDictionary(this LoginShardElementCollection shards)
        {
            if (shards == null ||
                shards.Count == 0)
                return null;

            var castShards = shards.Cast<LoginShardElement>();

            if (castShards.IsNullOrEmpty())
                return null;

            return castShards.ToDictionary(x => x.StartsWith, x => x.ClusterId);
        }
    }
}
