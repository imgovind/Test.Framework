using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework
{
    public interface IWebConfiguration
    {
        NameValueCollection AppSettings
        {
            get;
        }

        Dictionary<string, string> AppSettingsSection(string Section);

        string ConnectionStrings(string name);
        object GetSection(string sectionName);
        T GetSection<T>(string sectionName);

        IEnumerable<string> GetConnectionStringNames();
        IEnumerable<string> GetConnectionStringNames(string dbType);
        IEnumerable<string> GetCacheStringNames();
    }
}
