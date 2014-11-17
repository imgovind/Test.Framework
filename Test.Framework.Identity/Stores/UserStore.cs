using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Identity.Data;
using Test.Framework.Extensions;
using Test.Framework.Identity.Model;
using Test.Framework.Identity.Services;
using Test.Framework;
using Test.Framework.Identity.Entity;

namespace Test.Identity.Stores
{
    public class UserStore<TUser> : IUserLoginStore<TUser, Guid>,
        IUserClaimStore<TUser, Guid>,
        IUserRoleStore<TUser, Guid>,
        IUserPasswordStore<TUser, Guid>,
        IUserSecurityStampStore<TUser, Guid>,
        IQueryableUserStore<TUser, Guid>,
        IUserEmailStore<TUser, Guid>,
        IUserPhoneNumberStore<TUser, Guid>,
        IUserTwoFactorStore<TUser, Guid>,
        IUserLockoutStore<TUser, Guid>,
        IUserStore<TUser, Guid>
        where TUser : IdentityUser
    {
        private IIdentityDataProvider dataProvider;
        private IUserService<TUser> userService;
        private IRoleService roleService;
        private IUserRolesService userRolesService;
        private IUserClaimsService userClaimsService;
        private IUserLoginsService userLoginsService;

        public IQueryable<TUser> Users
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private int GetCluster(string userName)
        {
            int result = 0;
            if (userName.IsNullOrEmpty())
                return result;

            var output1 = userName.TakeWhile(Char.IsLetter).ToArray();

            if (output1.IsNullOrEmpty())
                return result;

            var output2 = output1.ToString();

            if (output2.IsNullOrEmpty())
                return result;

            var output3 = output2.Substring(0, 1);

            if (output3.IsNullOrEmpty())
                return result;

            ShardLookup.Login.Instance.Store.TryGetValue(output3, out result);

            return result;
        }

        private int GetCluster(Guid Id)
        {
            int result = 0;
            if (Id.IsEmpty())
                return result;
            var clusters = ShardLookup.Login.Instance.Store.Values.Distinct();
            User user = null;

            foreach (var clusterId in clusters)
            {
                user = this.dataProvider.UserRepository(clusterId).GetUserById(Id);

                if (user == null)
                    continue;

                return clusterId;
            }
            return result;
        }

        private int GetCluster(TUser user)
        {
            int result = 0;

            if (user == null ||
               (user.Email.IsNullOrEmpty() && user.Id.IsEmpty()))
                return result;

            if (user.Email.IsNotNullOrEmpty())
                return GetCluster(user.Email);

            if (user.UserName.IsNotNullOrEmpty() &&
                user.UserName.IsEmail())
                return GetCluster(user.UserName);

            if(user.Id.IsNotEmpty())
                return GetCluster(user.Id);

            return result;
        }

        /// <summary>
        /// Default constructor that initializes a new MySQLDatabase
        /// instance using the Default Connection string
        /// </summary>
        public UserStore()
        {
            new UserStore<TUser>(Container.Resolve<IIdentityDataProvider>());
        }

        /// <summary>
        /// Constructor that takes a MySQLDatabase as argument 
        /// </summary>
        /// <param name="database"></param>
        public UserStore(IIdentityDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
            userService = Container.Resolve<IUserService<TUser>>();
            roleService = Container.Resolve<IRoleService>();
            userRolesService = Container.Resolve<IUserRolesService>();
            userClaimsService = Container.Resolve<IUserClaimsService>();
            userLoginsService = Container.Resolve<IUserLoginsService>();
        }

        /// <summary>
        /// Insert a new TUser in the UserTable
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task CreateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var result = await userService.InsertAsync(GetCluster(user.UserName), user);
        }

        /// <summary>
        /// Returns an TUser instance based on a userId query 
        /// </summary>
        /// <param name="userId">The user's Id</param>
        /// <returns></returns>
        public async Task<TUser> FindByIdAsync(Guid userId)
        {
            if(userId.IsEmpty())
                throw new ArgumentException("Null or empty argument: userId");

            var tempResult = await userService.GetUserByIdAsync(GetCluster(userId), userId);
            TUser result = tempResult as TUser;
            if (result != null)
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Returns an TUser instance based on a userName query 
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public async Task<TUser> FindByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Null or empty argument: userName");
            }

