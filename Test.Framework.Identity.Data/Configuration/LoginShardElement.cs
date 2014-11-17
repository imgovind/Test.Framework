using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Identity.Data
{
    public class LoginShardElement : ConfigurationElement
    {
        [ConfigurationProperty("startsWith", IsKey = true, IsRequired = true)]
        public string StartsWith
        {
            get { return (string)this["startsWith"]; }
            set { this["startsWith"] = value; }
        }

        [ConfigurationProperty("clusterId", IsKey = false, IsRequired = true)]
        public int ClusterId
        {
            get { return (int)this["clusterId"]; }
            set { this["clusterId"] = value; }
        }

        [ConfigurationProperty("host", IsKey = false, IsRequired = false)]
        public string Host
        {
            get { return (string)this["host"]; }
            set { this["host"] = value; }
        }
    }

}
