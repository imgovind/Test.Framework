using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Identity.Model;

namespace Test.Framework.Identity.Services
{
    public interface IRoleService
    {
        bool Delete(Guid roleId);
        bool Delete(int clusterId, Guid roleId);
        Task<bool> DeleteAsync(Guid roleId);
        Task<bool> DeleteAsync(int clusterId, Guid roleId);
        bool Insert(IdentityRole role);
        bool Insert(int clusterId, IdentityRole role);
        Task<bool> InsertAsync(IdentityRole role);
        Task<bool> InsertAsync(int clusterId, IdentityRole role);
        string GetRoleName(Guid roleId);
        string GetRoleName(int clusterId, Guid roleId);
        Task<string> GetRoleNameAsync(Guid roleId);
        Task<string> GetRoleNameAsync(int clusterId, Guid roleId);
        Guid GetRoleId(string roleName);
        Guid GetRoleId(int clusterId, string roleName);
        Task<Guid> GetRoleIdAsync(string roleName);
        Task<Guid> GetRoleIdAsync(int clusterId, string roleName);
        IdentityRole GetRoleById(Guid roleId);
        IdentityRole GetRoleById(int clusterId, Guid roleId);
        Task<IdentityRole> GetRoleByIdAsync(Guid roleId);
        Task<IdentityRole> GetRoleByIdAsync(int clusterId, Guid roleId);
        IdentityRole GetRoleByName(string roleName);
        IdentityRole GetRoleByName(int clusterId, string roleName);
        Task<IdentityRole> GetRoleByNameAsync(string roleName);
        Task<IdentityRole> GetRoleByNameAsync(int clusterId, string roleName);
        bool Update(IdentityRole role);
        bool Update(int clusterId, IdentityRole role);
        Task<bool> UpdateAsync(IdentityRole role);
        Task<bool> UpdateAsync(int clusterId, IdentityRole role);
    }
}
