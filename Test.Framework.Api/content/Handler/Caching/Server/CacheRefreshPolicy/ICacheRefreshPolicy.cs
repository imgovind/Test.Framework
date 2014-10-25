using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace Test.Framework.Api
{
    public interface ICacheRefreshPolicy
    {
        TimeSpan GetCacheRefreshPolicy(HttpRequestMessage request, HttpConfiguration configuration);
    }
}
