using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ofl.Serialization.Json.Newtonsoft;

namespace Ofl.Net.Http
{
    public static partial class HttpClientExtensions
    {
        #region PostJsonAsync

        public static Task PostJsonAsync<TRequest>(this HttpClient httpClient, string uri,
            TRequest request, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Call the overload.
            return httpClient.PostJsonAsync(uri, JsonSerializer.CreateDefault(), request, cancellationToken);
        }

        public static Task<TResult> PostJsonAsync<TRequest, TResult>(this HttpClient httpClient, string uri,
            TRequest request, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Call the overload.
            return httpClient.PostJsonAsync<TRequest, TResult>(uri, JsonSerializer.CreateDefault(), request, cancellationToken);
        }

        public static Task PostJsonAsync<TRequest>(this HttpClient httpClient, string uri,
            JsonSerializerSettings jsonSerializerSettings, TRequest request, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));
            if (jsonSerializerSettings == null) throw new ArgumentNullException(nameof(jsonSerializerSettings));
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Call the overload.
            return httpClient.PostJsonAsync(uri, JsonSerializer.Create(jsonSerializerSettings),
                request, cancellationToken);
        }

        public static Task<TResponse> PostJsonAsync<TRequest, TResponse>(this HttpClient httpClient, string uri,
            JsonSerializerSettings jsonSerializerSettings, TRequest request, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));
            if (jsonSerializerSettings == null) throw new ArgumentNullException(nameof(jsonSerializerSettings));
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Call the overload.
            return httpClient.PostJsonAsync<TRequest, TResponse>(uri, JsonSerializer.Create(jsonSerializerSettings),
                request, cancellationToken);
        }

        public static async Task PostJsonAsync<TRequest>(this HttpClient httpClient,
            string uri, JsonSerializer jsonSerializer, TRequest request, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));
            if (jsonSerializer == null) throw new ArgumentNullException(nameof(jsonSerializer));
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Get the message.
            using (HttpResponseMessage httpResponseMessage = await httpClient.
                    PostJsonForHttpResponseMessageAsync(uri, jsonSerializer, request, cancellationToken).ConfigureAwait(false))
                // Ensure the success code.
                httpResponseMessage.EnsureSuccessStatusCode();
        }

        public static async Task<TResponse> PostJsonAsync<TRequest, TResponse>(this HttpClient httpClient,
            string uri, JsonSerializer jsonSerializer, TRequest request, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));
            if (jsonSerializer == null) throw new ArgumentNullException(nameof(jsonSerializer));
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Get the message.
            using (HttpResponseMessage httpResponseMessage = await httpClient.
                PostJsonForHttpResponseMessageAsync(uri, jsonSerializer, request, cancellationToken).ConfigureAwait(false))
            {
                // Ensure the success code.
                httpResponseMessage.EnsureSuccessStatusCode();

                // Return the object.
                return await httpResponseMessage.ToObjectAsync<TResponse>(jsonSerializer, cancellationToken).ConfigureAwait(false);
            }
        }

        public static Task<HttpResponseMessage> PostJsonForHttpResponseMessageAsync<TRequest>(this HttpClient httpClient,
            string uri, JsonSerializerSettings jsonSerializerSettings, TRequest request, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));
            if (jsonSerializerSettings == null) throw new ArgumentNullException(nameof(jsonSerializerSettings));
            if (request == null) throw new ArgumentNullException(nameof(request));

            // Call the overload.
            return httpClient.PostJsonForHttpResponseMessageAsync(uri, JsonSerializer.Create(jsonSerializerSettings),
                request, cancellationToken);
        }

        public static async Task<HttpResponseMessage> PostJsonForHttpResponseMessageAsync<TRequest>(this HttpClient httpClient,
            string uri, JsonSerializer jsonSerializer, TRequest request, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(uri)) throw new ArgumentNullException(nameof(uri));
            if (jsonSerializer == null) throw new ArgumentNullException(nameof(jsonSerializer));
            if (request == null) throw new ArgumentNullException(nameof(request));

            // The json.
            string json = jsonSerializer.SerializeToString(request);

            // Create the content.
            using (HttpContent content = new StringContent(json, Encoding.UTF8, "application/json"))
                // Post and return the message.
                return await httpClient.PostAsync(uri, content, cancellationToken).ConfigureAwait(false);
        }

        #endregion
    }
}
