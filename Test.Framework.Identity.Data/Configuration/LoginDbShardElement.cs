using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Identity.Data
{
    public class LoginDbShardElement : ConfigurationElement
    {
        [ConfigurationProperty("clusterId", IsKey = true, IsRequired = true)]
        public int ClusterId
        {
            get { return (int)this["clusterId"]; }
            set { this["clusterId"] = value; }
        }

        [ConfigurationProperty("connectionName", IsKey = false, IsRequired = true)]
        public string ConnectionName
        {
            get { return (string)this["connectionName"]; }
            set { this["connectionName"] = value; }
        }
    }
}
