using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime MinDate = new DateTime(1900, 1, 1);
        private static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);

        public static bool IsValid(this DateTime target)
        {
            return (target >= MinDate) && (target <= MaxDate);
        }

        public static bool IsValid(this DateTime target, DateTime time)
        {
            return (target >= time) && (target <= MaxDate);
        }

        public static bool IsWeekend(this DateTime target)
        {
            return (target.DayOfWeek == DayOfWeek.Sunday || target.DayOfWeek == DayOfWeek.Saturday);
        }

        public static bool IsInPast(this DateTime target)
        {
            if (target.IsValid())
            {
                return DateTime.Today.Date > target.Date;
            }
            return false;
        }

        public static string ToZeroFilled(this string target)
        {
            if (target.IsNotNullOrEmpty())
            {
                return DateTime.Parse(target).ToString("MM/dd/yyyy");
            }
            return string.Empty;
        }

        public static int ToInteger(this DateTime target)
        {
            int integer = 0;
            if (target.IsValid())
            {
                integer = target.ToString("hhmmss").ToInteger();
            }
            return integer;
        }

        public static string ToZeroFilled(this DateTime target)
        {
            if (target.IsValid())
            {
                return target.ToString("MM/dd/yyyy");
            }
            return string.Empty;
        }

        public static DateTime ToDateTimeOrMin(this string target)
        {
            if (target.IsNotNullOrEmpty())
            {
                return DateTime.Parse(target);
            }
            return DateTime.MinValue;
        }

        public static string ToJavascriptFormat(this DateTime dateTime)
        {
            return dateTime.ToString("MMM d, yyyy");
        }

        public static bool HourToDateTime(this string target, ref DateTime time)
        {
            if (target.IsNotNullOrEmpty())
            {
                return DateTime.TryParse(target, out time); ;
            }
            time = DateTime.Now;
            return false;
        }

        public static bool IsBetween(this DateTime dateTime, DateTime startDate, DateTime endDate)
        {
            if (dateTime.IsValid())
            {
                return dateTime.Ticks >= startDate.Ticks && dateTime.Ticks <= endDate.Ticks;
            }
            return false;
        }

    }
}
