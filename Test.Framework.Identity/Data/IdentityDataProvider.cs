using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;
using Test.Framework.Data;
using Test.Framework.Extensions;
using Test.Framework.Identity.Data;

namespace Test.Framework.Identity.Data
{
    public class IdentityDataProvider : IIdentityDataProvider
    {
		public IUserRepository UserRepository(int clusterId)
        {
            return Container.ResolveOrRegister<IUserRepository, UserRepository>(
					ConnectionConstants.Auth + clusterId.ToString(),
                    RepositoryFactory.Instance.Get<IUserRepository, UserRepository>(
                    new object[] { 
                        Container.Resolve<IDatabase>(ConnectionConstants.Auth + clusterId.ToString()) 
                    }));
        }

		public IUserRepository UserRepository()
        {
            return Container.ResolveOrRegister<IUserRepository, UserRepository>(
					ConnectionConstants.Auth,
                    RepositoryFactory.Instance.Get<IUserRepository, UserRepository>(
                    new object[] { 
                        Container.Resolve<IDatabase>(ConnectionConstants.Auth) 
                    }));
        }


        public IAuthenticationRepository AuthenticationRepository(int clusterId)
        {
            return Container.ResolveOrRegister<IAuthenticationRepository, AuthenticationRepository>(
                    ConnectionConstants.Auth + clusterId.ToString(),
                    RepositoryFactory.Instance.Get<IAuthenticationRepository, AuthenticationRepository>(
                    new object[] { 
                        Container.Resolve<IDatabase>(ConnectionConstants.Auth + clusterId.ToString()) 
                    }));
        }

        public IAuthenticationRepository AuthenticationRepository()
        {
            return Container.ResolveOrRegister<IAuthenticationRepository, AuthenticationRepository>(
                    ConnectionConstants.Auth,
                    RepositoryFactory.Instance.Get<IAuthenticationRepository, AuthenticationRepository>(
                    new object[] { 
                        Container.Resolve<IDatabase>(ConnectionConstants.Auth) 
                    }));
        }

        public IRoleRepository RoleRepository(int clusterId)
        {
            return Container.ResolveOrRegister<IRoleRepository, RoleRepository>(
                    ConnectionConstants.Auth + clusterId.ToString(),
                    RepositoryFactory.Instance.Get<IRoleRepository, RoleRepository>(
                    new object[] { 
                        Container.Resolve<IDatabase>(ConnectionConstants.Auth + clusterId.ToString()) 
                    }));
        }

        public IRoleRepository RoleRepository()
        {
            return Container.ResolveOrRegister<IRoleRepository, RoleRepository>(
                    ConnectionConstants.Auth,
                    RepositoryFactory.Instance.Get<IRoleRepository, RoleRepository>(
                    new object[] { 
                        Container.Resolve<IDatabase>(ConnectionConstants.Auth) 
                    }));
        }

        public IUserClaimsRepository UserClaimsRepository(int clusterId)
        {
            return Container.ResolveOrRegister<IUserClaimsRepository, UserClaimsRepository>(
                    ConnectionConstants.Auth + clusterId.ToString(),
                    RepositoryFactory.Instance.Get<IUserClaimsRepository, UserClaimsRepository>(
                    new object[] { 
                        Container.Resolve<IDatabase>(ConnectionConstants.Auth + clusterId.ToString()) 
                    }));
        }

        public IUserClaimsRepository UserClaimsRepository()
        {
            return Container.ResolveOrRegister<IUserClaimsRepository, UserClaimsRepository>(
                    ConnectionConstants.Auth,
                    RepositoryFactory.Instance.Get<IUserClaimsRepository, UserClaimsRepository>(
                    new object[] { 
                        Container.Resolve<IDatabase>(ConnectionConstants.Auth) 
                    }));
        }

        public IUserLoginsRepository UserLoginsRepository(int clusterId)
        {
            return Container.ResolveOrRegister<IUserLoginsRepository, UserLoginsRepository>(
                    ConnectionConstants.Auth + clusterId.ToString(),
                    RepositoryFactory.Instance.Get<IUserLoginsRepository, UserLoginsRepository>(
                    new object[] { 
                        Container.Resolve<IDatabase>(ConnectionConstants.Auth + clusterId.ToString()) 
                    }));
        }

        public IUserLoginsRepository UserLoginsRepository()
        {
            return Container.ResolveOrRegister<IUserLoginsRepository, UserLoginsRepository>(
                    ConnectionConstants.Auth,
                    RepositoryFactory.Instance.Get<IUserLoginsRepository, UserLoginsRepository>(
                    new object[] { 
                        Container.Resolve<IDatabase>(ConnectionConstants.Auth) 
                    }));
        }

        public IUserRolesRepository UserRolesRepository(int clusterId)
        {
            return Container.ResolveOrRegister<IUserRolesRepository, UserRolesRepository>(
                    ConnectionConstants.Auth + clusterId.ToString(),
                    RepositoryFactory.Instance.Get<IUserRolesRepository, UserRolesRepository>(
                    new object[] { 
                        Container.Resolve<IDatabase>(ConnectionConstants.Auth + clusterId.ToString()) 
                    }));
        }

        public IUserRolesRepository UserRolesRepository()
        {
            return Container.ResolveOrRegister<IUserRolesRepository, UserRolesRepository>(
                    ConnectionConstants.Auth,
                    RepositoryFactory.Instance.Get<IUserRolesRepository, UserRolesRepository>(
                    new object[] { 
                        Container.Resolve<IDatabase>(ConnectionConstants.Auth) 
                    }));
        }
    }
}
