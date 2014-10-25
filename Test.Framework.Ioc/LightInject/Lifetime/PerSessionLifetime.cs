using LightInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Log;
using Test.Framework.Session;

namespace Test.Framework.Ioc
{
    public class PerSessionLifetime : ILifetime
    {
        public ISessionStore session { get; set; }
        public ILogger log { get; set; }

        public object GetInstance(Func<object> createInstance, Scope scope)
        {
            var instanceName = createInstance().GetType().FullName;
            string instanceKey = GetSessionKey(instanceName);
            return TryGetInstance(instanceKey, createInstance);
        }

        private object TryGetInstance(string instanceKey, Func<object> createInstance)
        {
            try
            {
                object instance = createInstance();

                if (!session.Contains(instanceKey))
                    SetInstance(instanceKey, createInstance);

                var sessionInstance = session.Get<object>(instanceKey);

                if (sessionInstance != null)
                    instance = sessionInstance;

                return instance;
            }
            catch (Exception ex)
            {
                log.Error("Extensibility - Light Inject - PerSessionLifeTime - Start of Error \n\n\n" + ex.Message);
                log.Error(ex.StackTrace + "\n\n\n Extensibility - Light Inject - PerSessionLifeTime - End of Error");
                return createInstance();
            }
        }

        private void SetInstance(string instanceKey, Func<object> createInstance)
        {
            session.Add<object>(instanceKey, createInstance());
        }

        public string GetSessionKey(string instanceName)
        {
            return string.Format(FrameworkSettings.ExtensibilityKey, instanceName, "SESSION");
        }
    }
}
