using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Ioc
{
    public static class Container
    {
        public static ITypeResolver resolver;

        public static void InitializeWith(ITypeResolver resolver)
        {
            Ensure.Argument.IsNotNull(resolver, "resolver");

            Container.resolver = resolver;
        }

        public static ITypeResolver IocContainer { get { return resolver; } }

        public static void Register<I, T>()
            where I : class
            where T : class, I
        {
            resolver.Register<I, T>();
        }

        public static void Register<I, T>(string name)
            where I : class
            where T : class, I
        {
            resolver.Register<I, T>(name);
        }

        public static void Register<I, T>(ObjectLifeSpans lifeSpan)
            where I : class
            where T : class, I
        {
            resolver.Register<I, T>(lifeSpan);
        }

        public static void Register<I, T>(string name, ObjectLifeSpans lifeSpan)
            where I : class
            where T : class, I
        {
            resolver.Register<I, T>(name, lifeSpan);
        }


        public static void RegisterAll<T>() where T : class
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

        public static void RegisterInstance<T>(T existing) where T : class
        {
            Ensure.Argument.IsNotNull(existing, "existing");

            resolver.RegisterInstance<T>(existing.GetType().Name, existing, ObjectLifeSpans.Transient);
        }

        public static void RegisterInstance<T>(string name, T existing) where T : class
        {
            Ensure.Argument.IsNotEmpty(name, "name");
            Ensure.Argument.IsNotNull(existing, "existing");

            resolver.RegisterInstance<T>(name, existing, ObjectLifeSpans.Transient);
        }

        public static void RegisterInstance<T>(string name, T existing, ObjectLifeSpans lifeSpan) where T : class
        {
            Ensure.Argument.IsNotEmpty(name, "name");
            Ensure.Argument.IsNotNull(existing, "existing");

            resolver.RegisterInstance<T>(name, existing, lifeSpan);
        }

        public static void RegisterInstance<I, T>(string name, T instance, ObjectLifeSpans lifeSpan)
            where I : class
            where T : class, I
        {
            resolver.RegisterInstance<I, T>(name, instance, lifeSpan);
        }

        public static object Resolve(Type type)
        {
            Ensure.Argument.IsNotNull(type, "type");

            return resolver.Resolve(type);
        }

        public static T Resolve<T>(Type type) where T : class
        {
            Ensure.Argument.IsNotNull(type, "type");

            return resolver.Resolve<T>(type);
        }

        public static T Resolve<T>(Type type, string name) where T : class
        {
            Ensure.Argument.IsNotNull(type, "type");
            Ensure.Argument.IsNotEmpty(name, "name");

            return resolver.Resolve<T>(type, name);
        }

        public static T Resolve<T>() where T : class
        {
            return resolver.Resolve<T>();
        }

        public static T Resolve<T>(string name) where T : class
        {
            Ensure.Argument.IsNotEmpty(name, "name");

            return resolver.Resolve<T>(name);
        }

        public static IEnumerable<T> ResolveAll<T>() where T : class
        {
            return resolver.ResolveAll<T>();
        }

        public static IEnumerable<T> ResolveAll<T>(Type type) where T : class
        {
            return resolver.ResolveAll<T>(type);
        }

        public static IChildContainer GetChildContainer()
        {
            Register<IChildContainer, ChildContainer>();
            return Resolve<IChildContainer>();
        }

        public static void Reset()
        {
            if (resolver != null)
            {
                resolver.Dispose();
            }
        }
    }
}
