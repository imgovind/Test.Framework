using System;
using System.Data;
using System.Collections.Generic;

namespace Test.Framework.Data
{
    public static class CustomMapper
    {
        private static IDictionary<System.Type, object> register;

        static CustomMapper()
        {
            CustomMapper.Initialize();
        }

        private static void Initialize()
        {
            if (register == null)
            {
                register = new Dictionary<System.Type, object>();
            }
        }

        public static void Register<T>(ISelectable<T> converter)
        {
            CustomMapper.Initialize();
            register[typeof(T)] = converter;
        }

        public static ISelectable<T> Resolve<T>()
        {
            return register.ContainsKey(typeof(T)) ? register[typeof(T)] as ISelectable<T> : null;
        }
    }
}
