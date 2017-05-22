using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ofl.Net.Http
{
    public static partial class HttpClientExtensions
    {
        #region GetJsonAsync

        public static Task<T> GetJsonAsync<T>(this HttpClient httpClient, string uri,
            CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));

            // Call the overload.
            return httpClient.GetJsonAsync<T>(uri, JsonSerializer.CreateDefault(), cancellationToken);
        }

        public static Task<T> GetJsonAsync<T>(this HttpClient httpClient, string uri,
            JsonSerializerSettings jsonSerializerSettings, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));
            if (jsonSerializerSettings == null) throw new ArgumentNullException(nameof(jsonSerializerSettings));

            // Call the overload.
            return httpClient.GetJsonAsync<T>(uri, JsonSerializer.Create(jsonSerializerSettings),
                cancellationToken);
        }

        public static async Task<T> GetJsonAsync<T>(this HttpClient httpClient,
            string uri, JsonSerializer jsonSerializer, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));
            if (jsonSerializer == null) throw new ArgumentNullException(nameof(jsonSerializer));

            // Get the message.
            using (HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false))
            {
                // Ensure the status code.
                httpResponseMessage.EnsureSuccessStatusCode();

                // Return the object.
                return await httpResponseMessage.ToObjectAsync<T>(jsonSerializer, cancellationToken).ConfigureAwait(false);
            }
        }

        #endregion
    }
}
