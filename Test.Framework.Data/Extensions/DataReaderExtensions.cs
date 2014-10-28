using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using Test.Framework.Data;
using System.Reflection.Emit;
using System.Data.Common;

namespace Test.Framework.Extensions
{
    public static class DataReaderExtensions
    {
        internal static bool IsNullableEnum(Type type)
        {
            var enumType = Nullable.GetUnderlyingType(type);

            return enumType != null && enumType.IsEnum;
        }

        internal static object ChangeTypeTo(this object value, Type conversionType)
        {
            if (conversionType == null)
                throw new ArgumentNullException("conversionType");

            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                    return null;

                NullableConverter nullableConverter = new NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            else if (conversionType == typeof(Guid))
            {
                return new Guid(value.ToString());
            }
            else if (conversionType == typeof(Int64) && value.GetType() == typeof(int))
            {
                throw new InvalidOperationException("Can't convert an Int64 (long) to Int32(int). If you're using SQLite - this is probably due to your PK being an INTEGER, which is 64bit. You'll need to set your key to long.");
            }

            return Convert.ChangeType(value, conversionType);
        }

        public static HashSet<string> GetColumns(this IDataReader reader)
        {
            var result = new HashSet<string>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                result.Add(reader.GetName(i));
            }

            return result;
        }

        public static T Hydrate<T>(this IDataReader reader, T item)
        {
            Type objectType = typeof(T);
            PropertyInfo currentProperty;
            PropertyInfo[] properties = PropertyCache.Get<T>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string propertyName = reader.GetName(i);
                currentProperty = properties.SingleOrDefault(x => x.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));

                if (currentProperty != null && !DBNull.Value.Equals(reader.GetValue(i)))
                {
                    object readerValue = reader.GetValue(i);
                    Type valueType = readerValue.GetType();
                    if (valueType == typeof(Boolean))
                    {
                        string value = readerValue.ToString();
                        currentProperty.SetValue(item, value == "1" || value == "True", null);
                    }
                    else if (currentProperty.PropertyType == typeof(Guid))
                    {
                        currentProperty.SetValue(item, reader.GetGuid(i), null);
                    }
                    else if (IsNullableEnum(currentProperty.PropertyType))
                    {
                        var nullEnumObjectValue = Enum.ToObject(Nullable.GetUnderlyingType(currentProperty.PropertyType), readerValue);
                        currentProperty.SetValue(item, nullEnumObjectValue, null);
                    }
                    else if (currentProperty.PropertyType.IsEnum)
                    {
                        var enumValue = Enum.ToObject(currentProperty.PropertyType, readerValue);
                        currentProperty.SetValue(item, enumValue, null);
                    }
                    else
                    {
                        var valType = readerValue.GetType();
                        if (currentProperty.PropertyType.IsAssignableFrom(valueType))
                        {
                            currentProperty.SetValue(item, readerValue, null);
                        }
                        else
                        {
                            currentProperty.SetValue(item, readerValue.ChangeTypeTo(currentProperty.PropertyType), null);
                        }
                    }
                }
            }
            return item;
        }

        public static IEnumerable<T> Hydrate<T>(this IDataReader reader, ISelectable<T> traits = null)
            where T : class, new()
        {
            var customReader = new DataReader(reader);
            while (reader.Read())
            {
                T instance = default(T);
                if (traits != null)
                {
                    instance = traits.ApplySelect(customReader, reader.GetColumns());
                }
                else
                {
                    instance = new T();
                    reader.Hydrate(instance);
                }
                yield return instance;
            }
        }

        public static IEnumerable<T> Hydrate<T>(this IDataReader reader, Func<DataReader, T> readMapper = null)
            where T : class, new()
        {
            var customReader = new DataReader(reader);
            while (reader.Read())
            {
                T instance = default(T);
                if (readMapper != null)
                {
                    yield return readMapper(customReader);
                }
                else
                {
                    instance = new T();
                    reader.Hydrate(instance);
                }
                yield return instance;
            }
        }

        public static IEnumerable<T> MapToEntities<T>(this IDataReader reader)
        {
            T instance = default(T);
            PropertyInfo[] properties = PropertyCache.Get<T>();
            while (reader.Read())
            {
                instance = Activator.CreateInstance<T>();
                foreach (PropertyInfo property in properties)
                {
                    if (!object.Equals(reader[property.Name], DBNull.Value))
                    {
                        property.SetValue(instance, reader[property.Name], null);
                    }
                }
                yield return instance;
            }
        }

        public static T MapToEntity<T>(this IDataReader reader, T instance)
        {
            if (!CustomILOrm.Contains(typeof(T)))
            {
                CustomILOrm.CreateMapEntity<T>(instance, reader.GetColumns());
            }

            CustomILOrm.MapEntity<T> InvokeMapEntity = CustomILOrm.GetMapEntity<T>(typeof(T));

            return InvokeMapEntity(reader);
        }

        public static T Load2<T>(this IDataReader reader, T instance)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            PropertyDescriptor[] propArray = new PropertyDescriptor[reader.FieldCount];

            for (int i = 0; i < propArray.Length; i++)
            {
                propArray[i] = props[reader.GetName(i)];
            }

            for (int i = 0; i < propArray.Length; i++)
            {
                if (propArray[i] != null)
                {
                    object value = reader.IsDBNull(i) ? null : reader[i];
                    if (value != null && propArray[i].GetType() == typeof(Boolean))
                    {
                        propArray[i].SetValue(instance, value.ToString().IsEqual("1"));
                    }
                    else
                    {
                        propArray[i].SetValue(instance, value);
                    }
                }
            }

            return instance;
        }

        public static T Load<T>(this IDataReader reader, T instance)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            PropertyDescriptor[] propArray = new PropertyDescriptor[reader.FieldCount];

            for (int i = 0; i < propArray.Length; i++)
            {
                propArray[i] = props[reader.GetName(i)];
            }

            for (int i = 0; i < propArray.Length; i++)
            {
                object value = reader.IsDBNull(i) ? null : reader[i];
                propArray[i].SetValue(instance, value);
            }

            return instance;
        }
    }
}
