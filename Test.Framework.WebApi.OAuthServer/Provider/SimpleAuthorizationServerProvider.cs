using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Identity.Data;
using Test.Framework.Identity.Entity;
using Test.Framework.Identity.Enum;
using Test.Framework.Identity.Model;

namespace Test.Framework.Identity.Provider
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public ApplicationUserManager userManager
        {
            get
            {
                return new ApplicationUserManager(Container.Resolve<IUserStore<IdentityUser, Guid>>());
            }
        }

        public IIdentityDataProvider DataProvider
        {
            get
            {
                return Container.Resolve<IIdentityDataProvider>();
            }
        }

        //public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        //{
        //    // validate client credentials
        //    // should be stored securely (salted, hashed, iterated)
        //    string id, secret;
        //    if (context.TryGetBasicCredentials(out id, out secret))
        //    {
        //        if (secret == "secret")
        //        {
        //            context.Validated();
        //        }
        //    }
        //}

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>(IdentityConstants.Client.Context.AllowedOrigin);

            if (allowedOrigin == null) allowedOrigin = "*";

            context.OwinContext.Response.Headers.Add(IdentityConstants.Client.Header.AllowedOrigin, new[] { allowedOrigin });

            IdentityUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            // create identity
            var id = new ClaimsIdentity(context.Options.AuthenticationType);
            id.AddClaim(new Claim("Name", context.UserName));
            id.AddClaim(new Claim("sub", context.UserName));
            id.AddClaim(new Claim("issued_at", DateTime.UtcNow.ToString()));
            id.AddClaim(new Claim("email", user.Email));
            id.AddClaim(new Claim("email_verified", user.EmailConfirmed.ToString()));

            context.OwinContext.Set<string>(IdentityConstants.Client.Context.UserName, context.UserName);
            context.OwinContext.Set<string>(IdentityConstants.Client.Context.UserId, user.Id.ToString());

            var props = new AuthenticationProperties(new Dictionary<string, string> { 
                { IdentityConstants.Client.Context.Id, (context.ClientId == null) ? string.Empty : context.ClientId },
                { "issuer", IdentityConstants.JwtIssuerName },
                { "audience", IdentityConstants.JwtAllowedAudience }
            });

            var ticket = new AuthenticationTicket(id, props);
            context.Validated(ticket);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary[IdentityConstants.Client.Context.Id];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            //var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            //newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            //var newTicket = new AuthenticationTicket(context.Ticket.Identity, context.Ticket.Properties);
            context.Validated(context.Ticket);

            return Task.FromResult<object>(null);
        }

        //public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        //{
        //    foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
        //    {
        //        context.AdditionalResponseParameters.Add(property.Key, property.Value);
        //    }

        //    return Task.FromResult<object>(null);
        //}

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            Client client = null;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context 
                //if you want to force sending clientId/secrects once obtain access tokens. 
                context.Validated();
                //context.SetError("invalid_clientId", "ClientId should be sent.");
                return Task.FromResult<object>(null);
            }

            client = this.DataProvider.AuthenticationRepository().FindClient(context.ClientId);

            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
                return Task.FromResult<object>(null);
            }

            if (client.Type == ClientTypes.iOS || client.Type == ClientTypes.Android)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent.");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    if (client.Secret != CryptoHelper.GetHash(clientSecret))
                    {
                        context.SetError("invalid_clientId", "Client secret is invalid.");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            if (!client.IsActive)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set<string>(IdentityConstants.Client.Context.PK, client.Id.ToString());
            context.OwinContext.Set<string>(IdentityConstants.Client.Context.AllowedOrigin, client.AllowedOrigin);
            context.OwinContext.Set<string>(IdentityConstants.RefreshToken.LifeTime, client.RefreshTokenLifeTime.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }

        //public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        //{
        //    if (context.ClientId == _publicClientId)
        //    {
        //        Uri expectedRootUri = new Uri(context.Request.Uri, "/");

        //        if (expectedRootUri.AbsoluteUri == context.RedirectUri)
        //        {
        //            context.Validated();
        //        }
        //    }

        //    return Task.FromResult<object>(null);
        //}

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}
