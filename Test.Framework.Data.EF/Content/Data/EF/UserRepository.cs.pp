using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.Framework;
using Test.Framework.Data;
using Test.Framework.Extensions;
using System.Threading.Tasks;
using $rootnamespace$.Models;

namespace $rootnamespace$.Data.EF
{
    public class UserRepository : GenericRepository<UserAuthContext>, IUserRepository
    {
        public UserRepository(UserAuthContext context)
            : base(context)
        {
        }

        public IEnumerable<User> GetAllUsers()
        {
            return GetAll<User>().ToList();
        }

        public User GetUser(string emailAddress)
        {
            return FindBy<User>(x => x.Email == emailAddress).FirstOrDefault();
        }
    }
}
