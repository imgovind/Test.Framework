using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $rootnamespace$.Models;

namespace $rootnamespace$.Data.Petapoco
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUser(string emailAddress);
    }
}
