using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Identity.Data
{
    [ConfigurationCollection(typeof(LoginDbShardElement))]
    public class LoginDbShardElementCollection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new LoginDbShardElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LoginDbShardElement)element).ClusterId;
        }
    }
}
