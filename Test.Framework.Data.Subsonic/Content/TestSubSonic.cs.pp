using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;
using $rootnamespace$.Models;
using $rootnamespace$.Data.Subsonic;

namespace $rootnamespace$
{
    public static class TestSubSonic
    {
        public static void Start()
        {
            AppInitalizer.Core.Initialize();
            var config = Container.Resolve<IWebConfiguration>();
            var mysql = config.GetConnectionStringNames("mysql");
            SubSonicRegister.Register(mysql);
            var userRepository = new UserRepository(Container.Resolve<SubSonicDatabase>("mysql:AuthenticationConnectionString"));
            var users = userRepository.GetAllUsers();
            var user = userRepository.GetUser("imgovind@live.com");
        }
    }
}
