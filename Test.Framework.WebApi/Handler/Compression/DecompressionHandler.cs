using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Test.Framework.Extensions;

namespace Test.Framework.Handlers.Compression
{
    public class DecompressionHandler : DelegatingHandler
    {
        public Collection<ICompressor> Compressors;

        public const string CustomHeaderKey = "X-Request-Encoding";

        public DecompressionHandler()
        {
            Compressors = new Collection<ICompressor>();
            Compressors.Add(new GZipCompressor());
            Compressors.Add(new DeflateCompressor());
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            IEnumerable<string> headerValues;
            var customHeader = string.Empty;
            var keyFound = request.Headers.TryGetValues(CustomHeaderKey, out headerValues);

            if(!keyFound)
                return await base.SendAsync(request, cancellationToken);

            if (keyFound)
            {
                customHeader = headerValues.FirstOrDefault();
            }       

            if (customHeader.IsNullOrEmpty())
                return await base.SendAsync(request, cancellationToken);

            var requestEncodingArray = customHeader.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            if (requestEncodingArray.IsNullOrEmpty() ||
                requestEncodingArray.Length < 2)
                return await base.SendAsync(request, cancellationToken);

            var compressor = Compressors.Where(x => x.EncodingType.Equals(requestEncodingArray[1])).FirstOrDefault();

            if (compressor == null)
                return await base.SendAsync(request, cancellationToken);

            request.Content = await DecompressContentAsync(request.Content, compressor);

            return await base.SendAsync(request, cancellationToken);
        }

        private static async Task<HttpContent> DecompressContentAsync(HttpContent compressedContent, ICompressor compressor)
        {
            using (compressedContent)
            {
                MemoryStream decompressed = new MemoryStream();
                await compressor.Decompress(await compressedContent.ReadAsStreamAsync(), decompressed).ConfigureAwait(false);

                // set position back to 0 so it can be read again
                decompressed.Position = 0;

                var uncompressedContent = new StreamContent(decompressed);

                // copy all the headers from the original request and add it to the uncompressed content
                foreach (var header in compressedContent.Headers)
                {
                    uncompressedContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }

                // copy content type so we know how to load correct formatter
                // uncompressedContent.Headers.ContentType = compressedContent.Headers.ContentType;
                uncompressedContent.Headers.Remove(CustomHeaderKey);

                return uncompressedContent;
            }
        }
    }
}
