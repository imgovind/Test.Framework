using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Test.Framework.Extensions;

namespace Test.Framework.Mvc
{
    public class CustomControllerFactory : IControllerFactory
    {
        #region IControllerFactory Members

        public CustomControllerFactory()
        {
            var controllerTypes = AppDomain.CurrentDomain.GetAssemblies().ToList()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IController).IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);
            controllerTypes.ForEach(t =>
            {
                var instance = (IController)Activator.CreateInstance(t);
                Container.RegisterInstance<IController>(t.Name, instance);
            });
        }

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            try
            {
                return Container.Resolve<IController>(controllerName);
            }
            catch
            {
                return null;
            }
        }

        public System.Web.SessionState.SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return System.Web.SessionState.SessionStateBehavior.Default;
        }

        public void ReleaseController(IController controller)
        {
            var disposable = controller as IDisposable;

            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        #endregion
    }
}
