using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Data;
using Test.Framework.Identity.Entity;

namespace Test.Framework.Identity.Data
{
    public interface IRoleRepository
    {
        bool Delete(Guid roleId);
        Task<bool> DeleteAsync(Guid roleId);
        bool Delete(Role role);
        Task<bool> DeleteAsync(Role role);
        bool Insert(Role role);
        Task<bool> InsertAsync(Role role);
        string GetRoleName(Guid roleId);
        Task<string> GetRoleNameAsync(Guid roleId);
        Guid GetRoleId(string roleName);
        Task<Guid> GetRoleIdAsync(string roleName);
        Role GetRoleById(Guid roleId);
        Task<Role> GetRoleByIdAsync(Guid roleId);
        Role GetRoleByName(string roleName);
        Task<Role> GetRoleByNameAsync(string roleName);
        bool Update(Role role);
        Task<bool> UpdateAsync(Role role);
    }
}