            var tempResult = await userService.GetUserByNameAsync(GetCluster(userName), userName);
                
            List<TUser> result = tempResult as List<TUser>;

            // Should I throw if > 1 user?
            if (result != null && result.Count == 1)
            {
                return result.FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Updates the UsersTable with the TUser instance values
        /// </summary>
        /// <param name="user">TUser to be updated</param>
        /// <returns></returns>
        public async Task UpdateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var result = await userService.UpdateAsync(GetCluster(user), user);
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// Inserts a claim to the UserClaimsTable for the given user
        /// </summary>
        /// <param name="user">User to have claim added</param>
        /// <param name="claim">Claim to be added</param>
        /// <returns></returns>
        public async Task AddClaimAsync(TUser user, Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("user");
            }

            var result = await userClaimsService.InsertAsync(GetCluster(user), claim, user.Id);
        }

        /// <summary>
        /// Returns all claims for a given user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            ClaimsIdentity identity = await userClaimsService.FindByUserIdAsync(GetCluster(user), user.Id);

            if (identity == null ||
                identity.Claims == null ||
                !identity.Claims.Any())
                return null;

            return identity.Claims.ToList();
        }

        /// <summary>
        /// Removes a claim froma user
        /// </summary>
        /// <param name="user">User to have claim removed</param>
        /// <param name="claim">Claim to be removed</param>
        /// <returns></returns>
        public async Task RemoveClaimAsync(TUser user, Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (claim == null)
            {
                throw new ArgumentNullException("claim");
            }

            var result = await userClaimsService.DeleteAsync(GetCluster(user), user, claim);
        }

        /// <summary>
        /// Inserts a Login in the UserLoginsTable for a given User
        /// </summary>
        /// <param name="user">User to have login added</param>
        /// <param name="login">Login to be added</param>
        /// <returns></returns>
        public async Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            var result = await userLoginsService.InsertAsync(GetCluster(user), user, login);
        }

        /// <summary>
        /// Returns an TUser based on the Login info
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public async Task<TUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            var userId = await userLoginsService.FindUserIdByLoginAsync(login);
            if (userId != null)
            {
                var tempResult = await userService.GetUserByIdAsync(userId);
                TUser user = tempResult as TUser;
                if (user != null)
                {
                    return user;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns list of UserLoginInfo for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            List<UserLoginInfo> userLogins = new List<UserLoginInfo>();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            List<UserLoginInfo> logins = await userLoginsService.FindByUserIdAsync(GetCluster(user), user.Id);
            if (logins != null)
            {
                return logins;
            }

            return null;
        }

        /// <summary>
        /// Deletes a login from UserLoginsTable for a given TUser
        /// </summary>
        /// <param name="user">User to have login removed</param>
        /// <param name="login">Login to be removed</param>
        /// <returns></returns>
        public async Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            var result = await userLoginsService.DeleteAsync(GetCluster(user), user, login);
        }

        /// <summary>
        /// Inserts a entry in the UserRoles table
        /// </summary>
        /// <param name="user">User to have role added</param>
        /// <param name="roleName">Name of the role to be added to user</param>
        /// <returns></returns>
        public async Task AddToRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentException("Argument cannot be null or empty: roleName.");
            }

            Guid roleId = await roleService.GetRoleIdAsync(roleName);
            if (roleId.IsNotEmpty())
            {
                var result = await userRolesService.InsertAsync(GetCluster(user), user, roleId);
            }
        }

        /// <summary>
        /// Returns the roles for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IList<string>> GetRolesAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            List<string> roles = await userRolesService.FindByUserIdAsync(GetCluster(user), user.Id);
            {
                if (roles != null)
                {
                    return roles;
                }
            }

            return null;
        }

        /// <summary>
        /// Verifies if a user is in a role
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<bool> IsInRoleAsync(TUser user, string role)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentNullException("role");
            }

            List<string> roles = await userRolesService.FindByUserIdAsync(GetCluster(user), user.Id);
            {
                if (roles != null && roles.Contains(role))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes a user from a role
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task RemoveFromRoleAsync(TUser user, string role)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task DeleteAsync(TUser user)
        {
            if (user != null)
            {
                var result = await userService.DeleteAsync(GetCluster(user), user);
            }
        }

        /// <summary>
        /// Returns the PasswordHash for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<string> GetPasswordHashAsync(TUser user)
        {
            string passwordHash = await userService.GetPasswordHashAsync(GetCluster(user), user.Id);
            return passwordHash;
        }

        /// <summary>
        /// Verifies if user has password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> HasPasswordAsync(TUser user)
        {
            var result = await userService.GetPasswordHashAsync(GetCluster(user), user.Id);
            var hasPassword = !string.IsNullOrEmpty(result);

            return hasPassword;
        }

        /// <summary>
        /// Sets the password hash for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;

            return Task.FromResult<Object>(null);
        }

        /// <summary>
        ///  Set security stamp
        /// </summary>
        /// <param name="user"></param>
        /// <param name="stamp"></param>
        /// <returns></returns>
        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            user.SecurityStamp = stamp;

            return Task.FromResult(0);

        }

        /// <summary>
        /// Get security stamp
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetSecurityStampAsync(TUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        /// <summary>
        /// Set email on user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task SetEmailAsync(TUser user, string email)
        {
            user.Email = email;
            var result = await userService.UpdateAsync(GetCluster(user), user);
        }

        /// <summary>
        /// Get email from user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetEmailAsync(TUser user)
        {
            return Task.FromResult(user.Email);
        }

        /// <summary>
        /// Get if user email is confirmed
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        /// <summary>
        /// Set when user email is confirmed
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmed"></param>
        /// <returns></returns>
        public async Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
            var result = await userService.UpdateAsync(GetCluster(user), user);
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<TUser> FindByEmailAsync(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("email");
            }

            var tempResult = await userService.GetUserByEmailAsync(GetCluster(email), email);
            TUser result = tempResult as TUser;
            if (result != null)
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Set user phone number
        /// </summary>
        /// <param name="user"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            var result = await userService.UpdateAsync(GetCluster(user), user);
        }

        /// <summary>
        /// Get user phone number
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetPhoneNumberAsync(TUser user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        /// <summary>
        /// Get if user phone number is confirmed
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        /// <summary>
        /// Set phone number if confirmed
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmed"></param>
        /// <returns></returns>
        public async Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            user.PhoneNumberConfirmed = confirmed;
            var result = await userService.UpdateAsync(GetCluster(user), user);
        }

        /// <summary>
        /// Set two factor authentication is enabled on the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public async Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;
            var result = await userService.UpdateAsync(GetCluster(user), user);
        }

        /// <summary>
        /// Get if two factor authentication is enabled on the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        /// <summary>
        /// Get user lock out end date
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            return
                Task.FromResult(user.LockoutEndDateUtc.HasValue
                    ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                    : new DateTimeOffset());
        }

        /// <summary>
        /// Set user lockout end date
        /// </summary>
        /// <param name="user"></param>
        /// <param name="lockoutEnd"></param>
        /// <returns></returns>
        public async Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDateUtc = lockoutEnd.UtcDateTime;
            var result = await userService.UpdateAsync(GetCluster(user), user);
        }

        /// <summary>
        /// Increment failed access count
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            user.AccessFailedCount++;
            var result = await userService.UpdateAsync(GetCluster(user), user);
            return user.AccessFailedCount;
        }

        /// <summary>
        /// Reset failed access count
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task ResetAccessFailedCountAsync(TUser user)
        {
            user.AccessFailedCount = 0;
            var result = await userService.UpdateAsync(GetCluster(user), user);
        }

        /// <summary>
        /// Get failed access count
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        /// <summary>
        /// Get if lockout is enabled for the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        /// <summary>
        /// Set lockout enabled for user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public async Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            var result = await userService.UpdateAsync(GetCluster(user), user);
        }
    }
}
