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
                    RepositoryFactory.Instance.Get<ITestRepository, TestRepository>(
                    new object[] { 
                        Container.Resolve<IDatabase>(ConnectionConstants.Test) 
                    }));
        }
    }
}
