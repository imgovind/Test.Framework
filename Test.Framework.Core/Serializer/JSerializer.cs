using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework
{
    public static class JSerializer
    {
        public static string Serialize<T>(T instance)
        {
            if (instance != null)
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(instance.GetType());
                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, instance);
                    return Encoding.Default.GetString(ms.ToArray());
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static T Deserialize<T>(string json)
        {
            T instance = Activator.CreateInstance<T>();
            using (var memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(instance.GetType());
                instance = (T)serializer.ReadObject(memoryStream);
            }
            return instance;
        }
    }
}
