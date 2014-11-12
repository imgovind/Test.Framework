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

		
        public IUserRepository UserRepository(int clusterId)
        {
            return Container.ResolveOrRegister<IUserRepository, UserRepository>(
                ConnectionConstants.User + clusterId.ToString(),
                RepositoryFactory.Instance.Get<IUserRepository, UserRepository, UserAuthContext>(
                ConnectionConstants.User + clusterId.ToString(),
                new object[] { ConnectionConstants.User + clusterId.ToString() }
                ));
        }

        public IUserRepository UserRepository()
        {
            return Container.ResolveOrRegister<IUserRepository, UserRepository>(
                ConnectionConstants.User,
                RepositoryFactory.Instance.Get<IUserRepository, UserRepository, UserAuthContext>(
                ConnectionConstants.User,
                new object[] { ConnectionConstants.User }
                ));
        }
    }
}