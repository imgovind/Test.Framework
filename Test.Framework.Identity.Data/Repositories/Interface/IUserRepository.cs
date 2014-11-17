using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Data;
using Test.Framework.Identity.Entity;

namespace Test.Framework.Identity.Data
{
    public interface IUserRepository
    {
        string GetUserName(Guid userId);
        Task<string> GetUserNameAsync(Guid userId);
        string GetUserId(string userName);
        Task<string> GetUserIdAsync(string userName);
        User GetUserById(Guid userId);
        Task<User> GetUserByIdAsync(Guid userId);
        IEnumerable<User> GetUserByName(string userName);
        Task<IEnumerable<User>> GetUserByNameAsync(string userName);
        IEnumerable<User> GetUserByEmail(string email);
        Task<IEnumerable<User>> GetUserByEmailAsync(string email);
        string GetPasswordHash(Guid userId);
        Task<string> GetPasswordHashAsync(Guid userId);
        bool SetPasswordHash(Guid userId, string passwordHash);
        Task<bool> SetPasswordHashAsync(Guid userId, string passwordHash);
        string GetSecurityStamp(Guid userId);
        Task<string> GetSecurityStampAsync(Guid userId);
        bool Insert(User user);
        Task<bool> InsertAsync(User user);
        bool Delete(Guid userId);
        Task<bool> DeleteAsync(Guid userId);
        bool Delete(User user);
        Task<bool> DeleteAsync(User user);
        bool Update(User user);
        Task<bool> UpdateAsync(User user);
    }
}
