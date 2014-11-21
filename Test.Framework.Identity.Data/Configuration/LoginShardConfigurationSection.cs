using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Identity.Data
{
    public class LoginShardConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("loginShards", IsDefaultCollection = true)]
        public LoginShardElementCollection LoginShards
        {
            get { return (LoginShardElementCollection)this["loginShards"]; }
            set { this["loginShards"] = value; }
        }

        [ConfigurationProperty("dbShards", IsDefaultCollection = true)]
        public LoginDbShardElementCollection DbShards
        {
            get { return (LoginDbShardElementCollection)this["dbShards"]; }
            set { this["dbShards"] = value; }
        }
    }
}
