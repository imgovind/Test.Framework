using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $rootnamespace$.Models;

namespace $rootnamespace$.Data.EF
{
    public class UserAuthContext : DbContext
    {
        public UserAuthContext(string connectionName)
            : base(connectionName)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
