using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework
{
    public static class EnumExtensions
    {
        public static T ToEnum<T>(this byte target, T defaultValue) where T : IComparable, IFormattable
        {
            return target.ToString().ToEnum(defaultValue);
        }

        public static T ToEnum<T>(this int target, T defaultValue) where T : IComparable, IFormattable
        {
            return target.ToString().ToEnum(defaultValue);
        }

        public static T ToEnum<T>(this string target, T defaultValue) where T : IComparable, IFormattable
        {
            T convertedValue = defaultValue;

            if (target.IsNotNullOrEmpty())
            {
                try
                {
                    convertedValue = (T)Enum.Parse(typeof(T), target.Trim(), true);
                }
                catch (ArgumentException)
                {
                }
            }

            return convertedValue;
        }

        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static string GetCustomShortDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            CustomDescriptionAttribute[] attributes =
                (CustomDescriptionAttribute[])fi.GetCustomAttributes(
                typeof(CustomDescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].ShortDescription;
            else
                return value.ToString();
        }

        public static string GetGroup(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            GroupDescriptionAttribute[] attributes =
                (GroupDescriptionAttribute[])fi.GetCustomAttributes(
                typeof(GroupDescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Group;
            else
                return value.ToString();
        }

        public static string GetCustomCategory(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            CustomDescriptionAttribute[] attributes =
                (CustomDescriptionAttribute[])fi.GetCustomAttributes(
                typeof(CustomDescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Category;
            else
                return value.ToString();
        }

        public static string GetCustomGroup(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            CustomDescriptionAttribute[] attributes =
                (CustomDescriptionAttribute[])fi.GetCustomAttributes(
                typeof(CustomDescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].StatusGroup;
            else
                return value.ToString();
        }

        public static string GetFormGroup(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            CustomDescriptionAttribute[] attributes =
                (CustomDescriptionAttribute[])fi.GetCustomAttributes(
                typeof(CustomDescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].FormGroup;
            else
                return value.ToString();
        }

        public static string GetActualDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            CustomDescriptionAttribute[] attributes =
                (CustomDescriptionAttribute[])fi.GetCustomAttributes(
                typeof(CustomDescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].ActualDescription;
            else
                return value.ToString();
        }
    }
}
