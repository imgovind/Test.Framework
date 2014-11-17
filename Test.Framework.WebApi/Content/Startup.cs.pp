using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Test.Framework.WebApi;

[assembly: OwinStartup(typeof($rootnamespace$.Startup))]

namespace $rootnamespace$
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AppInitializer.Core.Initialize();

            //ConfigureAuth(app);

            var config = new HttpConfiguration();

            WebApiConfig.Register(config);

            app.UseCors(CorsOptions.AllowAll);

            app.UseWebApi(config);
        }
    }
}
