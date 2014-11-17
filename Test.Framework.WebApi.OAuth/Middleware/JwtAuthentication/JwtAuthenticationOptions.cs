using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;

namespace Test.Framework.WebApi.OAuth
{
    public class JwtAuthenticationOptions : AuthenticationOptions
    {
        public SigningCredentials SigningCredentials { get; set; }
        public IEnumerable<string> AllowedAudiences { get; set; }
        public string Realm { get; set; }
        public string Challenge { get; set; }
        public ISystemClock SystemClock { get; set; }
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; set; }
        public IOAuthBearerAuthenticationProvider Provider { get; set; }
        public IAuthenticationTokenProvider AccessTokenProvider { get; set; }
        public JwtAuthenticationOptions() 
            : base(OAuthDefaults.AuthenticationType)
        {
            SystemClock = new SystemClock();
        }
    }
}