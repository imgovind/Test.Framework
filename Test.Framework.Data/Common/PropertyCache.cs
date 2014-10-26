using System;
using System.Reflection;
using System.Collections.Generic;

namespace Test.Framework.DataAccess
{
    public static class PropertyCache
    {
        private static IDictionary<string, PropertyInfo[]> register;

        private static void Initialize()
        {
            if (register == null)
            {
                register = new Dictionary<string, PropertyInfo[]>();
            }
        }

        public static void Register(System.Type type)
        {
            Initialize();
            if (!register.ContainsKey(type.Name))
            {
                register[type.Name] = type.GetProperties();
            }
        }

        public static void Register<T>()
        {
            Initialize();
            if (!register.ContainsKey(typeof(T).Name))
            {
                register[typeof(T).Name] = typeof(T).GetProperties();
            }
        }

        public static PropertyInfo[] Get<T>()
        {
            return register.ContainsKey(typeof(T).Name) ? register[typeof(T).Name] : typeof(T).GetProperties();
        }
    }
}
