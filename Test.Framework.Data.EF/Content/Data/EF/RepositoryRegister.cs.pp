using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;
using Test.Framework.Data;

namespace $rootnamespace$.Data.EF
{
    public partial class EFRegister<TContext>
        where TContext : class
    {
        public static void RegisterRepo(IEnumerable<string> connectionStringNames)
        {
            foreach (var connectionStringName in connectionStringNames)
            {
                var instance = Container.Resolve<TContext>(connectionStringName);
                if (instance == null)
                    continue;
                RegisterRepository(instance, connectionStringName);
            }
        }

        private static void RegisterRepository(TContext instance, string connectionString)
        {
            if (typeof(TContext) == typeof(FooBarEntities))
                RegisterAuthContextRepo(instance, connectionString);
        }

        private static void RegisterAuthContextRepo(TContext instance, string connectionString)
        {
            Container.RegisterInstance<IFooRepository, FooRepository>(
                connectionString,
                RepositoryFactory.Instance.Get<IFooRepository, FooRepository>(
                connectionString,
                new object[]{
                    instance
                }),
                ObjectLifeSpans.Singleton);
        }
    }
}
