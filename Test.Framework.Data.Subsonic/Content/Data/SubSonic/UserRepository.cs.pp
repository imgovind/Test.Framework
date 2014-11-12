using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;
using Test.Framework.Data;
using Test.Framework.Extensions;
using $rootnamespace$.Models;

namespace $rootnamespace$.Data.Subsonic
{
    public class UserRepository : IUserRepository
    {
        public SubSonicDatabase database { get; set; }

        public UserRepository(SubSonicDatabase database)
        {
            Ensure.Argument.IsNotNull(database, "database");
            this.database = database;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return database.All<User>();
        }

        public User GetUser(string emailAddress)
        {
            return database.Single<User>(x => x.Email == emailAddress);
        }
    }
}
