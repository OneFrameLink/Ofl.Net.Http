using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ofl.Net.Http
{
    public static partial class HttpClientExtensions
    {
        #region DeleteJsonAsync

        public static Task<TResult> DeleteJsonAsync<TResult>(this HttpClient httpClient, string uri,
            CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));

            // Call the overload.
            return httpClient.DeleteJsonAsync<TResult>(uri, JsonSerializer.CreateDefault(), cancellationToken);
        }

        public static Task<TResponse> DeleteJsonAsync<TResponse>(this HttpClient httpClient, string uri,
            JsonSerializerSettings jsonSerializerSettings, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));
            if (jsonSerializerSettings == null) throw new ArgumentNullException(nameof(jsonSerializerSettings));

            // Call the overload.
            return httpClient.DeleteJsonAsync<TResponse>(uri, JsonSerializer.Create(jsonSerializerSettings),
                cancellationToken);
        }

        public static async Task<TResponse> DeleteJsonAsync<TResponse>(this HttpClient httpClient,
            string uri, JsonSerializer jsonSerializer, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));
            if (jsonSerializer == null) throw new ArgumentNullException(nameof(jsonSerializer));

            // Get the message.
            using (HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync(uri, cancellationToken).ConfigureAwait(false))
            {
                // Ensure the response code.
                httpResponseMessage.EnsureSuccessStatusCode();

                // Deserialize the result.
                return await httpResponseMessage.ToObjectAsync<TResponse>(jsonSerializer, cancellationToken).
                    ConfigureAwait(false);
            }
        }

        #endregion
    }
}
