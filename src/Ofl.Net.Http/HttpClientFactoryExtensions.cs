using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Ofl.Net.Http
{
    public static class HttpClientFactoryExtensions
    {
        public static Task<HttpClient> CreateAsync(this IHttpClientFactory factory, CancellationToken cancellationToken)
        {
            // Create the message handler.
            HttpMessageHandler handler = HttpMessageHandlerExtensions.CreateDefaultMessageHandler();

            // Call the overload.
            return factory.CreateAsync(handler, cancellationToken);
        }

        public static Task<HttpClient> CreateAsync(this IHttpClientFactory factory, HttpMessageHandler handler, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            // Call the overload.
            return factory.CreateAsync(handler, true, cancellationToken);
        }
    }
}
