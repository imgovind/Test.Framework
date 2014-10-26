using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework
{
    public class WebConfiguration : IWebConfiguration
    {
        #region IWebConfiguration Members

        public NameValueCollection AppSettings
        {
            get
            {
                return ConfigurationManager.AppSettings;
            }
        }

        public string ConnectionStrings(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public IEnumerable<string> GetConnectionStringNames()
        {
            var connectionStrings = ConfigurationManager.ConnectionStrings;
            foreach (ConnectionStringSettings item in connectionStrings)
            {
                if (item.Name.ToLower().Contains("connectionstring")) yield return item.Name;
            }
        }

        public object GetSection(string sectionName)
        {
            return ConfigurationManager.GetSection(sectionName);
        }

        public T GetSection<T>(string sectionName)
        {
            return (T)ConfigurationManager.GetSection(sectionName);
        }

        public IEnumerable<string> GetCacheStringNames()
        {
            var cacheStrings = ConfigurationManager.GetSection("cacheStrings");
            if (cacheStrings == null) return null;
            return new List<string>();
        }

        #endregion
    }
}
