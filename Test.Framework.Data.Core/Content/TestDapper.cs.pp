using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;
using Test.Framework.Data;
using $rootnamespace$.Models;
using $rootnamespace$.Data;

namespace $rootnamespace$
{
    public static class TestDapper
    {
        public static void Start()
        {
            AppInitalizer.Core.Initialize();
            var config = Container.Resolve<IWebConfiguration>();
            var mysql = config.GetConnectionStringNames("mysql");
            CustomMapperRegister.Initialize();
            SqlDbConnectionRegister.Register(mysql, SqlDbmsType.MySql);
            SqlOrmRegister.Register(mysql, SqlDbmsType.MySql, OrmType.Dapper);
            SqlDbRegister.Register(mysql, SqlDbmsType.MySql);
            Container.Register<IDataProvider, DataProvider>();
            var dbProvider = Container.Resolve<IDataProvider>();
            var repo = dbProvider.UserRepository(1);
            var rUsers = repo.GetAllUsers();
            var rUser = repo.GetUser("imgovind@live.com");
            var userRepository = new UserRepository(Container.Resolve<IDatabase>("mysql:AuthenticationConnectionString"));
            var users = userRepository.GetAllUsers();
            var user = userRepository.GetUser("imgovind@live.com");
        }
    }
}
