using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;
using Test.Framework.Data;
using Test.Framework.Extensions;

namespace Test.Framework.Identity.Data
{
    public class IdentityDataProvider : IIdentityDataProvider
    {
		public IUserRepository UserRepository(int clusterId)
        {
            var connectionString = ShardLookup.Login.Instance.DbShardStore[clusterId];
            return Container.ResolveOrRegister<IUserRepository, UserRepository>(
					typeof(IUserRepository).Name + connectionString,
                    RepositoryFactory.Instance.Get<IUserRepository, UserRepository>(
					connectionString,
                    new object[] { 
                        Container.Resolve<IDatabase>(connectionString)
                    }));
        }

		public IUserRepository UserRepository()
        {
            var connectionString = ShardLookup.Login.Instance.DbShardStore[0];
            return Container.ResolveOrRegister<IUserRepository, UserRepository>(
					typeof(IUserRepository).Name + connectionString,
                    RepositoryFactory.Instance.Get<IUserRepository, UserRepository>(
					connectionString,
                    new object[] { 
                        Container.Resolve<IDatabase>(connectionString) 
                    }));
        }


        public IAuthenticationRepository AuthenticationRepository(int clusterId)
        {
            var connectionString = ShardLookup.Login.Instance.DbShardStore[clusterId];
            return Container.ResolveOrRegister<IAuthenticationRepository, AuthenticationRepository>(
					typeof(IAuthenticationRepository).Name + connectionString,
                    RepositoryFactory.Instance.Get<IAuthenticationRepository, AuthenticationRepository>(
					connectionString,
                    new object[] { 
                        Container.Resolve<IDatabase>(connectionString)
                    }));
        }

        public IAuthenticationRepository AuthenticationRepository()
        {
            var connectionString = ShardLookup.Login.Instance.DbShardStore[0];
            return Container.ResolveOrRegister<IAuthenticationRepository, AuthenticationRepository>(
					typeof(IAuthenticationRepository).Name + connectionString,
                    RepositoryFactory.Instance.Get<IAuthenticationRepository, AuthenticationRepository>(
					connectionString,
                    new object[] { 
                        Container.Resolve<IDatabase>(connectionString) 
                    }));
        }

        public IRoleRepository RoleRepository(int clusterId)
        {
            var connectionString = ShardLookup.Login.Instance.DbShardStore[clusterId];
            return Container.ResolveOrRegister<IRoleRepository, RoleRepository>(
                    typeof(IRoleRepository).Name + connectionString,
                    RepositoryFactory.Instance.Get<IRoleRepository, RoleRepository>(
					connectionString,
                    new object[] { 
                        Container.Resolve<IDatabase>(connectionString)
                    }));
        }

        public IRoleRepository RoleRepository()
        {
            var connectionString = ShardLookup.Login.Instance.DbShardStore[0];
            return Container.ResolveOrRegister<IRoleRepository, RoleRepository>(
                    typeof(IRoleRepository).Name + connectionString,
                    RepositoryFactory.Instance.Get<IRoleRepository, RoleRepository>(
					connectionString,
                    new object[] { 
                        Container.Resolve<IDatabase>(connectionString) 
                    }));
        }

        public IUserClaimsRepository UserClaimsRepository(int clusterId)
        {
            var connectionString = ShardLookup.Login.Instance.DbShardStore[clusterId];
            return Container.ResolveOrRegister<IUserClaimsRepository, UserClaimsRepository>(
                    typeof(IUserClaimsRepository).Name + connectionString,
                    RepositoryFactory.Instance.Get<IUserClaimsRepository, UserClaimsRepository>(
					connectionString,
                    new object[] { 
                        Container.Resolve<IDatabase>(connectionString)
                    }));
        }

        public IUserClaimsRepository UserClaimsRepository()
        {
            var connectionString = ShardLookup.Login.Instance.DbShardStore[0];
            return Container.ResolveOrRegister<IUserClaimsRepository, UserClaimsRepository>(
                    typeof(IUserClaimsRepository).Name + connectionString,
                    RepositoryFactory.Instance.Get<IUserClaimsRepository, UserClaimsRepository>(
					connectionString,
                    new object[] { 
                        Container.Resolve<IDatabase>(connectionString) 
                    }));
        }

        public IUserLoginsRepository UserLoginsRepository(int clusterId)
        {
            var connectionString = ShardLookup.Login.Instance.DbShardStore[clusterId];
            return Container.ResolveOrRegister<IUserLoginsRepository, UserLoginsRepository>(
                    typeof(IUserLoginsRepository).Name + connectionString,
                    RepositoryFactory.Instance.Get<IUserLoginsRepository, UserLoginsRepository>(
					connectionString,
                    new object[] { 
                        Container.Resolve<IDatabase>(connectionString)
                    }));
        }

        public IUserLoginsRepository UserLoginsRepository()
        {
            var connectionString = ShardLookup.Login.Instance.DbShardStore[0];
            return Container.ResolveOrRegister<IUserLoginsRepository, UserLoginsRepository>(
                    typeof(IUserLoginsRepository).Name + connectionString,
                    RepositoryFactory.Instance.Get<IUserLoginsRepository, UserLoginsRepository>(
					connectionString,
                    new object[] { 
                        Container.Resolve<IDatabase>(connectionString) 
                    }));
        }

        public IUserRolesRepository UserRolesRepository(int clusterId)
        {
            var connectionString = ShardLookup.Login.Instance.DbShardStore[clusterId];
            return Container.ResolveOrRegister<IUserRolesRepository, UserRolesRepository>(
                    typeof(IUserRolesRepository).Name + connectionString,
                    RepositoryFactory.Instance.Get<IUserRolesRepository, UserRolesRepository>(
					connectionString,
                    new object[] { 
                        Container.Resolve<IDatabase>(connectionString) 
                    }));
        }

        public IUserRolesRepository UserRolesRepository()
        {
            var connectionString = ShardLookup.Login.Instance.DbShardStore[0];
            return Container.ResolveOrRegister<IUserRolesRepository, UserRolesRepository>(
                    typeof(IUserRolesRepository).Name + connectionString,
                    RepositoryFactory.Instance.Get<IUserRolesRepository, UserRolesRepository>(
					connectionString,
                    new object[] { 
                        Container.Resolve<IDatabase>(connectionString) 
                    }));
        }
    }
}
