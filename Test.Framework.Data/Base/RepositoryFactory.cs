using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Data
{
    public sealed class RepositoryFactory
    {
        #region Nested Class for Singleton

        class Nested
        {
            static Nested()
            {
                instance.Initialize();
            }

            internal static readonly RepositoryFactory instance = new RepositoryFactory();
        }

        #endregion

        #region Private Members

        private HashSet<string> register;

        #endregion

        #region Public Instance

        public static RepositoryFactory Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        #endregion

        #region Private Constructor / Methods

        private RepositoryFactory() { }

        private void Initialize()
        {
            if (register == null)
            {
                register = new HashSet<string>();
            }
        }

        public T Get<T>(params object[] args) where T : class
        {
            T instance = null;
            Ensure.Argument.IsNotNull(args, "args");
            Ensure.Argument.IsNotEmpty(args, "args");
            var input = args[0].ToString();
            var typeName = string.Format("{0}_{1}", typeof(T).Name, input);

            return GetInstance<T>(args, ref instance, typeName);
        }

        public T Get<I, T>(IDatabase database)
            where I : class
            where T : class, I
        {
            T instance = null;
            Ensure.Argument.IsNotNull(database, "database");
            object[] args = new object[] { database };
            var input = args[0].ToString();
            var typeName = typeof(T).Name + "_" + input;
            return GetInstance<I, T>(args, ref instance, typeName);
        }

        public T Get<I, T>(params object[] args)
            where I : class
            where T : class, I
        {
            T instance = null;
            Ensure.Argument.IsNotNull(args, "args");
            Ensure.Argument.IsNotEmpty(args, "args");
            var input = args[0].ToString();
            var typeName = string.Format("{0}_{1}", typeof(T).Name, input);
            return GetInstance<I, T>(args, ref instance, typeName);
        }

        public T Get<I, T>(string connectionString, params object[] args)
            where I : class
            where T : class, I
        {
            T instance = null;
            Ensure.Argument.IsNotNull(args, "args");
            Ensure.Argument.IsNotEmpty(args, "args");
            var input = args[0].ToString();
            var typeName = string.Format("{0}_{1}_{2}", typeof(T).Name, input, connectionString);
            return GetInstance<I, T>(args, ref instance, typeName);
        }

        public T Get<I, T, C>(string connectionString, params object[] args)
            where I : class
            where T : class, I
            where C : class
        {
            T instance = null;
            var context = Container.Resolve<C>(connectionString);
            Ensure.Argument.IsNotNull(context, "context");
            args[0] = context;
            Ensure.Argument.IsNotNull(args, "args");
            Ensure.Argument.IsNotEmpty(args, "args");
            var input = args[0].ToString();
            var typeName = string.Format("{0}_{1}_{2}", typeof(T).Name, input, connectionString);
            return GetInstance<I, T>(args, ref instance, typeName);
        }

        #endregion

        #region Private Methods
        private T GetInstance<I, T>(object[] args, ref T instance, string typeName)
            where I : class
            where T : class, I
        {
            if (register.Contains(typeName))
                return Container.Resolve<T>(typeName);

            var abstractType = typeof(T);

            var types = AppDomain.CurrentDomain.GetAssemblies().ToList()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsClass && !p.IsAbstract && abstractType.IsAssignableFrom(p));

            var concreteType = types.FirstOrDefault();

            if (concreteType == null)
                throw new InvalidOperationException(String.Format("No implementation of {0} was found", abstractType));

            instance = Activator.CreateInstance(concreteType, args) as T;

            register.Add(typeName);

            Container.RegisterInstance<T>(typeName, instance);

            return instance;
        }
        private T GetInstance<T>(object[] args, ref T instance, string typeName) where T : class
        {
            if (register.Contains(typeName))
                return Container.Resolve<T>(typeName);

            var abstractType = typeof(T);

            var types = AppDomain.CurrentDomain.GetAssemblies().ToList()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsClass && !p.IsAbstract && abstractType.IsAssignableFrom(p));

            var concreteType = types.FirstOrDefault();

            if (concreteType == null)
            {
                throw new InvalidOperationException(String.Format("No implementation of {0} was found", abstractType));
            }

            instance = Activator.CreateInstance(concreteType, args) as T;

            register.Add(typeName);

            Container.RegisterInstance<T>(typeName, instance);

            return instance;
        } 
        #endregion
    }
}
