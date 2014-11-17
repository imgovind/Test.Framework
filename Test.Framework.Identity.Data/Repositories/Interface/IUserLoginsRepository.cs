using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Data;
using Test.Framework.Identity.Entity;

namespace Test.Framework.Identity.Data
{
    public interface IUserLoginsRepository
    {
        bool Delete(UserLogin userLogin);
        Task<bool> DeleteAsync(UserLogin userLogin);
        bool Delete(Guid userId);
        Task<bool> DeleteAsync(Guid userId);
        bool Insert(UserLogin userLogin);
        Task<bool> InsertAsync(UserLogin userLogin);
        Guid FindUserIdByLogin(string loginProvider, string providerKey);
        Task<Guid> FindUserIdByLoginAsync(string loginProvider, string providerKey);
        IEnumerable<UserLogin> FindByUserId(Guid userId);
        Task<IEnumerable<UserLogin>> FindByUserIdAsync(Guid userId);
    }
}
