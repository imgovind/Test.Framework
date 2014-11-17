using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Identity.Data;
using Test.Framework.Identity.Model;
using Test.Framework.Extensions;
using Test.Framework.Identity.Entity;

namespace Test.Framework.Identity.Services
{
    /// <summary>
    /// Class that represents the UserLogins table in the MySQL Database
    /// </summary>
    public class UserLoginsService : IUserLoginsService
    {
        private IIdentityDataProvider dataProvider;

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public UserLoginsService(IIdentityDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        /// <summary>
        /// Deletes a login from a user in the UserLogins table
        /// </summary>
        /// <param name="user">User to have login deleted</param>
        /// <param name="login">Login to be deleted from user</param>
        /// <returns></returns>
        public bool Delete(IdentityUser user, UserLoginInfo login)
        {
            //string commandText = "Delete from UserLogins where UserId = @userId and LoginProvider = @loginProvider and ProviderKey = @providerKey";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("UserId", user.Id);
            //parameters.Add("loginProvider", login.LoginProvider);
            //parameters.Add("providerKey", login.ProviderKey);

            //return _database.Execute(commandText, parameters);
            if (user == null ||
                user.Id.IsEmpty() ||
                login == null ||
                login.LoginProvider.IsNullOrEmpty() ||
                login.ProviderKey.IsNullOrEmpty())
                return false;

            return dataProvider.UserLoginsRepository().Delete(new UserLogin { UserId = user.Id, LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey });
        }

        public bool Delete(int clusterId, IdentityUser user, UserLoginInfo login)
        {
            //string commandText = "Delete from UserLogins where UserId = @userId and LoginProvider = @loginProvider and ProviderKey = @providerKey";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("UserId", user.Id);
            //parameters.Add("loginProvider", login.LoginProvider);
            //parameters.Add("providerKey", login.ProviderKey);

            //return _database.Execute(commandText, parameters);
            if (user == null ||
                user.Id.IsEmpty() ||
                login == null ||
                login.LoginProvider.IsNullOrEmpty() ||
                login.ProviderKey.IsNullOrEmpty())
                return false;

            return dataProvider.UserLoginsRepository(clusterId).Delete(new UserLogin { UserId = user.Id, LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey });
        }

        public async Task<bool> DeleteAsync(IdentityUser user, UserLoginInfo login)
        {
            //string commandText = "Delete from UserLogins where UserId = @userId and LoginProvider = @loginProvider and ProviderKey = @providerKey";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("UserId", user.Id);
            //parameters.Add("loginProvider", login.LoginProvider);
            //parameters.Add("providerKey", login.ProviderKey);

            //return _database.Execute(commandText, parameters);
            if (user == null ||
                user.Id.IsEmpty() ||
                login == null ||
                login.LoginProvider.IsNullOrEmpty() ||
                login.ProviderKey.IsNullOrEmpty())
                return false;

            var result = await dataProvider.UserLoginsRepository().DeleteAsync(new UserLogin { UserId = user.Id, LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey });
            return result;
        }

        public async Task<bool> DeleteAsync(int clusterId, IdentityUser user, UserLoginInfo login)
        {
            //string commandText = "Delete from UserLogins where UserId = @userId and LoginProvider = @loginProvider and ProviderKey = @providerKey";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("UserId", user.Id);
            //parameters.Add("loginProvider", login.LoginProvider);
            //parameters.Add("providerKey", login.ProviderKey);

            //return _database.Execute(commandText, parameters);
            if (user == null ||
                user.Id.IsEmpty() ||
                login == null ||
                login.LoginProvider.IsNullOrEmpty() ||
                login.ProviderKey.IsNullOrEmpty())
                return false;

            var result = await dataProvider.UserLoginsRepository(clusterId).DeleteAsync(new UserLogin { UserId = user.Id, LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey });
            return result;
        }

        /// <summary>
        /// Deletes all Logins from a user in the UserLogins table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public bool Delete(Guid userId)
        {
            //string commandText = "Delete from UserLogins where UserId = @userId";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("UserId", userId);

            //return _database.Execute(commandText, parameters);
            if (userId.IsNotEmpty())
                return false;

            return dataProvider.UserLoginsRepository().Delete(userId);
        }

        public bool Delete(int clusterId, Guid userId)
        {
            //string commandText = "Delete from UserLogins where UserId = @userId";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("UserId", userId);

            //return _database.Execute(commandText, parameters);
            if (userId.IsNotEmpty())
                return false;

            return dataProvider.UserLoginsRepository(clusterId).Delete(userId);
        }

        public async Task<bool> DeleteAsync(Guid userId)
        {
            //string commandText = "Delete from UserLogins where UserId = @userId";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("UserId", userId);

            //return _database.Execute(commandText, parameters);
            if (userId.IsNotEmpty())
                return false;

            var result = await dataProvider.UserLoginsRepository().DeleteAsync(userId);
            return result;
        }

        public async Task<bool> DeleteAsync(int clusterId, Guid userId)
        {
            //string commandText = "Delete from UserLogins where UserId = @userId";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("UserId", userId);

            //return _database.Execute(commandText, parameters);
            if (userId.IsNotEmpty())
                return false;

            var result = await dataProvider.UserLoginsRepository(clusterId).DeleteAsync(userId);
            return result;
        }

        /// <summary>
        /// Inserts a new login in the UserLogins table
        /// </summary>
        /// <param name="user">User to have new login added</param>
        /// <param name="login">Login to be added</param>
        /// <returns></returns>
        public bool Insert(IdentityUser user, UserLoginInfo login)
        {
            //string commandText = "Insert into UserLogins (LoginProvider, ProviderKey, UserId) values (@loginProvider, @providerKey, @userId)";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("loginProvider", login.LoginProvider);
            //parameters.Add("providerKey", login.ProviderKey);
            //parameters.Add("userId", user.Id);

            //return _database.Execute(commandText, parameters);

            if (user == null ||
                user.Id.IsEmpty() ||
                login == null ||
                login.LoginProvider.IsNullOrEmpty() ||
                login.ProviderKey.IsNullOrEmpty())
                return false;

            return dataProvider.UserLoginsRepository().Insert(new UserLogin { UserId = user.Id, LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey });
        }

        public bool Insert(int clusterId, IdentityUser user, UserLoginInfo login)
        {
            //string commandText = "Insert into UserLogins (LoginProvider, ProviderKey, UserId) values (@loginProvider, @providerKey, @userId)";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("loginProvider", login.LoginProvider);
            //parameters.Add("providerKey", login.ProviderKey);
            //parameters.Add("userId", user.Id);

            //return _database.Execute(commandText, parameters);

            if (user == null ||
                user.Id.IsEmpty() ||
                login == null ||
                login.LoginProvider.IsNullOrEmpty() ||
                login.ProviderKey.IsNullOrEmpty())
                return false;

            return dataProvider.UserLoginsRepository(clusterId).Insert(new UserLogin { UserId = user.Id, LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey });
        }

        public async Task<bool> InsertAsync(IdentityUser user, UserLoginInfo login)
        {
            //string commandText = "Insert into UserLogins (LoginProvider, ProviderKey, UserId) values (@loginProvider, @providerKey, @userId)";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("loginProvider", login.LoginProvider);
            //parameters.Add("providerKey", login.ProviderKey);
            //parameters.Add("userId", user.Id);

            //return _database.Execute(commandText, parameters);

            if (user == null ||
                user.Id.IsEmpty() ||
                login == null ||
                login.LoginProvider.IsNullOrEmpty() ||
                login.ProviderKey.IsNullOrEmpty())
                return false;

            var result = await dataProvider.UserLoginsRepository().InsertAsync(new UserLogin { UserId = user.Id, LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey });
            return result;
        }

        public async Task<bool> InsertAsync(int clusterId, IdentityUser user, UserLoginInfo login)
        {
            //string commandText = "Insert into UserLogins (LoginProvider, ProviderKey, UserId) values (@loginProvider, @providerKey, @userId)";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("loginProvider", login.LoginProvider);
            //parameters.Add("providerKey", login.ProviderKey);
            //parameters.Add("userId", user.Id);

            //return _database.Execute(commandText, parameters);

            if (user == null ||
                user.Id.IsEmpty() ||
                login == null ||
                login.LoginProvider.IsNullOrEmpty() ||
                login.ProviderKey.IsNullOrEmpty())
                return false;

            var result = await dataProvider.UserLoginsRepository(clusterId).InsertAsync(new UserLogin { UserId = user.Id, LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey });
            return result;
        }

        /// <summary>
        /// Return a userId given a user's login
        /// </summary>
        /// <param name="userLogin">The user's login info</param>
        /// <returns></returns>
        public Guid FindUserIdByLogin(UserLoginInfo userLogin)
        {
            //string commandText = "Select UserId from UserLogins where LoginProvider = @loginProvider and ProviderKey = @providerKey";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("loginProvider", userLogin.LoginProvider);
            //parameters.Add("providerKey", userLogin.ProviderKey);

            //return _database.GetStrValue(commandText, parameters);
            if (userLogin == null ||
                userLogin.LoginProvider.IsNullOrEmpty() ||
                userLogin.ProviderKey.IsNullOrEmpty())
                return Guid.Empty;

            return dataProvider.UserLoginsRepository().FindUserIdByLogin(userLogin.LoginProvider, userLogin.ProviderKey);
        }

        public Guid FindUserIdByLogin(int clusterId, UserLoginInfo userLogin)
        {
            //string commandText = "Select UserId from UserLogins where LoginProvider = @loginProvider and ProviderKey = @providerKey";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("loginProvider", userLogin.LoginProvider);
            //parameters.Add("providerKey", userLogin.ProviderKey);

            //return _database.GetStrValue(commandText, parameters);
            if (userLogin == null ||
                userLogin.LoginProvider.IsNullOrEmpty() ||
                userLogin.ProviderKey.IsNullOrEmpty())
                return Guid.Empty;

            return dataProvider.UserLoginsRepository(clusterId).FindUserIdByLogin(userLogin.LoginProvider, userLogin.ProviderKey);
        }

        public async Task<Guid> FindUserIdByLoginAsync(UserLoginInfo userLogin)
        {
            //string commandText = "Select UserId from UserLogins where LoginProvider = @loginProvider and ProviderKey = @providerKey";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("loginProvider", userLogin.LoginProvider);
            //parameters.Add("providerKey", userLogin.ProviderKey);

            //return _database.GetStrValue(commandText, parameters);
            if (userLogin == null ||
                userLogin.LoginProvider.IsNullOrEmpty() ||
                userLogin.ProviderKey.IsNullOrEmpty())
                return Guid.Empty;

            var result = await dataProvider.UserLoginsRepository().FindUserIdByLoginAsync(userLogin.LoginProvider, userLogin.ProviderKey);
            return result;
        }

        public async Task<Guid> FindUserIdByLoginAsync(int clusterId, UserLoginInfo userLogin)
        {
            //string commandText = "Select UserId from UserLogins where LoginProvider = @loginProvider and ProviderKey = @providerKey";
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("loginProvider", userLogin.LoginProvider);
            //parameters.Add("providerKey", userLogin.ProviderKey);

            //return _database.GetStrValue(commandText, parameters);
            if (userLogin == null ||
                userLogin.LoginProvider.IsNullOrEmpty() ||
                userLogin.ProviderKey.IsNullOrEmpty())
                return Guid.Empty;

            var result = await dataProvider.UserLoginsRepository(clusterId).FindUserIdByLoginAsync(userLogin.LoginProvider, userLogin.ProviderKey);
            return result;
        }

        /// <summary>
        /// Returns a list of user's logins
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public List<UserLoginInfo> FindByUserId(Guid userId)
        {
            List<UserLoginInfo> logins = new List<UserLoginInfo>();

            if (userId.IsNotEmpty())
                return logins;

            //string commandText = "Select * from UserLogins where UserId = @userId";
            //Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@userId", userId } };

            //var rows = _database.Query(commandText, parameters);

            var rows = dataProvider.UserLoginsRepository().FindByUserId(userId);

            foreach (var row in rows)
            {
                if (row == null ||
                    row.LoginProvider.IsNullOrEmpty() ||
                    row.ProviderKey.IsNullOrEmpty())
                    continue;

                //var login = new UserLoginInfo(row["LoginProvider"], row["ProviderKey"]);
                logins.Add(new UserLoginInfo(row.LoginProvider, row.ProviderKey));
            }

            return logins;
        }

        public List<UserLoginInfo> FindByUserId(int clusterId, Guid userId)
        {
            List<UserLoginInfo> logins = new List<UserLoginInfo>();

            if (userId.IsNotEmpty())
                return logins;

            //string commandText = "Select * from UserLogins where UserId = @userId";
            //Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@userId", userId } };

            //var rows = _database.Query(commandText, parameters);

            var rows = dataProvider.UserLoginsRepository(clusterId).FindByUserId(userId);


            foreach (var row in rows)
            {
                if (row == null ||
                    row.LoginProvider.IsNullOrEmpty() ||
                    row.ProviderKey.IsNullOrEmpty())
                    continue;

                //var login = new UserLoginInfo(row["LoginProvider"], row["ProviderKey"]);
                logins.Add(new UserLoginInfo(row.LoginProvider, row.ProviderKey));
            }

            return logins;
        }

        public async Task<List<UserLoginInfo>> FindByUserIdAsync(Guid userId)
        {
            List<UserLoginInfo> logins = new List<UserLoginInfo>();

            if (userId.IsNotEmpty())
                return logins;

            //string commandText = "Select * from UserLogins where UserId = @userId";
            //Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@userId", userId } };

            //var rows = _database.Query(commandText, parameters);

            var rows = await dataProvider.UserLoginsRepository().FindByUserIdAsync(userId);

            foreach (var row in rows)
            {
                if (row == null ||
                    row.LoginProvider.IsNullOrEmpty() ||
                    row.ProviderKey.IsNullOrEmpty())
                    continue;

                //var login = new UserLoginInfo(row["LoginProvider"], row["ProviderKey"]);
                logins.Add(new UserLoginInfo(row.LoginProvider, row.ProviderKey));
            }

            return logins;
        }

        public async Task<List<UserLoginInfo>> FindByUserIdAsync(int clusterId, Guid userId)
        {
            List<UserLoginInfo> logins = new List<UserLoginInfo>();

            if (userId.IsNotEmpty())
                return logins;

            //string commandText = "Select * from UserLogins where UserId = @userId";
            //Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@userId", userId } };

            //var rows = _database.Query(commandText, parameters);

            var rows = await dataProvider.UserLoginsRepository(clusterId).FindByUserIdAsync(userId);

            foreach (var row in rows)
            {
                if (row == null ||
                    row.LoginProvider.IsNullOrEmpty() ||
                    row.ProviderKey.IsNullOrEmpty())
                    continue;

                //var login = new UserLoginInfo(row["LoginProvider"], row["ProviderKey"]);
                logins.Add(new UserLoginInfo(row.LoginProvider, row.ProviderKey));
            }

            return logins;
        }
    }
}
