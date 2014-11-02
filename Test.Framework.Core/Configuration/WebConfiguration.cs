using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Extensions;

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

        public Dictionary<string, string> AppSettingsSection(string section)
        {
            return AppSettings.Cast<string>()
                            .Where(key =>
                                key.Contains(":") &&
                                key.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries).Length > 1 &&
                                key.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0].ToLowerInvariant().Equals(section)
                            )
                            .Select(x => new { Key = x, Value = AppSettings[x] })
                            .ToDictionary(x => x.Key, x => x.Value);
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

        public IEnumerable<string> GetConnectionStringNames(string dbType)
        {
            var connectionStrings = ConfigurationManager.ConnectionStrings;
            foreach (ConnectionStringSettings connectionString in connectionStrings)
            {
                if(connectionString.Name.IsNullOrEmpty())
                    continue;

                if(!connectionString.Name.ToLower().Contains("connectionstring"))
                    continue;

                var nameArray = connectionString.Name.Split(new string[]{":"}, StringSplitOptions.RemoveEmptyEntries);

                if (nameArray.IsNullOrEmpty())
                    continue;

                if (nameArray.Length < 2)
                    continue;

                if (nameArray[0].ToLowerInvariant().Equals(dbType.ToLowerInvariant()))
                    yield return connectionString.Name;
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
