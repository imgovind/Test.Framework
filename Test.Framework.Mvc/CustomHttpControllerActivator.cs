using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Net.Http;

namespace Test.Framework.Mvc
{
    public class CustomHttpControllerActivator : IHttpControllerActivator
    {
        public CustomHttpControllerActivator() { }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            return Container.Resolve(controllerType) as IHttpController;
        }
    }
}
