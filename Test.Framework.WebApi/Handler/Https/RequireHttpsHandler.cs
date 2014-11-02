using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Framework.Handlers.Https
{
    /// <summary>
    /// A message handler that enforces all requests are made over HTTPS.
    /// </summary>
    public class RequireHttpsHandler : DelegatingHandler
    {
        private const string ReasonPhrase = "SSL Required";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                var response = request.CreateResponse(HttpStatusCode.Forbidden);
                response.ReasonPhrase = ReasonPhrase;

                return Task.FromResult(response);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
