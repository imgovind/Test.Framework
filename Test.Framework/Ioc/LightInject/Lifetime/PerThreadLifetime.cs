using LightInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Framework
{
    public class PerThreadLifetime : ILifetime
    {
        ThreadLocal<object> instances = new ThreadLocal<object>();

        public object GetInstance(Func<object> instanceFactory, Scope currentScope)
        {
            if (instances.Value == null)
            {
                object instance = instanceFactory();
                IDisposable disposable = instance as IDisposable;
                if (disposable != null)
                {
                    if (currentScope == null)
                    {
                        throw new InvalidOperationException("Attempt to create an disposable object without a current scope.");
                    }
                    currentScope.TrackInstance(disposable);
                }

                instances.Value = instance;
            }
            return instances.Value;
        }
    }
}
