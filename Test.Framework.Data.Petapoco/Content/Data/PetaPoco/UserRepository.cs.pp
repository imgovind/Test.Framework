using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;
using Test.Framework.Data;
using Test.Framework.Extensions;
using $rootnamespace$.Models;

namespace $rootnamespace$.Data.Petapoco
{
    public class UserRepository : IUserRepository
    {
        public PetaPocoDatabase database { get; set; }

        public UserRepository(PetaPocoDatabase database)
        {
            Ensure.Argument.IsNotNull(database, "database");
            this.database = database;
        }

        public IEnumerable<User> GetAllUsers()
        {
            var command = DQuery<User>.Select().ToCommand();
            return database.Query<User>(command.Statement, command.Parameters.ToObjectArray());
        }

        public User GetUser(string emailAddress)
        {
			return database.Select<User>(x => x.Email == emailAddress).FirstOrDefault();
        }
    }
}
