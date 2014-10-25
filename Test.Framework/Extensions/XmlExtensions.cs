using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Test.Framework.Extensions
{
    /// <summary>
    /// Extension methods for classes in the <see cref="System.Xml"/> namespace.
    /// </summary>
    public static class XmlExtensions
    {
        /// <summary>
        /// Sets the default xml namespace of every element in the given xml element
        /// </summary>
        public static void SetDefaultXmlNamespace(this XElement xelem, XNamespace xmlns)
        {
            Ensure.Argument.IsNotNull(xelem, "xelem");
            Ensure.Argument.IsNotNull(xmlns, "xmlns");

            if (xelem.Name.NamespaceName == string.Empty)
                xelem.Name = xmlns + xelem.Name.LocalName;

            foreach (var e in xelem.Elements())
                e.SetDefaultXmlNamespace(xmlns);
        }

        public static string ToXml<T>(this T instance)
        {
            if (instance == null)
                return string.Empty;

            return XSerializer.Serialize(instance);
        }

        public static string ToXml<T>(this T instance, bool omitXmlDeclaration)
        {
            if (instance == null)
                return string.Empty;

            return XSerializer.Serialize(instance, omitXmlDeclaration);
        }

        public static T ToObject<T>(this string text)
        {
            if (text.IsNullOrEmpty())
                return default(T);
            return XSerializer.Deserialize<T>(text);
        }

        public static string SanitizeXmlString(this string xml)
        {
            if (xml.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var buffer = new StringBuilder(xml.Length);

            foreach (char c in xml)
            {
                if (IsLegalXmlChar(c))
                {
                    buffer.Append(c);
                }
                else
                {
                    buffer.Append(" ");
                }
            }
            return buffer.ToString();
        }

        public static bool IsLegalXmlChar(this int character)
        {
            return
            (
                character == 0x9 ||
                character == 0xA ||
                character == 0xD ||
               (character >= 0x20 && character <= 0xD7FF) ||
               (character >= 0xE000 && character <= 0xFFFD) ||
               (character >= 0x10000 && character <= 0x10FFFF)
            );
        }
    }
}
