using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;
using Test.Framework.Data;
using Test.Framework.Identity;
using Test.Framework.Identity.Data;
using Test.Framework.Identity.Model;
using Test.Framework.Identity.Services;
using Test.Framework.Identity.Stores;
using Test.Framework.WebApi.OAuth;

namespace $rootnamespace$
{
    public partial class AppInitializer
    {
        public static class IdentityData
        {
            public static void Initialize()
            {
                var config = Container.Resolve<IWebConfiguration>();
                var connectionStringNames = ShardLookup.Login.Instance.DbShardStore.Values.Distinct().ToList();
                SqlDbConnectionRegister.Register(connectionStringNames, SqlDbmsType.MySql);
                SqlOrmRegister.Register(connectionStringNames, SqlDbmsType.MySql, OrmType.Dapper);
                SqlDbRegister.Register(connectionStringNames, SqlDbmsType.MySql);
                Container.Register<IIdentityDataProvider, IdentityDataProvider>();

                Container.Register<IUserService<IdentityUser>, UserService<IdentityUser>>();
                Container.Register<IRoleService, RoleService>();
                Container.Register<IUserClaimsService, UserClaimsService>();
                Container.Register<IUserLoginsService, UserLoginsService>();
                Container.Register<IUserRolesService, UserRolesService>();
                Container.Register<IUserStore<IdentityUser, Guid>, UserStore<IdentityUser>>();

                Container.Register<ISigningCredentialsProvider, SigningCredentialsProvider>();
            }
        }
    }
}
