using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $rootnamespace$.Data.EF
{
    public interface IDataProvider
    {
        IFooRepository FooRepository(int clusterId);
        IBarRepository BarRepository();
		IUserRepository UserRepository(int clusterId);
        IUserRepository UserRepository();
    }
}
