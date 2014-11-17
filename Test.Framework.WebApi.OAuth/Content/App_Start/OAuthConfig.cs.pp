using Microsoft.Owin.Security;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Framework;
using Test.Framework.WebApi.OAuth;

namespace $rootnamespace$
{
    public partial class Startup 
    {
        public static void ConfigureAuthConsumer(IAppBuilder app)
        {
            Container.Register<ISigningCredentialsProvider, SigningCredentialsProvider>();

            app.UseJwtAuthentication(new JwtAuthenticationOptions
            {
                AllowedAudiences = new List<string> { JwtWebApiConstants.Jwt.AllowedAudience },
                AuthenticationMode = AuthenticationMode.Active
            });
        }
    }
}
