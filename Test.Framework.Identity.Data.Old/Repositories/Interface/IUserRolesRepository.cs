using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Data;
using Test.Framework.Identity.Entity;

namespace Test.Framework.Identity.Data
{
    public interface IUserRolesRepository
    {
        IEnumerable<string> FindByUserId(Guid userId);
        Task<IEnumerable<string>> FindByUserIdAsync(Guid userId);
        bool Delete(Guid userId);
        Task<bool> DeleteAsync(Guid userId);
        bool Insert(UserRole userRole);
        Task<bool> InsertAsync(UserRole userRole);
        bool Update(UserRole userRole);
        Task<bool> UpdateAsync(UserRole userRole);
        bool Delete(UserRole userRole);
        Task<bool> DeleteAsync(UserRole userRole);
    }
}
