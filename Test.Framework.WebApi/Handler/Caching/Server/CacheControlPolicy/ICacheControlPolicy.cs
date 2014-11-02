using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Test.Framework.WebApi
{
    public interface ICacheControlPolicy
    {
        CacheControlHeaderValue GetCacheControl(HttpRequestMessage request, HttpConfiguration configuration);
    }
}
