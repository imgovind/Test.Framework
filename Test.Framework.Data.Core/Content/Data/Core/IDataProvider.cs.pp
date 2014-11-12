using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $rootnamespace$.Data
{
    public interface IDataProvider
    {
        ITestRepository TestRepository(int clusterId);
        ITestRepository TestRepository();
		IUserRepository UserRepository(int clusterId);
        IUserRepository UserRepository();
    }
}
