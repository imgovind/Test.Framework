using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Identity.Data
{
    [ConfigurationCollection(typeof(LoginShardElement))]
    public class LoginShardElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LoginShardElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LoginShardElement)element).StartsWith;
        }
    }
}
