using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Reflection.Emit;

namespace Test.Framework.DataAccess
{
    public static class CustomILOrm
    {
        public delegate T MapEntity<T>(IDataReader reader);
        private static Dictionary<Type, Delegate> CachedMappers = new Dictionary<Type, Delegate>();

        public static bool Contains(Type type)
        {
            return CachedMappers.ContainsKey(type);
        }

        public static MapEntity<T> GetMapEntity<T>(Type type)
        {
            return (MapEntity<T>)CachedMappers[type];
        }

        public static void CreateMapEntity<T>(T instance, ISet<string> columns)
        {
            Type[] methodArgs = { typeof(IDataReader) };
            PropertyInfo[] properties = PropertyCache.Get<T>();
            var module = AppDomain.CurrentDomain.GetAssemblies()[15].GetType().Module;
            DynamicMethod dynamicMethod = new DynamicMethod("MapDR", typeof(T), methodArgs, module, true);
            ILGenerator iL = dynamicMethod.GetILGenerator();

            iL.DeclareLocal(typeof(T));

            iL.Emit(OpCodes.Newobj, typeof(T).GetConstructor(Type.EmptyTypes));
            iL.Emit(OpCodes.Stloc_0);

            foreach (PropertyInfo property in properties)
            {
                if (!columns.Contains(property.Name)) continue;
                // Load the T instance, SqlDataReader parameter and the field name onto the stack
                iL.Emit(OpCodes.Ldloc_0);
                iL.Emit(OpCodes.Ldarg_0);
                iL.Emit(OpCodes.Ldstr, property.Name);

                // Push the column value onto the stack
                iL.Emit(OpCodes.Callvirt, typeof(DbDataReader).GetMethod("get_Item",new Type[] { typeof(string) }));

                // Depending on the type of the property, convert the datareader column value to the type
                switch (property.PropertyType.Name)
                {
                    case "Int16":
                        iL.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToInt16", new Type[] { typeof(object) }));
                        break;
                    case "Int32":
                        iL.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToInt32", new Type[] { typeof(object) }));
                        break;
                    case "Int64":
                        iL.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToInt64", new Type[] { typeof(object) }));
                        break;
                    case "Boolean":
                        iL.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToBoolean", new Type[] { typeof(object) }));
                        break;
                    case "String":
                        iL.Emit(OpCodes.Callvirt, typeof(string).GetMethod("ToString", new Type[] { }));
                        break;
                    case "DateTime":
                        iL.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToDateTime", new Type[] { typeof(object) }));
                        break;
                    case "Decimal":
                        iL.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToDecimal", new Type[] { typeof(object) }));
                        break;
                    case "Double":
                        iL.Emit(OpCodes.Call, typeof(Double).GetMethod("ToDouble", new Type[] { typeof(object) }));
                        break;
                    default:
                        // Don't set the field value as it's an unsupported type
                        continue;
                }

                // Set the T instances property value
                iL.Emit(OpCodes.Callvirt, typeof(T).GetMethod("set_" + property.Name, new Type[] { property.PropertyType }));
            }

            // Load the T instance onto the stack
            iL.Emit(OpCodes.Ldloc_0);

            // Return
            iL.Emit(OpCodes.Ret);

            // Cache the method so we won't have to create it again for the type T
            CachedMappers.Add(typeof(T), dynamicMethod.CreateDelegate(typeof(MapEntity<T>)));
        }

        public static void ClearCachedMapperMethods()
        {
            CachedMappers.Clear();
        }
    }
}
