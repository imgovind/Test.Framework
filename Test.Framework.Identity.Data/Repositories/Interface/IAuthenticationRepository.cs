using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Data;
using Test.Framework.Identity.Entity;

namespace Test.Framework.Identity.Data
{
    public interface IAuthenticationRepository : IRepository
    {
        Client FindClient(Guid clientId);
        Client FindClient(string clientName);
        Task<bool> AddRefreshToken(RefreshToken token);
        Task<bool> RemoveRefreshToken(string refreshTokenId);
        Task<bool> RemoveRefreshToken(RefreshToken refreshToken);
        Task<RefreshToken> FindRefreshToken(string refreshTokenId);
        List<RefreshToken> GetAllRefreshTokens();
        Task<bool> AddUserClient(UserClient userClient);
        Task<UserClient> FindUserClient(Guid userId, Guid clientId);
    }
}
