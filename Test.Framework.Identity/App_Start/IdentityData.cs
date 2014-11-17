using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;
using Test.Framework.Data;
using Test.Framework.Identity.Model;
using Test.Framework.Identity;
using Test.Framework.Identity.Data;

namespace Test.Framework.Identity.Data
{
    public partial class AppInitializer
    {
        public static class IdentityData
        {
            public static void Initialize()
            {
                var config = Container.Resolve<IWebConfiguration>();
                var mysql = config.GetConnectionStringNames("mysql");
                EntityMapperRegister.Initialize();
                SqlDbConnectionRegister.Register(mysql, SqlDbmsType.MySql);
                SqlOrmRegister.Register(mysql, SqlDbmsType.MySql, OrmType.Dapper);
                SqlDbRegister.Register(mysql, SqlDbmsType.MySql);
                Container.Register<IIdentityDataProvider, IdentityDataProvider>();

                //var dbProvider = Container.Resolve<IIdentityDataProvider>();
                //var repo = dbProvider.UserRepository(1);
                //var userRepository = new UserRepository(Container.Resolve<IDatabase>("mysql:AuthenticationConnectionString"));
            }
        }
    }
}
