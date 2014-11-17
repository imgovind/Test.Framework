using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Identity.Model;

namespace Test.Framework.Identity.Services
{
    public interface IUserLoginsService
    {
        bool Delete(IdentityUser user, UserLoginInfo login);
        bool Delete(int clusterId, IdentityUser user, UserLoginInfo login);
        Task<bool> DeleteAsync(IdentityUser user, UserLoginInfo login);
        Task<bool> DeleteAsync(int clusterId, IdentityUser user, UserLoginInfo login);
        bool Delete(Guid userId);
        bool Delete(int clusterId, Guid userId);
        Task<bool> DeleteAsync(Guid userId);
        Task<bool> DeleteAsync(int clusterId, Guid userId);
        bool Insert(IdentityUser user, UserLoginInfo login);
        bool Insert(int clusterId, IdentityUser user, UserLoginInfo login);
        Task<bool> InsertAsync(IdentityUser user, UserLoginInfo login);
        Task<bool> InsertAsync(int clusterId, IdentityUser user, UserLoginInfo login);
        Guid FindUserIdByLogin(UserLoginInfo userLogin);
        Guid FindUserIdByLogin(int clusterId, UserLoginInfo userLogin);
        Task<Guid> FindUserIdByLoginAsync(UserLoginInfo userLogin);
        Task<Guid> FindUserIdByLoginAsync(int clusterId, UserLoginInfo userLogin);
        List<UserLoginInfo> FindByUserId(Guid userId);
        List<UserLoginInfo> FindByUserId(int clusterId, Guid userId);
        Task<List<UserLoginInfo>> FindByUserIdAsync(Guid userId);
        Task<List<UserLoginInfo>> FindByUserIdAsync(int clusterId, Guid userId);
    }
}
