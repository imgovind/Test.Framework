using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Identity.Data;

namespace $rootnamespace$.Data
{
    public interface IIdentityDataProvider
    {
		IUserRepository UserRepository(int clusterId);
        IUserRepository UserRepository();
        IAuthenticationRepository AuthenticationRepository(int clusterId);
        IAuthenticationRepository AuthenticationRepository();
        IRoleRepository RoleRepository(int clusterId);
        IRoleRepository RoleRepository();
        IUserClaimsRepository UserClaimsRepository(int clusterId);
        IUserClaimsRepository UserClaimsRepository();
        IUserLoginsRepository UserLoginsRepository(int clusterId);
        IUserLoginsRepository UserLoginsRepository();
        IUserRolesRepository UserRolesRepository(int clusterId);
        IUserRolesRepository UserRolesRepository();
    }
}
