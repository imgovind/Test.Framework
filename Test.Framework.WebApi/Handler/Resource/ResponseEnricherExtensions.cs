using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Test.Framework.Handlers.Resource
{
    public static class ResponseEnricherExtensions
    {
        public static void AddResponseEnrichers(this HttpConfiguration config, params IResponseEnricher[] enrichers)
        {
            foreach (var enricher in enrichers)
            {
                config.GetResponseEnrichers().Add(enricher);
            }
        }

        public static Collection<IResponseEnricher> GetResponseEnrichers(this HttpConfiguration config)
        {
            return (Collection<IResponseEnricher>)config.Properties.GetOrAdd(
                    typeof(Collection<IResponseEnricher>),
                    k => new Collection<IResponseEnricher>()
                );
        }
    }
}
