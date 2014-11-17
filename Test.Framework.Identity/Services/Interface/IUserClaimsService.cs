using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Identity.Model;

namespace Test.Framework.Identity.Services
{
    public interface IUserClaimsService
    {
        ClaimsIdentity FindByUserId(Guid userId);
        ClaimsIdentity FindByUserId(int clusterId, Guid userId);
        Task<ClaimsIdentity> FindByUserIdAsync(Guid userId);
        Task<ClaimsIdentity> FindByUserIdAsync(int clusterId, Guid userId);
        bool Delete(Guid userId);
        bool Delete(int clusterId, Guid userId);
        Task<bool> DeleteAsync(Guid userId);
        Task<bool> DeleteAsync(int clusterId, Guid userId);
        bool Insert(Claim userClaim, Guid userId);
        bool Insert(int clusterId, Claim userClaim, Guid userId);
        Task<bool> InsertAsync(Claim userClaim, Guid userId);
        Task<bool> InsertAsync(int clusterId, Claim userClaim, Guid userId);
        bool Delete(IdentityUser user, Claim claim);
        bool Delete(int clusterId, IdentityUser user, Claim claim);
        Task<bool> DeleteAsync(IdentityUser user, Claim claim);
        Task<bool> DeleteAsync(int clusterId, IdentityUser user, Claim claim);
    }
}
