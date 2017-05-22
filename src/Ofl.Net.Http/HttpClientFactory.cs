using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Net.Http
{
    public class HttpClientFactory : IHttpClientFactory
    {
        public Task<HttpClient> CreateAsync(HttpMessageHandler handler, bool disposeHandler, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            // Create the instance.
            return Task.FromResult(new HttpClient(handler, disposeHandler));
        }
    }
}
