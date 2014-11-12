using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;
using Test.Framework.Data;
using $rootnamespace$.Models;
using $rootnamespace$.Data.EF;

namespace $rootnamespace$
{
    public static class TestEF
    {
        public static void Start()
        {
            AppInitalizer.Core.Initialize();
            var config = Container.Resolve<IWebConfiguration>();
            var mysql = config.GetConnectionStringNames("mysql");
            EFRegister<FooBarEntities>.RegisterContext(mysql);
            EFRegister<UserAuthContext>.RegisterContext(mysql);
            EFRegister<UserAuthContext>.RegisterRepo(mysql);
            Container.Register<IDataProvider, EFDataProvider>();
            var dbProvider = Container.Resolve<IDataProvider>();
            var repo = dbProvider.UserRepository(1);
            var rUsers = repo.GetAllUsers();
            var rUser = repo.GetUser("imgovind@live.com");
        }
    }
}
