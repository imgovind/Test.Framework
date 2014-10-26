using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Test.Framework
{
    public static class XSerializer
    {
        public static string Serialize<T>(T instance, bool omitXmlDeclaration)
        {
            var result = string.Empty;

            try
            {
                if (instance != null)
                {
                    var stringBuilder = new StringBuilder();

                    XmlWriterSettings xmlSettings = new XmlWriterSettings();
                    xmlSettings.Indent = true;
                    xmlSettings.OmitXmlDeclaration = omitXmlDeclaration;
                    xmlSettings.Encoding = Encoding.UTF8;

                    XmlSerializerNamespaces nameSpaces = new XmlSerializerNamespaces();
                    nameSpaces.Add("", "");

                    XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xmlSettings);

                    XmlSerializer xmlSerializer = new XmlSerializer(instance.GetType());
                    xmlSerializer.Serialize(xmlWriter, instance, nameSpaces);
                    xmlWriter.Flush();
                    result = stringBuilder.ToString();
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
            return result;
        }

        public static string Serialize<T>(T instance)
        {
            return Serialize<T>(instance, true);
        }

        public static T Deserialize<T>(string xmlString)
        {
            T instance = default(T);
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));

                instance = (T)xmlSerializer.Deserialize(memoryStream);

                if (instance == null)
                {
                    return default(T);
                }
            }
            catch (Exception)
            {
                return instance;
            }
            return instance;
        }
    }
}
