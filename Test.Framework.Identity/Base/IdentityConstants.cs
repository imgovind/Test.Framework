using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Identity
{
    public static class IdentityConstants
    {
        public static class UserClient
        {
            public static string Id { get; set; }
        }

        public static class Client
        {
            public static class Context
            {
                public static string AllowedOrigin { get { return "as:clientAllowedOrigin"; } }
                public static string Id { get { return "as:clientId"; } }
                public static string PK { get { return "as:clientIdPK"; } }
                public static string UserName { get { return "as:clientUserName"; } }
                public static string UserId { get { return "as:clientUserId"; } }
                public static string UserClientId { get { return "as:clientUserClientId"; } }

            }

            public static class Header
            {
                public static string AllowedOrigin { get { return "Access-Control-Allow-Origin"; } }
            }
        }

        public static class RefreshToken
        {
            public static string LifeTime { get { return "as:refreshTokenLifeTime"; } }
        }
    }
}
