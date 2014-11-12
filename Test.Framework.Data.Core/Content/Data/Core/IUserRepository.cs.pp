using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $rootnamespace$.Models;

namespace $rootnamespace$.Data
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
		User GetUser(string emailAddress);
        bool AddUser(User user);
        bool DeleteUser(User user);
        bool UpdateUser(User user);
        bool DeprecateUser(User user);
    }
}
