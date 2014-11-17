using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Identity.Model;

namespace Test.Framework.Identity.Services
{
    public interface IUserService<TUser>
        where TUser: IdentityUser
    {
        string GetUserName(Guid userId);
        string GetUserName(int clusterId, Guid userId);
        Task<string> GetUserNameAsync(Guid userId);
        Task<string> GetUserNameAsync(int clusterId, Guid userId);
        string GetUserId(string userName);
        string GetUserId(int clusterId, string userName);
        Task<string> GetUserIdAsync(string userName);
        Task<string> GetUserIdAsync(int clusterId, string userName);
        TUser GetUserById(Guid userId);
        TUser GetUserById(int clusterId, Guid userId);
        Task<TUser> GetUserByIdAsync(Guid userId);
        Task<TUser> GetUserByIdAsync(int clusterId, Guid userId);
        List<TUser> GetUserByName(string userName);
        List<TUser> GetUserByName(int clusterId, string userName);
        Task<List<TUser>> GetUserByNameAsync(string userName);
        Task<List<TUser>> GetUserByNameAsync(int clusterId, string userName);
        List<TUser> GetUserByEmail(string email);
        List<TUser> GetUserByEmail(int clusterId, string email);
        Task<List<TUser>> GetUserByEmailAsync(string email);
        Task<List<TUser>> GetUserByEmailAsync(int clusterId, string email);
        string GetPasswordHash(Guid userId);
        string GetPasswordHash(int clusterId, Guid userId);
        Task<string> GetPasswordHashAsync(Guid userId);
        Task<string> GetPasswordHashAsync(int clusterId, Guid userId);
        bool SetPasswordHash(Guid userId, string passwordHash);
        bool SetPasswordHash(int clusterId, Guid userId, string passwordHash);
        Task<bool> SetPasswordHashAsync(Guid userId, string passwordHash);
        Task<bool> SetPasswordHashAsync(int clusterId, Guid userId, string passwordHash);
        string GetSecurityStamp(Guid userId);
        string GetSecurityStamp(int clusterId, Guid userId);
        Task<string> GetSecurityStampAsync(Guid userId);
        Task<string> GetSecurityStampAsync(int clusterId, Guid userId);
        bool Insert(TUser user);
        bool Insert(int clusterId, TUser user);
        Task<bool> InsertAsync(TUser user);
        Task<bool> InsertAsync(int clusterId, TUser user);
        bool Delete(Guid userId);
        bool Delete(int clusterId, Guid userId);
        Task<bool> DeleteAsync(Guid userId);
        Task<bool> DeleteAsync(int clusterId, Guid userId);
        bool Delete(TUser user);
        bool Delete(int clusterId, TUser user);
        Task<bool> DeleteAsync(TUser user);
        Task<bool> DeleteAsync(int clusterId, TUser user);
        bool Update(TUser user);
        bool Update(int clusterId, TUser user);
        Task<bool> UpdateAsync(TUser user);
        Task<bool> UpdateAsync(int clusterId, TUser user);
    }
}
