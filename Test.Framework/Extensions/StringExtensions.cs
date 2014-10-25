using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Test.Framework
{
    /// <summary>
    /// Extensions for <see cref="System.String"/>
    /// </summary>.Extensions
    public static class StringExtensions
    {
        /// <summary>
        /// A nicer way of calling <see cref="System.String.IsNullOrEmpty(string)"/>
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>true if the value parameter is null or an empty string (""); otherwise, false.</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// A nicer way of calling the inverse of <see cref="System.String.IsNullOrEmpty(string)"/>
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>true if the value parameter is not null or an empty string (""); otherwise, false.</returns>
        public static bool IsNotNullOrEmpty(this string value)
        {
            return !value.IsNullOrEmpty();
        }

        /// <summary>
        /// A nicer way of calling <see cref="System.String.Format(string, object[])"/>
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>A copy of format in which the format items have been replaced by the string representation of the corresponding objects in args.</returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// Allows for using strings in null coalescing operations
        /// </summary>
        /// <param name="value">The string value to check</param>
        /// <returns>Null if <paramref name="value"/> is empty or the original value of <paramref name="value"/>.</returns>
        public static string NullIfEmpty(this string value)
        {
            if (value == string.Empty)
                return null;

            return value;
        }

        /// <summary>
        /// Slugifies a string
        /// </summary>
        /// <param name="value">The string value to slugify</param>
        /// <param name="maxLength">An optional maximum length of the generated slug</param>
        /// <returns>A URL safe slug representation of the input <paramref name="value"/>.</returns>
        public static string ToSlug(this string value, int? maxLength = null)
        {
            Ensure.Argument.IsNotNull(value, "value");

            // if it's already a valid slug, return it
            if (RegexUtils.SlugRegex.IsMatch(value))
                return value;

            return GenerateSlug(value, maxLength);
        }

        /// <summary>
        /// Converts a string into a slug that allows segments e.g. <example>.blog/2012/07/01/title</example>.
        /// Normally used to validate user entered slugs.
        /// </summary>
        /// <param name="value">The string value to slugify</param>
        /// <returns>A URL safe slug with segments.</returns>
        public static string ToSlugWithSegments(this string value)
        {
            Ensure.Argument.IsNotNull(value, "value");

            var segments = value.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var result = segments.Aggregate(string.Empty, (slug, segment) => slug += "/" + segment.ToSlug());
            return result.Trim('/');
        }

        /// <summary>
        /// Separates a PascalCase string
        /// </summary>
        /// <example>
        /// "ThisIsPascalCase".SeparatePascalCase(); // returns "This Is Pascal Case"
        /// </example>
        /// <param name="value">The value to split</param>
        /// <returns>The original string separated on each uppercase character.</returns>
        public static string SeparatePascalCase(this string value)
        {
            Ensure.Argument.IsNotNullOrEmpty(value, "value");
            return Regex.Replace(value, "([A-Z])", " $1").Trim();
        }

        /// <summary>
        /// Credit for this method goes to http://stackoverflow.com/questions/2920744/url-slugify-alrogithm-in-cs
        /// </summary>
        private static string GenerateSlug(string value, int? maxLength = null)
        {
            // prepare string, remove accents, lower case and convert hyphens to whitespace
            var result = RemoveAccent(value).Replace("-", " ").ToLowerInvariant();

            result = Regex.Replace(result, @"[^a-z0-9\s-]", string.Empty); // remove invalid characters
            result = Regex.Replace(result, @"\s+", " ").Trim(); // convert multiple spaces into one space

            if (maxLength.HasValue) // cut and trim
                result = result.Substring(0, result.Length <= maxLength ? result.Length : maxLength.Value).Trim();

            return Regex.Replace(result, @"\s", "-"); // replace all spaces with hyphens
        }

        /// <summary>
        /// Returns a string array containing the trimmed substrings in this <paramref name="value"/>
        /// that are delimited by the provided <paramref name="separators"/>.
        /// </summary>
        public static IEnumerable<string> SplitAndTrim(this string value, params char[] separators)
        {
            Ensure.Argument.IsNotNull(value, "source");
            return value.Trim().Split(separators, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
        }

        /// <summary>
        /// Checks if the <paramref name="source"/> contains the <paramref name="input"/> based on the provided <paramref name="comparison"/> rules.
        /// </summary>
        public static bool Contains(this string source, string input, StringComparison comparison)
        {
            return source.IndexOf(input, comparison) >= 0;
        }

        /// <summary>
        /// Limits the length of the <paramref name="source"/> to the specified <paramref name="maxLength"/>.
        /// </summary>
        public static string Limit(this string source, int maxLength, string suffix = null)
        {
            if (suffix.IsNotNullOrEmpty())
            {
                maxLength = maxLength - suffix.Length;
            }

            if (source.Length <= maxLength)
            {
                return source;
            }

            return string.Concat(source.Substring(0, maxLength).Trim(), suffix ?? string.Empty);
        }

        private static string RemoveAccent(string value)
        {
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
            return Encoding.ASCII.GetString(bytes);
        }

        /// <summary>
        /// Validates whether the provided <param name="value">string</param> is a valid slug.
        /// </summary>
        public static bool IsValidSlug(this string value)
        {
            return Match(value, RegexUtils.SlugRegex);
        }

        /// <summary>
        /// Validates whether the provided <param name="value">string</param> is a valid (absolute) URL.
        /// </summary>
        public static bool IsValidUrl(this string value) // absolute
        {
            return Match(value, RegexUtils.UrlRegex);
        }

        /// <summary>
        /// Validates whether the provided <param name="value">string</param> is a valid Email Address.
        /// </summary>
        public static bool IsValidEmail(this string value)
        {
            return Match(value, RegexUtils.EmailRegex);
        }

        /// <summary>
        /// Validates whether the provided <param name="value">string</param> is a valid IP Address.
        /// </summary>
        public static bool IsValidIPAddress(this string value)
        {
            return Match(value, RegexUtils.IPAddressRegex);
        }

        public static string ToTitleCase(this string target)
        {
            if (target.IsNotNullOrEmpty())
            {
                CultureInfo cultureInfo = CultureInfo.InvariantCulture;
                TextInfo textInfo = cultureInfo.TextInfo;

                return textInfo.ToTitleCase(target.Trim().ToLower());
            }
            return string.Empty;
        }


        public static string ToDigitsOnly(this string target)
        {
            return target.IsNotNullOrEmpty() ? RegexUtils.DigitsOnlyExpression.Replace(target, "") : string.Empty;
        }


        public static string ToUpperCase(this string target)
        {
            if (target.IsNotNullOrEmpty())
            {
                return target.Trim().ToUpper();
            }
            return string.Empty;
        }


        public static string ToPhone(this string target)
        {
            if (target.IsNotNullOrEmpty() && target.Length >= 10)
            {
                return string.Format("({0}) {1}-{2}", target.Substring(0, 3), target.Substring(3, 3), target.Substring(6, 4));
            }
            return string.Empty;
        }


        public static bool HasValue(this string target)
        {
            return !string.IsNullOrEmpty(target);
        }


        public static bool ToBoolean(this string target)
        {
            Ensure.Argument.IsNotEmpty(target, "target");
            return Convert.ToBoolean(target);
        }


        public static string ReplaceAt(this string target, int index, char newChar)
        {
            Ensure.Argument.IsNotEmpty(target, "target");
            if (target.Length < index)
            {
                return target;
            }
            var builder = new StringBuilder();
            for (var i = 0; i < target.Length; i++)
            {
                if (i == index)
                {
                    builder.Append(newChar);
                }
                else
                {
                    builder.Append(target[i]);
                }
            }
            return builder.ToString();
        }

        public static string RemoveAt(this string target, int index)
        {
            Ensure.Argument.IsNotEmpty(target, "target");
            if (target.Length < index)
            {
                return target;
            }
            var builder = new StringBuilder();
            for (var i = 0; i < target.Length; i++)
            {
                if (i == index)
                {
                }
                else
                {
                    builder.Append(target[i]);
                }
            }
            return builder.ToString();
        }


        public static bool GuidTryParse(this string s, out Guid result)
        {
            if (s == null)
                throw new ArgumentNullException("String is null in GuidTryParse()");

            try
            {
                result = new Guid(s);
                return true;
            }
            catch (FormatException)
            {
                result = Guid.Empty;
                return false;
            }
            catch (OverflowException)
            {
                result = Guid.Empty;
                return false;
            }
        }


        public static bool IsGuid(this string s)
        {
            if (s.IsNullOrEmpty())
                return false;
            try
            {
                new Guid(s);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }
        }


        public static int ToInteger(this string target)
        {
            Ensure.Argument.IsNotEmpty(target, "target");
            int i;
            if (!int.TryParse(target, out i))
                throw new InvalidCastException(target + " is an invalid integer.");

            return i;
        }


        public static ulong ToUlong(this string target)
        {
            Ensure.Argument.IsNotEmpty(target, "target");
            ulong i;
            if (!ulong.TryParse(target, out i))
                throw new InvalidCastException(target + " is an invalid ulong.");

            return i;
        }

        public static bool IsInteger(this string target)
        {
            Ensure.Argument.IsNotEmpty(target, "target");
            int i;
            return int.TryParse(target, out i);
        }


        public static bool IsEmail(this string target)
        {
            return !string.IsNullOrEmpty(target) && RegexUtils.EmailRegex.IsMatch(target.Trim());
        }


        public static bool IsPhone(this string target)
        {
            return !string.IsNullOrEmpty(target) && RegexUtils.PhoneNumberExpression.IsMatch(target);
        }


        public static bool IsSSN(this string target)
        {
            return !string.IsNullOrEmpty(target) && RegexUtils.SSNExpression.IsMatch(target);
        }


        public static bool IsValidDate(this string target)
        {
            DateTime date;
            bool result = DateTime.TryParse(target, out date);
            return result;
        }


        public static bool IsValidPHPDate(this string target)
        {
            var result = false;
            if (target.IsNotNullOrEmpty() && target.Length == 8)
            {
                DateTime date;

                result = DateTime.TryParse(string.Format("{0}-{1}-{2}", target.Substring(0, 4), target.Substring(4, 2), target.Substring(6, 2)), out date);
            }
            return result;
        }


        public static double ToDouble(this string target)
        {
            Ensure.Argument.IsNotEmpty(target, "target");
            double d;
            if (!double.TryParse(target, out d))
                throw new InvalidCastException(target + " is an invalid double.");

            return d;
        }

        public static bool IsDouble(this string target)
        {
            Ensure.Argument.IsNotEmpty(target, "target");
            double d;
            return double.TryParse(target, out d);
        }


        public static DateTime ToDateTime(this string target)
        {
            Ensure.Argument.IsNotEmpty(target, "target");
            DateTime dt;
            if (!DateTime.TryParse(target, out dt))
                throw new InvalidCastException(target + "is an invalid DateTime.");

            return dt;
        }


        public static DateTime ToDateTimePHP(this string target)
        {
            Ensure.Argument.IsNotEmpty(target, "target");
            DateTime dt;
            if (!DateTime.TryParse(string.Format("{0}-{1}-{2}", target.Substring(0, 4), target.Substring(4, 2), target.Substring(6, 2)), out dt))
                throw new InvalidCastException(target + "is an invalid DateTime.");

            return dt;
        }


        public static bool IsDate(this string target)
        {
            if (target.IsNotNullOrEmpty())
            {
                DateTime dt;
                if (DateTime.TryParse(target, out dt))
                {
                    return true;
                }
            }
            return false;
        }


        public static string ToDateTimeString(this string target)
        {
            Ensure.Argument.IsNotEmpty(target, "target");
            string date = string.Empty;
            if (target.Length == 8)
            {
                var dateTime = new DateTime(int.Parse(target.Substring(0, 4)), int.Parse(target.Substring(4, 2)), int.Parse(target.Substring(6, 2)));
                date = dateTime.ToString("MM/dd/yyyy");
            }
            return date;
        }


        public static DateTime ToMySqlDate(this string target)
        {
            Ensure.Argument.IsNotEmpty(target, "target");

            DateTime dt;
            if (!DateTime.TryParse(target, out dt))
                throw new InvalidCastException(target + "is an invalid DateTime.");

            return dt;
        }


        public static string ToOrderedDate(this string target)
        {
            Ensure.Argument.IsNotEmpty(target, "target");

            DateTime dt;
            if (!DateTime.TryParse(target, out dt))
                throw new InvalidCastException(target + "is an invalid DateTime.");

            return dt.ToString("yyyy/MM/dd");
        }


        public static string[] ToArray(this string target)
        {
            Ensure.Argument.IsNotEmpty(target, "target");
            return target.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }


        public static List<int> ToList(this string target)
        {
            Ensure.Argument.IsNotEmpty(target, "target");

            List<int> list = new List<int>();
            string[] array = target.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            array.ForEach(s => { list.Add(s.ToInteger()); });

            return list;
        }


        public static Guid ToGuid(this string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return Guid.Empty;
            }
            return new Guid(target);
        }

        public static bool IsEqual(this string original, string compare)
        {
            if (original != null && compare != null)
            {
                if (original.Trim().Equals(compare.Trim(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }


        public static string PartOfString(this string original, int startIndex, int length)
        {
            if (original != null && original.Length > length)
            {
                return original.Substring(startIndex, length);
            }
            return original;
        }


        public static string Join(this string[] values, string delimiter)
        {
            bool first = true;
            StringBuilder sb = new StringBuilder();

            foreach (string item in values)
            {
                if (item == " " || item == "")
                    continue;

                if (!first)
                {
                    sb.Append(delimiter);
                }
                else
                {
                    first = false;
                }

                sb.Append(item);
            }
            return sb.ToString();
        }


        public static string Clean(this string target)
        {
            if (target.IsNotNullOrEmpty())
            {
                return target.Replace("\n", " <br/> ").Replace("\r", "").Replace(" \"", " &#8220;").Replace("\"", "&#8221;").Replace(" '", " &#8216;").Replace("'", "&#8217;").Replace("\\", "\\\\");
            }
            return string.Empty;
        }


        public static string Unclean(this string target)
        {
            if (target.IsNotNullOrEmpty())
            {
                return target.Replace("<br/>", "\n").Replace("\r", "").Replace("&#8220;", "\"").Replace("&#8221;", "\"").Replace("&#8216;", " '").Replace("&#8217;", "'");
            }
            return string.Empty;
        }


        public static string EscapeApostrophe(this string target)
        {
            if (target.IsNotNullOrEmpty())
            {
                return target.Replace("'", "\\'");
            }
            return string.Empty;
        }


        public static bool StartsWithAny(this string input, string[] values)
        {
            bool result = false;

            foreach (string item in values)
            {
                if (input.ToLower().StartsWith(item.ToLower()))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }


        public static bool ContainsAny(this string input, string[] values)
        {
            bool result = false;

            foreach (string item in values)
            {
                if (input.ToLower().Contains(item.ToLower()))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }


        public static string TrimWithNullable(this string target)
        {
            if (target.IsNotNullOrEmpty())
            {
                return target.Trim();
            }
            return string.Empty;
        }


        public static string StripHtml(this string target)
        {
            if (target.IsNotNullOrEmpty())
            {
                var input = target.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("&nbsp;", " ").Replace("_", "");
                return RegexUtils.HtmlExpression.Replace(input, string.Empty);
            }
            return string.Empty;
        }


        public static String Pluralize(this String s)
        {
            return PluralizationService.CreateService(CultureInfo.CurrentCulture).Pluralize(s);
        }


        public static String Singularize(this String s)
        {
            return PluralizationService.CreateService(CultureInfo.CurrentCulture).Singularize(s);
        }

        private static bool Match(string value, Regex regex)
        {
            return value.IsNotNullOrEmpty() || regex.IsMatch(value);
        }

    }
}
