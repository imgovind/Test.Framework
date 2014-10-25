using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework
{
    public static class JsonExtension
    {
        public static T FromJson<T>(this string json)
        {
            Ensure.Argument.IsNotEmpty(json, "json");

            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string ToJson<T>(this T instance)
        {
            Ensure.Argument.IsNotNull(instance, "instance");

            return JsonConvert.SerializeObject(instance);
        }

        public static string ToJson<T>(this IEnumerable<T> instance)
        {
            Ensure.Argument.IsNotEmpty(instance, "instance");

            return JsonConvert.SerializeObject(instance);
        }
    }
}
