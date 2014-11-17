using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Test.Data;
using Test.Data.Repositories;
using Test.Framework.Extensibility;
using Test.Framework.Extensions;
using Test.Framework.Identity;
using Test.Framework.Identity.Entity;

namespace Test.WebApi.Middleware
{
    public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider
    {
        public IDataProvider DataProvider
        {
            get
            {
                return Container.Resolve<IDataProvider>();
            }
        }

        public async Task<Guid> GetUserClientId(string contextUserClientId, Guid userId, Guid clientId)
        {
            if (contextUserClientId.IsNotNullOrEmpty())
                return new Guid(contextUserClientId);

            Guid userClientId = Guid.NewGuid();

            var existingUserClient = await this.DataProvider.AuthenticationRepository.FindUserClient(userId, clientId);

            if (existingUserClient != null && existingUserClient.Id.IsNotEmpty())
                return existingUserClient.Id;

            var userClient = new UserClient
            {
                Id = userClientId,
                UserId = userId,
                ClientId = clientId,
                IsActive = true
            };

            var result = await this.DataProvider.AuthenticationRepository.AddUserClient(userClient);

            if (!result)
                return Guid.Empty;

            return userClientId;
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientName = context.Ticket.Properties.Dictionary[IdentityConstants.Client.Context.Id];
            var refreshTokenLifeTime = context.OwinContext.Get<string>(IdentityConstants.RefreshToken.LifeTime);
            var clientIdString = context.OwinContext.Get<string>(IdentityConstants.Client.Context.PK);
            var userIdString = context.OwinContext.Get<string>(IdentityConstants.Client.Context.UserId);

            if (clientName.IsNullOrEmpty() || 
                clientIdString.IsNullOrEmpty() || 
                userIdString.IsNullOrEmpty() || 
                !clientIdString.IsGuid() || 
                !userIdString.IsGuid())
                return;

            var clientId = new Guid(clientIdString);
            var userId = new Guid(userIdString);

            Guid userClientId = await GetUserClientId(context.OwinContext.Get<string>(IdentityConstants.Client.Context.UserClientId), userId, clientId);

            var refreshTokenId = Guid.NewGuid().ToString("n");

            var token = new RefreshToken()
            {
                Id = CryptoHelper.GetHash(refreshTokenId),
                ClientId = clientId,
                ClientName = clientName,
                Subject = context.Ticket.Identity.Name.IsNotNullOrEmpty() 
                            ? context.Ticket.Identity.Name 
                            : context.OwinContext.Get<string>(IdentityConstants.Client.Context.UserName),
                IssuedUtc = DateTime.UtcNow,
                ExpiredUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime)),
                UserId = userId,
                UserClientId = userClientId,
                ProtectedTicket = context.SerializeTicket()
            };

            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiredUtc;

            bool result = await this.DataProvider.AuthenticationRepository.AddRefreshToken(token);

            if (result)
                context.SetToken(refreshTokenId);
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>(IdentityConstants.Client.Context.AllowedOrigin);
            context.OwinContext.Response.Headers.Add(IdentityConstants.Client.Header.AllowedOrigin, new[] { allowedOrigin });

            string hashedTokenId = CryptoHelper.GetHash(context.Token);

            var refreshToken = await this.DataProvider.AuthenticationRepository.FindRefreshToken(hashedTokenId);

            context.OwinContext.Set<string>(IdentityConstants.Client.Context.UserName, refreshToken.Subject);
            context.OwinContext.Set<string>(IdentityConstants.Client.Context.UserId, refreshToken.UserId.ToString());
            context.OwinContext.Set<string>(IdentityConstants.Client.Context.UserClientId, refreshToken.UserClientId.ToString());

            if (refreshToken != null)
            {
                //Get protectedTicket from refreshToken class
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                var result = await this.DataProvider.AuthenticationRepository.RemoveRefreshToken(hashedTokenId);
            }
        }
    }
}