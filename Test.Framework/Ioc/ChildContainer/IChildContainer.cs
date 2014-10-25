using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework
{
    public interface IChildContainer
    {
        ITypeResolver GetIocContainer();

        void Register<I, T>()
            where I : class
            where T : class, I;


        void Register<I, T>(string name)
            where I : class
            where T : class, I;


        void Register<I, T>(ObjectLifeSpans lifeSpan)
            where I : class
            where T : class, I;


        void Register<I, T>(string name, ObjectLifeSpans lifeSpan)
            where I : class
            where T : class, I;


        void RegisterAll<T>() where T : class;



        void RegisterInstance<T>(T existing) where T : class;


        void RegisterInstance<T>(string name, T existing) where T : class;


        void RegisterInstance<T>(string name, T existing, ObjectLifeSpans lifeSpan) where T : class;


        void RegisterInstance<I, T>(string name, T instance, ObjectLifeSpans lifeSpan)
            where I : class
            where T : class, I;


        object Resolve(Type type);


        T Resolve<T>(Type type) where T : class;


        T Resolve<T>(Type type, string name) where T : class;



        T Resolve<T>() where T : class;


        T Resolve<T>(string name) where T : class;


        IEnumerable<T> ResolveAll<T>() where T : class;


        IEnumerable<T> ResolveAll<T>(Type type) where T : class;

    }
}
