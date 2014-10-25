using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework
{
    public class CacheString : ConfigurationElement
    {
        public CacheString() { }

        public CacheString(string name, string server, int port, string password)
        {
            Name = name;
            Server = server;
            Port = port;
            Password = password;
        }

        [ConfigurationProperty("name", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("server", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string Server
        {
            get { return (string)this["server"]; }
            set { this["server"] = value; }
        }

        [ConfigurationProperty("port", DefaultValue = 0, IsRequired = true, IsKey = false)]
        public int Port
        {
            get { return (int)this["port"]; }
            set { this["port"] = value; }
        }

        [ConfigurationProperty("password", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string Password
        {
            get { return (string)this["password"]; }
            set { this["password"] = value; }
        }

        [ConfigurationProperty("dbvalue", DefaultValue = 0, IsRequired = false, IsKey = false)]
        public int DbValue
        {
            get { return (int)this["dbvalue"]; }
            set { this["dbvalue"] = value; }
        }
    }
}
