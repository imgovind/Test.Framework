using LightInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Cache;
using Test.Framework.Log;

namespace Test.Framework
{
    public class PerCachedLifetime : ILifetime
    {
        public ICacher cache { get; set; }
        public ILogger log { get; set; }

        public object GetInstance(Func<object> createInstance, Scope currentScope)
        {
            var instanceName = createInstance().GetType().FullName;
            string instanceKey = GetCacheKey(instanceName);
            return TryGetInstance(instanceKey, createInstance);
        }

        private object TryGetInstance(string instanceKey, Func<object> createInstance)
        {
            try
            {
                object instance = createInstance();

                if (!cache.CachedKeys.Contains(instanceKey))
                    SetInstance(instanceKey, createInstance);

                if (!cache.TryGet<object>(instanceKey, out instance))
                    instance = createInstance();

                return instance;
            }
            catch (Exception ex)
            {
                log.Error("Extensibility - Light Inject - PerCachedLifeTime - Start of Error \n\n\n" + ex.Message);
                log.Error(ex.StackTrace + "\n\n\n Extensibility - Light Inject - PerCachedLifeTime - End of Error");
                return createInstance();
            }
        }

        private void SetInstance(string instanceKey, Func<object> createInstance)
        {
            cache.Set<object>(instanceKey, createInstance());
        }

        public string GetCacheKey(string instanceName)
        {
            return string.Format(FrameworkSettings.ExtensibilityKey, instanceName, "CACHE");
        }
    }
}
