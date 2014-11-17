using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.WebApi.OAuth
{
    public partial class JwtWebApiConstants
    {
        public static class Jwt
        {
            public static IWebConfiguration config { get { return Container.Resolve<IWebConfiguration>(); } }

            public static string IssuerName { get { return config.AppSettings["jwt:IssuerName"]; } }
            public static string SigningKey { get { return config.AppSettings["jwt:SigningKey"]; } }
            public static string AllowedAudience { get { return config.AppSettings["jwt:AllowedAudience"]; } }
            public static TimeSpan DefaultTimeSpan { get { return TimeSpan.Parse(config.AppSettings["jwt:DefaultTimeSpan"]); } }
        }
    }
}
