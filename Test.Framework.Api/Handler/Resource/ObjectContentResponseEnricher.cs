﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Handlers.Resource
{
    /// <summary>
    /// A base class for response enrichers that require access to the response object content.
    /// </summary>
    public abstract class ObjectContentResponseEnricher<T> : IResponseEnricher
    {
        private HttpResponseMessage httpResponse;

        public virtual bool CanEnrich(Type contentType)
        {
            return contentType == typeof(T);
        }

        public abstract void Enrich(T content);

        bool IResponseEnricher.CanEnrich(HttpResponseMessage response)
        {
            var content = response.Content as ObjectContent;
            return (content != null && CanEnrich(content.ObjectType));
        }

        HttpResponseMessage IResponseEnricher.Enrich(HttpResponseMessage response)
        {
            httpResponse = response;

            T content;
            if (response.TryGetContentValue(out content))
            {
                Enrich(content);
            }

            return response;
        }

        protected HttpRequestMessage Request
        {
            get { return httpResponse.RequestMessage; }
        }
    }
}
