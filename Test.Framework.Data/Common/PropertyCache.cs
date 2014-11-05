using System;
using System.Reflection;
using System.Collections.Generic;

namespace Test.Framework.Data
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

        public static void Register<T>()
        {
            Register(typeof(T));
        }

        public static void Register(System.Type type)
        {
            Initialize();
            if (!register.ContainsKey(type.Name))
            {
                register[type.Name] = type.GetProperties();
            }
        }

        public static PropertyInfo[] SafeResolve<T>()
        {
            return SafeResolve(typeof(T));
        }

        public static PropertyInfo[] SafeResolve(Type type)
        {
            if (register == null)
                Register(type);

            if (register.ContainsKey(type.Name))
                return Resolve(type);

            Register(type);

            return Resolve(type);
        }

        public static PropertyInfo[] Resolve<T>()
        {
            return Resolve(typeof(T));
        }

        public static PropertyInfo[] Resolve(Type type)
        {
            return register != null && register.ContainsKey(type.Name) ? register[type.Name] : SafeResolve(type);
        }
    }
}
