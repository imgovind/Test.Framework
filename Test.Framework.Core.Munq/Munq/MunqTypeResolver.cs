using Munq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework
{
    public class MunqTypeResolver : ITypeResolver
    {
        public readonly static IocContainer container = new IocContainer();

        public object GetUnderlyingContainer()
        {
            return container;
        }

        #region Register Methods

        public void Register<I, T>()
            where I : class
            where T : class, I
        {
            Register<I, T>(ObjectLifeSpans.Transient);
        }

        public void Register<I, T>(string name)
            where I : class
            where T : class, I
        {
            Register<I, T>(name, ObjectLifeSpans.Transient);
        }

        public void Register<I, T>(ObjectLifeSpans lifeSpan)
            where I : class
            where T : class, I
        {
            switch (lifeSpan)
            {
                case ObjectLifeSpans.Thread:
                    container.Register<I, T>().AsThreadSingleton();
                    break;
                case ObjectLifeSpans.WebRequest:
                    container.Register<I, T>().AsRequestSingleton();
                    break;
                case ObjectLifeSpans.Singleton:
                    container.Register<I, T>().AsContainerSingleton();
                    break;
                case ObjectLifeSpans.Session:
                    container.Register<I, T>().AsSessionSingleton();
                    break;
                case ObjectLifeSpans.Cached:
                    container.Register<I, T>().AsCached();
                    break;
                default:
                    container.Register<I, T>().AsAlwaysNew();
                    break;
            }
        }

        public void Register<I, T>(string name, ObjectLifeSpans lifeSpan)
            where I : class
            where T : class, I
        {
            switch (lifeSpan)
            {
                case ObjectLifeSpans.Thread:
                    container.Register<I, T>(name).AsThreadSingleton();
                    break;
                case ObjectLifeSpans.WebRequest:
                    container.Register<I, T>(name).AsRequestSingleton();
                    break;
                case ObjectLifeSpans.Singleton:
                    container.Register<I, T>(name).AsContainerSingleton();
                    break;
                case ObjectLifeSpans.Session:
                    container.Register<I, T>(name).AsSessionSingleton();
                    break;
                case ObjectLifeSpans.Cached:
                    container.Register<I, T>(name).AsCached();
                    break;
                default:
                    container.Register<I, T>(name).AsAlwaysNew();
                    break;
            }
        }
        public void RegisterInstance<T>(T instance) where T : class
        {
            container.RegisterInstance<T>(instance);
        }

        public void RegisterInstance<T>(string name, T instance)
            where T : class
        {
            container.RegisterInstance(name, typeof(T), instance).AsAlwaysNew();
        }

        public void RegisterInstance<T>(string name, T instance, ObjectLifeSpans lifeSpan)
            where T : class
        {
            switch (lifeSpan)
            {
                case ObjectLifeSpans.Thread:
                    container.RegisterInstance(name, typeof(T), instance).AsThreadSingleton();
                    break;
                case ObjectLifeSpans.WebRequest:
                    container.RegisterInstance(name, typeof(T), instance).AsRequestSingleton();
                    break;
                case ObjectLifeSpans.Singleton:
                    container.RegisterInstance(name, typeof(T), instance).AsContainerSingleton();
                    break;
                case ObjectLifeSpans.Session:
                    container.RegisterInstance(name, typeof(T), instance).AsSessionSingleton();
                    break;
                case ObjectLifeSpans.Cached:
                    container.RegisterInstance(name, typeof(T), instance).AsCached();
                    break;
                default:
                    container.RegisterInstance(name, typeof(T), instance).AsAlwaysNew();
                    break;
            }
        }

        public void RegisterInstance<I, T>(T instance)
            where I : class
            where T : class, I
        {
            container.RegisterInstance(typeof(I), instance).AsAlwaysNew();
        }

        public void RegisterInstance<I, T>(string name, T instance)
            where I : class
            where T : class, I
        {
            container.RegisterInstance(name, typeof(I), instance).AsAlwaysNew();
        }

        public void RegisterInstance<I, T>(T instance, ObjectLifeSpans lifeSpan)
            where I : class
            where T : class, I
        {
            switch (lifeSpan)
            {
                case ObjectLifeSpans.Thread:
                    container.RegisterInstance(typeof(I), instance).AsThreadSingleton();
                    break;
                case ObjectLifeSpans.WebRequest:
                    container.RegisterInstance(typeof(I), instance).AsRequestSingleton();
                    break;
                case ObjectLifeSpans.Singleton:
                    container.RegisterInstance(typeof(I), instance).AsContainerSingleton();
                    break;
                case ObjectLifeSpans.Session:
                    container.RegisterInstance(typeof(I), instance).AsSessionSingleton();
                    break;
                case ObjectLifeSpans.Cached:
                    container.RegisterInstance(typeof(I), instance).AsCached();
                    break;
                default:
                    container.RegisterInstance(typeof(I), instance).AsAlwaysNew();
                    break;
            }
        }

        public void RegisterInstance<I, T>(string name, T instance, ObjectLifeSpans lifeSpan)
            where I : class
            where T : class, I
        {
            switch (lifeSpan)
            {
                case ObjectLifeSpans.Thread:
                    container.RegisterInstance(name, typeof(I), instance).AsThreadSingleton();
                    break;
                case ObjectLifeSpans.WebRequest:
                    container.RegisterInstance(name, typeof(I), instance).AsRequestSingleton();
                    break;
                case ObjectLifeSpans.Singleton:
                    container.RegisterInstance(name, typeof(I), instance).AsContainerSingleton();
                    break;
                case ObjectLifeSpans.Session:
                    container.RegisterInstance(name, typeof(I), instance).AsSessionSingleton();
                    break;
                case ObjectLifeSpans.Cached:
                    container.RegisterInstance(name, typeof(I), instance).AsCached();
                    break;
                default:
                    container.RegisterInstance(name, typeof(I), instance).AsAlwaysNew();
                    break;
            }
        }

        #endregion

        #region Resolve Methods

        public object Resolve(Type type)
        {
            return container.Resolve(type);
        }

        public T Resolve<T>() where T : class
        {
            return Resolve<T>(typeof(T));
        }

        public T Resolve<T>(Type type) where T : class
        {
            return (T)container.Resolve(type);
        }

        public T Resolve<T>(string name) where T : class
        {
            return (T)container.Resolve<T>(name);
        }

        public T Resolve<T>(Type type, string name) where T : class
        {
            return (T)container.Resolve(name, type);
        }

        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return container.ResolveAll<T>().ToList<T>();
        }

        public IEnumerable<T> ResolveAll<T>(Type type) where T : class
        {
            return (IList<T>)container.ResolveAll(type);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (container != null)
            {
                container.Dispose();
            }
        }

        #endregion
    }
}
