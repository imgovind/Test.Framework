using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Framework.WebApi.OAuth
{
    public static class JwtAuthenticationMiddlewareExtensions
    {
        public static IAppBuilder UseJwtAuthentication(this IAppBuilder app, JwtAuthenticationOptions options)
        {
            return app.Use<JwtAuthenticationMiddleware>(options);
        }
    }
}