using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Test.Framework.WebApi
{
    /// <summary>
    /// Generates ETag based where ContentHashETagAttribute has been defined on action 
    /// </summary>
    public class ContentHashETagGenerator : DefaultETagGenerator
    {
        public override EntityTagHeaderValue Generate(string url, 
            IEnumerable<KeyValuePair<string, IEnumerable<string>>> requestHeaders)
        {
            var keyValuePair = requestHeaders.FirstOrDefault(x => 
                x.Key == ContentHashETagAttribute.ContentHashHeaderName);

            
            return keyValuePair.Key == null ? base.Generate(url, requestHeaders) :
                new EntityTagHeaderValue("\"" + keyValuePair.Value.First() + "\"", false);
        }
    }
}
