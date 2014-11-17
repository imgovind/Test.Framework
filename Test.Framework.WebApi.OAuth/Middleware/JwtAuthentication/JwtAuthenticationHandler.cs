using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Thinktecture.IdentityModel;

namespace Test.Framework.WebApi.OAuth
{
    public class JwtAuthenticationHandler : AuthenticationHandler<JwtAuthenticationOptions>
    {
        private string challenge;
        public JwtAuthenticationHandler(JwtAuthenticationOptions options, string challenge)
        {
            this.challenge = challenge;
        }

        protected async override Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            // Find token in default location
            string requestToken = null;
            string authorization = Request.Headers.Get("Authorization");
            if (!string.IsNullOrEmpty(authorization))
            {
                if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    requestToken = authorization.Substring("Bearer ".Length).Trim();
                }
            }

            // Give application opportunity to find from a different location, adjust, or reject token
            var requestTokenContext = new OAuthRequestTokenContext(Context, requestToken);
            await Options.Provider.RequestToken(requestTokenContext);

            // If no token found, no further work possible
            if (string.IsNullOrEmpty(requestTokenContext.Token))
            {
                return null;
            }

            //var validatingCert = X509.LocalMachine.My.SubjectDistinguishedName.Find("CN=as.local", false).First();
            var tokenHandler = new JwtSecurityTokenHandler();
            //var x509SecurityToken = new X509SecurityToken(validatingCert);

            var validationParameters = new TokenValidationParameters
            {
                ValidAudiences = Options.AllowedAudiences,
                IssuerSigningKey = new InMemorySymmetricSecurityKey(Convert.FromBase64String(JwtWebApiConstants.Jwt.SigningKey)),
                ValidIssuer = JwtWebApiConstants.Jwt.IssuerName
            };

            var tokenReceiveContext = new AuthenticationTokenReceiveContext(
                Context,
                Options.AccessTokenFormat,
                requestTokenContext.Token);

            SecurityToken outToken;
            ClaimsPrincipal output = tokenHandler.ValidateToken(requestToken, validationParameters, out outToken);

            var id = new ClaimsIdentity(output.Claims, Options.AuthenticationType);
            if (id == null)
                return null;

            var props = new AuthenticationProperties(new Dictionary<string, string> { 
                { "issuer", JwtWebApiConstants.Jwt.IssuerName },
                { "audience", JwtWebApiConstants.Jwt.AllowedAudience }
            });

            AuthenticationTicket ticket = new AuthenticationTicket(id, props);

            return ticket;
        }

        protected override Task ApplyResponseChallengeAsync()
        {
            if (Response.StatusCode != 401)
            {
                return Task.FromResult<object>(null);
            }

            AuthenticationResponseChallenge challenge = Helper.LookupChallenge(Options.AuthenticationType, Options.AuthenticationMode);

            if (challenge != null)
            {
                OAuthChallengeContext challengeContext = new OAuthChallengeContext(Context, this.challenge);
                Options.Provider.ApplyChallenge(challengeContext);
            }

            return Task.FromResult<object>(null);
        }
    }
}