using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Ioc
{
    public class ChildContainer : IChildContainer
    {
        public ITypeResolver GetIocContainer() { return Container.resolver; }

        public void Register<I, T>()
            where I : class
            where T : class, I
        {
            Container.resolver.Register<I, T>();
        }

        public void Register<I, T>(string name)
            where I : class
            where T : class, I
        {
            Container.resolver.Register<I, T>(name);
        }

        public void Register<I, T>(ObjectLifeSpans lifeSpan)
            where I : class
            where T : class, I
        {
            Container.resolver.Register<I, T>(lifeSpan);
        }

        public void Register<I, T>(string name, ObjectLifeSpans lifeSpan)
            where I : class
            where T : class, I
        {
            Container.resolver.Register<I, T>(name, lifeSpan);
        }


        public void RegisterAll<T>() where T : class
        {
            var type = typeof(T);
            var types = AppDomain.CurrentDomain.GetAssemblies().ToList()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);
            types.ForEach(t =>
            {
                var instance = (T)Activator.CreateInstance(t);
                RegisterInstance<T>(type.Name, instance);
            });
        }

        public void RegisterInstance<T>(T existing) where T : class
        {
            Ensure.Argument.IsNotNull(existing, "existing");

            Container.resolver.RegisterInstance<T>(existing.GetType().Name, existing, ObjectLifeSpans.Transient);
        }

        public void RegisterInstance<T>(string name, T existing) where T : class
        {
            Ensure.Argument.IsNotEmpty(name, "name");
            Ensure.Argument.IsNotNull(existing, "existing");

            Container.resolver.RegisterInstance<T>(name, existing, ObjectLifeSpans.Transient);
        }

        public void RegisterInstance<T>(string name, T existing, ObjectLifeSpans lifeSpan) where T : class
        {
            Ensure.Argument.IsNotEmpty(name, "name");
            Ensure.Argument.IsNotNull(existing, "existing");

            Container.resolver.RegisterInstance<T>(name, existing, lifeSpan);
        }

        public void RegisterInstance<I, T>(string name, T instance, ObjectLifeSpans lifeSpan)
            where I : class
            where T : class, I
        {
            Container.resolver.RegisterInstance<I, T>(name, instance, lifeSpan);
        }

        public object Resolve(Type type)
        {
            Ensure.Argument.IsNotNull(type, "type");

            return Container.resolver.Resolve(type);
        }

        public T Resolve<T>(Type type) where T : class
        {
            Ensure.Argument.IsNotNull(type, "type");

            return Container.resolver.Resolve<T>(type);
        }

        public T Resolve<T>(Type type, string name) where T : class
        {
            Ensure.Argument.IsNotNull(type, "type");
            Ensure.Argument.IsNotEmpty(name, "name");

            return Container.resolver.Resolve<T>(type, name);
        }

        public T Resolve<T>() where T : class
        {
            return Container.resolver.Resolve<T>();
        }

        public T Resolve<T>(string name) where T : class
        {
            Ensure.Argument.IsNotEmpty(name, "name");

            return Container.resolver.Resolve<T>(name);
        }

        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return Container.resolver.ResolveAll<T>();
        }

        public IEnumerable<T> ResolveAll<T>(Type type) where T : class
        {
            return Container.resolver.ResolveAll<T>(type);
        }

    }
}
