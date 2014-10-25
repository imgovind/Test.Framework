using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Framework.Handlers.Resource
{
    public class EnrichingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
            var enrichers = request.GetConfiguration().GetResponseEnrichers();

            return enrichers.Where(e => e.CanEnrich(response))
                .Aggregate(response, (resp, enricher) => enricher.Enrich(response));
        }
    }
}
