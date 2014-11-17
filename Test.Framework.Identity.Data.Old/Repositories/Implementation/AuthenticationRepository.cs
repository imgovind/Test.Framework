using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Data;
using Test.Framework.Extensions;
using Test.Framework.Identity.Entity;
using Test.Framework.Identity.Model;
using Test.Framework.Identity.Enum;

namespace Test.Framework.Identity.Data
{
    public class AuthenticationRepository : BaseRepository, IAuthenticationRepository
    {
        #region Private Members
        
        #endregion

        #region Constructors
        public AuthenticationRepository(IDatabase database)
            : base(database)
        {
        }

        public AuthenticationRepository(string connectionName)
            : base(connectionName)
        {
        } 
        #endregion

        public Client FindClient(Guid clientId)
        {
            return this.Database.Select<Client>(x => x.Id == clientId).FirstOrDefault();
        }

        public Client FindClient(string clientName)
        {
            return this.Database.Select<Client>(x => x.Name == clientName).FirstOrDefault();
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

            var existingToken = this.Database.Select<RefreshToken>(r => r.Subject == token.Subject && r.ClientId == token.ClientId).FirstOrDefault();

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            return this.Database.Insert<RefreshToken>(token);
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshTokenTempResult = await this.Database.SelectAsync<RefreshToken>(x => x.Id == refreshTokenId);
            var refreshToken = refreshTokenTempResult.FirstOrDefault();

            if (refreshToken != null)
            {
                return this.Database.Delete<RefreshToken>(refreshToken);
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            var result = await this.Database.DeleteAsync<RefreshToken>(refreshToken);
            return result;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var tempResult = await this.Database.SelectAsync<RefreshToken>(x => x.Id == refreshTokenId);
            var result = tempResult.FirstOrDefault();
            return result;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            var script = @"SELECT * FROM refreshtokens;";
            return this.Database.Select<RefreshToken>(script, null, 30).ToList();
        }

        public async Task<bool> AddUserClient(UserClient userClient)
        {
            var result = await this.Database.InsertAsync<UserClient>(userClient);
            return result;
        }

        public async Task<UserClient> FindUserClient(Guid userId, Guid clientId)
        {
            var script = @"
                            SELECT *
                            FROM userclients
                            WHERE UserId = @userid
                            AND ClientId = @clientid
                            AND IsActive = 1";
            List<Parameter> parameters = new List<Parameter>();
            parameters.AddParameter("userid", userId);
            parameters.AddParameter("clientid", clientId);
            var tempResult = await this.Database.SelectAsync<UserClient>(script, parameters);
            return tempResult.FirstOrDefault();
        }
    }
}
