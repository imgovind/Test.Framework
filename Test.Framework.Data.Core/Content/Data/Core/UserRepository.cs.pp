using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;
using Test.Framework.Data;
using Test.Framework.Extensions;
using $rootnamespace$.Models;

namespace $rootnamespace$.Data
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IDatabase database)
			: base(database)
        {
        }

		public UserRepository(string connectionName)
            : base(connectionName)
        {
        }

        public IEnumerable<User> GetAllUsers()
        {
			return this.Database.Select<User>();
        }

		public User GetUser(string emailAddress)
        {
            return this.Database.Select<User>(x => x.Email == emailAddress).FirstOrDefault();
        }

        public bool AddUser(User user)
        {
            return this.Database.Insert<User>(user, true, "Id");
        }

        public bool DeleteUser(User user)
        {
            return this.Database.Delete<User>(user);
        }

        public bool UpdateUser(User user)
        {
            return this.Database.Update<User>(user);
        }

        public bool DeprecateUser(User user)
        {
            return this.Database.Deprecate<User>(user);
        }
    }
}
