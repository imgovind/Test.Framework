using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Identity.Model;

namespace Test.Framework.Identity.Services
{
    public interface IUserRolesService
    {
        List<string> FindByUserId(Guid userId);
        List<string> FindByUserId(int clusterId, Guid userId);
        Task<List<string>> FindByUserIdAsync(Guid userId);
        Task<List<string>> FindByUserIdAsync(int clusterId, Guid userId);
        bool Delete(Guid userId);
        bool Delete(int clusterId, Guid userId);
        Task<bool> DeleteAsync(Guid userId);
        Task<bool> DeleteAsync(int clusterId, Guid userId);
        bool Insert(IdentityUser user, Guid roleId);
        bool Insert(int clusterId, IdentityUser user, Guid roleId);
        Task<bool> InsertAsync(IdentityUser user, Guid roleId);
        Task<bool> InsertAsync(int clusterId, IdentityUser user, Guid roleId);
    }
}
