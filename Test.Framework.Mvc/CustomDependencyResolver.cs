using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Test.Framework.Mvc
{
    public class CustomDependencyResolver : IDependencyResolver
    {
        public object GetService(Type serviceType)
        {
            object instance = Container.Resolve(serviceType);
            if (instance == null && !serviceType.IsAbstract)
            {
                Container.RegisterInstance(Activator.CreateInstance(serviceType));
                instance = Container.Resolve(serviceType);
            }
            return instance;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Container.ResolveAll<object>(serviceType);
        }
    }
}
