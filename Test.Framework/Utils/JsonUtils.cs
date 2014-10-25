using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework
{
    public class JsonUtils
    {
        public bool WriteToFile<T>(List<T> list, string fileName)
        {
            Ensure.Argument.IsNotEmpty(fileName, "Json File Write : filename");
            Ensure.Argument.IsNotEmpty(list, "Json File Write: collection");

            try
            {
                using (StreamWriter sw = new StreamWriter(fileName))
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jw, list);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T> ReadFromFile<T>(string fileName)
        {
            Ensure.Argument.IsNotEmpty(fileName, "Json File Read : filename");

            List<T> result = new List<T>();

            using (StreamReader sr = new StreamReader(fileName))
            using (JsonReader jr = new JsonTextReader(sr))
            {
                try
                {
                    var deserializer = new JsonSerializer();
                    result = (List<T>)deserializer.Deserialize(jr);
                }
                catch (Exception)
                {
                }
            }
            return result;
        }
    }
}
