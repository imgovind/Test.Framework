using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;
using Test.Framework.Data;

namespace $rootnamespace$.Data
{
    public class DataProvider : IDataProvider
    {
		public ITestRepository TestRepository(int clusterId)
        {
            return Container.ResolveOrRegister<ITestRepository, TestRepository>(
					ConnectionConstants.Test + clusterId.ToString(),
                    RepositoryFactory.Instance.Get<ITestRepository, TestRepository>(
                    new object[] { 
                        Container.Resolve<IDatabase>(ConnectionConstants.Test + clusterId.ToString()) 
                    }));
        }

		public ITestRepository TestRepository()
        {
            return Container.ResolveOrRegister<ITestRepository, TestRepository>(
					ConnectionConstants.Test,
                    RepositoryFactory.Instance.Get<ITestRepository, TestRepository>(
                    new object[] { 
                        Container.Resolve<IDatabase>(ConnectionConstants.Test) 
                    }));
        }

		public IUserRepository UserRepository(int clusterId)
        {
            return Container.ResolveOrRegister<IUserRepository, UserRepository>(
					ConnectionConstants.User + clusterId.ToString(),
                    RepositoryFactory.Instance.Get<IUserRepository, UserRepository>(
                    new object[] { 
                        Container.Resolve<IDatabase>(ConnectionConstants.User + clusterId.ToString()) 
                    }));
        }

		public IUserRepository UserRepository()
        {
            return Container.ResolveOrRegister<IUserRepository, UserRepository>(
					ConnectionConstants.User,
                    RepositoryFactory.Instance.Get<IUserRepository, UserRepository>(
                    new object[] { 
                        Container.Resolve<IDatabase>(ConnectionConstants.User) 
                    }));
        }
    }
}
