using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;
using Test.Framework.Identity;
using Test.Framework.Identity.Provider;
using Test.Framework.WebApi.OAuth;

namespace $rootnamespace$
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            //app.UseCookieAuthentication(new CookieAuthenticationOptions());
            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            //PublicClientId = "self";
            //OAuthOptions = new OAuthAuthorizationServerOptions
            //{
            //TokenEndpointPath = new PathString("/Token"),
            //Provider = new ApplicationOAuthProvider(PublicClientId),
            //Provider = new SimpleAuthorizationServerProvider(),
            //AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
            //AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
            //AllowInsecureHttp = true
            //};

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                // for demo purposes
                AccessTokenFormat = new CustomJwtFormat(
                    Container.Resolve<ISigningCredentialsProvider>(),
                    IdentityConstants.JwtDefaultTimeSpan),
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/Token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(8),
                Provider = new SimpleAuthorizationServerProvider(),
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
            });

            // token consumption
            app.UseJwtAuthentication(new JwtAuthenticationOptions
            {
                AccessTokenFormat = new CustomJwtFormat(Container.Resolve<ISigningCredentialsProvider>(), IdentityConstants.JwtDefaultTimeSpan),
                AuthenticationMode = AuthenticationMode.Active,
                AllowedAudiences = new[] { IdentityConstants.JwtAllowedAudience },
                SigningCredentials = Container.Resolve<ISigningCredentialsProvider>().GetSigningCredentials(IdentityConstants.JwtIssuerName, IdentityConstants.JwtAllowedAudience)
            });

            //app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            //{
            //    AuthenticationMode = AuthenticationMode.Active,
            //    TokenValidationParameters = new TokenValidationParameters{
            //        ValidAudiences = new[] { AppSettings.JwtAllowedAudience },
            //        IssuerSigningKey = new InMemorySymmetricSecurityKey(Convert.FromBase64String(AppSettings.JwtSigningKey)),
            //        ValidIssuer = AppSettings.JwtIssuerName
            //    }
            //});



            // Enable the application to use bearer tokens to authenticate users
            //app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}
