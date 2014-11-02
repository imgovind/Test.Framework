using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;
using Test.Framework.Data;

namespace $rootnamespace$.Data.EF
{
    public class EFDataProvider : IDataProvider
    {
        public IFooRepository FooRepository(int clusterId)
        {
            return Container.ResolveOrRegister<IFooRepository, FooRepository>(
                ConnectionConstants.Foo + clusterId.ToString(),
                RepositoryFactory.Instance.Get<IFooRepository, FooRepository, FooBarEntities>(
                ConnectionConstants.Foo + clusterId.ToString(),
                new object[] { }
                ));
        }

        public IBarRepository BarRepository()
        {
            return Container.ResolveOrRegister<IBarRepository, BarRepository>(
                ConnectionConstants.Bar,
                RepositoryFactory.Instance.Get<IBarRepository, BarRepository, FooBarEntities>(
                ConnectionConstants.Bar,
                new object[] { }
                ));
        }
    }
}