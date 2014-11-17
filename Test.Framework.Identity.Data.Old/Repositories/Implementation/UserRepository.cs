using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Data;
using Test.Framework.Extensions;
using Test.Framework.Identity.Entity;
using Test.Framework.Identity.Model;
using Test.Framework.Identity.Enum;
using Dapper;

namespace Test.Framework.Identity.Data
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        #region Constructor

        public UserRepository(IDatabase database)
            : base(database)
        {
        }

        public UserRepository(string connectionName)
            : base(connectionName)
        {
        }

        #endregion

        public string GetUserName(Guid userId)
        {
            string result = string.Empty;
            string commandText = "Select UserName from users where Id = @id";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("id", userId);

            using (var connection = this.Database.Connection)
            {
                result = connection.Query<string>(connection.CreateDapperCommand(new SqlDbCommand(commandText, parameters))).FirstOrDefault();
            }

            return result;
        }

        public async Task<string> GetUserNameAsync(Guid userId)
        {
            string result = string.Empty;
            string commandText = "Select UserName from users where Id = @id";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("id", userId);

            using (var connection = this.Database.Connection)
            {
                var tempResult = await connection.QueryAsync<string>(connection.CreateDapperCommand(new SqlDbCommand(commandText, parameters)));
                result = tempResult.FirstOrDefault();
            }

            return result;
        }

        public string GetUserId(string userName)
        {
            string result = string.Empty;
            string commandText = "Select Id from users where UserName = @name";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("name", userName);

            using (var connection = this.Database.Connection)
            {
                result = connection.Query<string>(connection.CreateDapperCommand(new SqlDbCommand(commandText, parameters))).FirstOrDefault();
            }

            return result;
        }

        public async Task<string> GetUserIdAsync(string userName)
        {
            string result = string.Empty;
            string commandText = "Select Id from users where UserName = @name";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("name", userName);

            using (var connection = this.Database.Connection)
            {
                var tempResult = await connection.QueryAsync<string>(connection.CreateDapperCommand(new SqlDbCommand(commandText, parameters)));
                result = tempResult.FirstOrDefault();
            }

            return result;
        }

        public User GetUserById(Guid userId)
        {
            string commandText = "Select * from users where Id = @id";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("id", userId);
            var user = this.Database.Select<User>(commandText, parameters).FirstOrDefault();

            if (user == null ||
                user.UserName.IsNullOrEmpty())
                return null;

            return user;
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            string commandText = "Select * from users where Id = @id";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("id", userId);
            var result = await this.Database.SelectAsync<User>(commandText, parameters);
            var user = result.FirstOrDefault();

            if (user == null ||
                user.UserName.IsNullOrEmpty())
                return null;

            return user;
        }

        public IEnumerable<User> GetUserByName(string userName)
        {
            string commandText = "Select * from users where UserName = @name";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("name", userName);
            var users = this.Database.Select<User>(commandText, parameters);

            if (users.IsNullOrEmpty())
                return null;

            return users;
        }

        public async Task<IEnumerable<User>> GetUserByNameAsync(string userName)
        {
            string commandText = "Select * from users where UserName = @name";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("name", userName);
            var result = await this.Database.SelectAsync<User>(commandText, parameters);
            var users = result;

            if (users.IsNullOrEmpty())
                return null;

            return users;
        }

        public IEnumerable<User> GetUserByEmail(string email)
        {
            string commandText = "Select * from users where Email = @email";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("email", email);
            var users = this.Database.Select<User>(commandText, parameters);

            if (users.IsNullOrEmpty())
                return null;

            return users; ;
        }

        public async Task<IEnumerable<User>> GetUserByEmailAsync(string email)
        {
            string commandText = "Select * from users where Email = @email";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("email", email);
            var result = await this.Database.SelectAsync<User>(commandText, parameters);
            var users = result;

            if (users.IsNullOrEmpty())
                return null;

            return users; ;
        }

        public string GetPasswordHash(Guid userId)
        {
            string result = string.Empty;
            string commandText = "Select PasswordHash from users where Id = @id";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("id", userId);

            using (var connection = this.Database.Connection)
            {
                result = connection.Query<string>(connection.CreateDapperCommand(new SqlDbCommand(commandText, parameters))).FirstOrDefault();
            }

            return result; ;
        }

        public async Task<string> GetPasswordHashAsync(Guid userId)
        {
            string result = string.Empty;
            string commandText = "Select PasswordHash from users where Id = @id";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("id", userId);

            using (var connection = this.Database.Connection)
            {
                var tempResult = await connection.QueryAsync<string>(connection.CreateDapperCommand(new SqlDbCommand(commandText, parameters)));
                result = tempResult.FirstOrDefault();
            }

            return result; ;
        }

        public bool SetPasswordHash(Guid userId, string passwordHash)
        {
            string commandText = "Update users set PasswordHash = @pwdHash where Id = @id";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("id", userId);
            parameters.AddParameter("pwdHash", passwordHash);

            return this.Database.Execute(commandText, parameters);
        }

        public async Task<bool> SetPasswordHashAsync(Guid userId, string passwordHash)
        {
            string commandText = "Update users set PasswordHash = @pwdHash where Id = @id";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("id", userId);
            parameters.AddParameter("pwdHash", passwordHash);

            var result = await this.Database.ExecuteAsync(commandText, parameters);
            return result;
        }

        public string GetSecurityStamp(Guid userId)
        {
            string result = string.Empty;
            string commandText = "Select SecurityStamp from users where Id = @id";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("id", userId);

            using (var connection = this.Database.Connection)
            {
                result = connection.Query<string>(connection.CreateDapperCommand(new SqlDbCommand(commandText, parameters))).FirstOrDefault();
            }

            return result;
        }

        public async Task<string> GetSecurityStampAsync(Guid userId)
        {
            string result = string.Empty;
            string commandText = "Select SecurityStamp from users where Id = @id";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("id", userId);

            using (var connection = this.Database.Connection)
            {
                var tempResult = await connection.QueryAsync<string>(connection.CreateDapperCommand(new SqlDbCommand(commandText, parameters)));
                result = tempResult.FirstOrDefault();
            }

            return result;
        }

        public bool Insert(User user)
        {
            return this.Database.Insert<User>(user);
        }

        public async Task<bool> InsertAsync(User user)
        {
            var result = await this.Database.InsertAsync<User>(user);
            return result;
        }

        public bool Delete(Guid userId)
        {
            string commandText = "Delete from users where Id = @userId";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("userId", userId);

            return this.Database.Execute(commandText, parameters);
        }

        public async Task<bool> DeleteAsync(Guid userId)
        {
            string commandText = "Delete from users where Id = @userId";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("userId", userId);

            var result = await this.Database.ExecuteAsync(commandText, parameters);
            return result;
        }

        public bool Delete(User user)
        {
            return this.Database.Delete<User>(user);
        }

        public async Task<bool> DeleteAsync(User user)
        {
            var result = await this.Database.DeleteAsync<User>(user);
            return result;
        }

        public bool Update(User user)
        {
            return this.Database.Update<User>(user);
        }

        public async Task<bool> UpdateAsync(User user)
        {
            var result = await this.Database.UpdateAsync<User>(user);
            return result;
        }
    }
}
