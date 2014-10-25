using LightInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework
{
    public class LightInjectTypeResolver : ITypeResolver
    {
        private readonly static ServiceContainer container = new ServiceContainer();

        public object GetUnderlyingContainer()
        {
            return container;
        }

        #region Resolve
        public object Resolve(Type type)
        {
            return container.GetInstance(type);
        }

        public T Resolve<T>() where T : class
        {
            return container.GetInstance<T>();
        }

        public T Resolve<T>(Type type) where T : class
        {
            return (T)container.GetInstance(type);
        }

        public T Resolve<T>(string name) where T : class
        {
            return (T)container.GetInstance<T>(name);
        }

        public T Resolve<T>(Type type, string name) where T : class
        {
            return (T)container.GetInstance(type, name);
        }

        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return container.GetAllInstances<T>();
        }

        public IEnumerable<T> ResolveAll<T>(Type type) where T : class
        {
            return (IEnumerable<T>)container.GetAllInstances(type);
        }
        #endregion

        #region Register

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
                case ObjectLifeSpans.Singleton:
                    container.Register<I, T>(new PerContainerLifetime());
                    break;
                case ObjectLifeSpans.WebRequest:
                    container.Register<I, T>(new PerRequestLifeTime());
                    break;
                case ObjectLifeSpans.Thread:
                    container.Register<I, T>(new PerThreadLifetime());
                    break;
                case ObjectLifeSpans.Session:
                    container.Register<I, T>(new PerSessionLifetime());
                    break;
                case ObjectLifeSpans.Cached:
                    container.Register<I, T>(new PerCachedLifetime());
                    break;
                case ObjectLifeSpans.Transient:
                default:
                    container.Register<I, T>();
                    break;
            }
        }

        public void Register<I, T>(string name, ObjectLifeSpans lifeSpan)
            where I : class
            where T : class, I
        {
            switch (lifeSpan)
            {
                case ObjectLifeSpans.Singleton:
                    container.Register<I, T>(name, new PerContainerLifetime());
                    break;
                case ObjectLifeSpans.WebRequest:
                    container.Register<I, T>(name, new PerRequestLifeTime());
                    break;
                case ObjectLifeSpans.Thread:
                    container.Register<I, T>(name, new PerThreadLifetime());
                    break;
                case ObjectLifeSpans.Session:
                    container.Register<I, T>(name, new PerSessionLifetime());
                    break;
                case ObjectLifeSpans.Cached:
                    container.Register<I, T>(name, new PerCachedLifetime());
                    break;
                case ObjectLifeSpans.Transient:
                default:
                    container.Register<I, T>(name);
                    break;
            }
        }

        public void RegisterInstance<T>(T instance) where T : class
        {
            container.RegisterInstance(instance);
        }

        public void RegisterInstance<T>(string name, T instance)
            where T : class
        {
            container.RegisterInstance(typeof(T), instance, name);
        }

        public void RegisterInstance<T>(string name, T instance, ObjectLifeSpans lifeSpan)
            where T : class
        {
            switch (lifeSpan)
            {
                case ObjectLifeSpans.Singleton:
                case ObjectLifeSpans.Transient:
                case ObjectLifeSpans.WebRequest:
                case ObjectLifeSpans.Thread:
                case ObjectLifeSpans.Session:
                case ObjectLifeSpans.Cached:
                default:
                    container.RegisterInstance(typeof(T), instance, name);
                    break;
            }
        }

        public void RegisterInstance<I, T>(string name, T instance, ObjectLifeSpans lifeSpan)
            where I : class
            where T : class, I
        {
            switch (lifeSpan)
            {
                case ObjectLifeSpans.Singleton:
                case ObjectLifeSpans.Transient:
                case ObjectLifeSpans.WebRequest:
                case ObjectLifeSpans.Thread:
                case ObjectLifeSpans.Session:
                case ObjectLifeSpans.Cached:
                default:
                    container.RegisterInstance(typeof(T), instance, name);
                    break;
            }
        }

        #endregion

        public void Dispose()
        {
            if (container != null)
            {
                container.Dispose();
            }
        }
    }
}
