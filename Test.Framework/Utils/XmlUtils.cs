using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Test.Framework
{
    public class XmlUtils
    {
        public bool WriteToFile<T>(List<T> list, string fileName) 
        {
            Ensure.Argument.IsNotEmpty(fileName, "Xml File Write : filename");
            Ensure.Argument.IsNotEmpty(list, "Xml File Write: collection");

            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));

            using (TextWriter textWriter = new StreamWriter(fileName))
            {
                try
                {
                    serializer.Serialize(textWriter, list);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<T> ReadFromFile<T>(string fileName)
        {
            Ensure.Argument.IsNotEmpty(fileName, "Xml File Read : filename");

            List<T> result = new List<T>();

            XmlSerializer deserializer = new XmlSerializer(typeof(List<T>));
            using (TextReader textReader = new StreamReader(fileName))
            {
                try
                {
                    result = (List<T>)deserializer.Deserialize(textReader);
                }
                catch (Exception)
                {
                }
            }
            return result;
        }
    }
}
