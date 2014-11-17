using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Test.Framework.WebApi.OAuth
{
    public class TestAuthorizationFilterAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //Helper.Write("AuthorizationFilter", actionContext.RequestContext.Principal);

            var principal = actionContext.RequestContext.Principal;

            return true;
        }
    }
}