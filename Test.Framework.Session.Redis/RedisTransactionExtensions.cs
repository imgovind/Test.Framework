using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Session.Redis
{
    internal static class RedisTransactionExtensions
    {
        public static void QueueCommandMap(this IRedisTransaction transaction, Func<IRedisClient, byte[][]> command, Action<IDictionary<string, byte[]>> onSuccessCallback)
        {
            transaction.QueueCommand(command, (multiData) =>
            {
                onSuccessCallback(RedisClientExtensions.MultiByteArrayToDictionary(multiData));
            });
        }
    }
}
