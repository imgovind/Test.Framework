using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;

namespace Test.Framework.WebApi.OAuth
{
    public interface ISigningCredentialsProvider
    {
        SigningCredentials GetSigningCredentials(string issuer, string audience);
    }
}