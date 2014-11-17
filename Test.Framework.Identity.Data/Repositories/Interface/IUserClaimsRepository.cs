using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Data;
using Test.Framework.Identity.Entity;

namespace Test.Framework.Identity.Data
{
    public interface IUserClaimsRepository
    {
        IEnumerable<UserClaim> Get(Guid userId);
        Task<IEnumerable<UserClaim>> GetAsync(Guid userId);
        bool Delete(Guid userId);
        Task<bool> DeleteAsync(Guid userId);
        bool Delete(UserClaim userClaim);
        Task<bool> DeleteAsync(UserClaim userClaim);
        bool Insert(UserClaim userClaim);
        Task<bool> InsertAsync(UserClaim userClaim);
    }
}
