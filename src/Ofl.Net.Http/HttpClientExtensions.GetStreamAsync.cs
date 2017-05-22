using System;
using System.IO;
using System.Net.Http;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;
using Ofl.IO;

namespace Ofl.Net.Http
{
    public static partial class HttpClientExtensions
    {
        #region GetStreamAsync

        public static Task<Stream> GetStreamAsync(this HttpClient httpClient,
            string requestUri, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(requestUri)) throw new ArgumentNullException(nameof(requestUri));

            // Call the overload, dispose of the client.
            return httpClient.GetStreamAsync(new Uri(requestUri), true, cancellationToken);
        }

        public static Task<Stream> GetStreamAsync(this HttpClient httpClient,
            Uri requestUri, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (requestUri == null) throw new ArgumentNullException(nameof(requestUri));

            // Call the overload, dispose of the client.
            return httpClient.GetStreamAsync(requestUri, true, cancellationToken);
        }

        public static Task<Stream> GetStreamAsync(this HttpClient httpClient,
            string requestUri, bool disposeHttpClient, CancellationToken cancellationToken)
        {

            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (string.IsNullOrWhiteSpace(requestUri)) throw new ArgumentNullException(nameof(requestUri));

            // Call the overload, dispose of the client.
            return httpClient.GetStreamAsync(new Uri(requestUri), disposeHttpClient, cancellationToken);
        }

        public static async Task<Stream> GetStreamAsync(this HttpClient httpClient,
            Uri requestUri, bool disposeHttpClient, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            if (requestUri == null) throw new ArgumentNullException(nameof(requestUri));

            // Create a disposable.
            var compositeDisposable = new CompositeDisposable();

            // The copy.
            CompositeDisposable compositeDisposableCopy = compositeDisposable;

            // Wrap in a try/finally.
            try
            {
                // Add the client if disposing of it.
                if (disposeHttpClient) compositeDisposable.Add(httpClient);

                // Set the response message.
                HttpResponseMessage httpResponseMessage;
                compositeDisposable.Add(httpResponseMessage =
                    await httpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false));

                // Get the stream.  Wrap.
                Stream stream;
                compositeDisposable.Add(stream =
                    await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false));

                // Set the copy to null.
                compositeDisposableCopy = null;

                // Return the stream.
                return new StreamWithState<IDisposable>(stream, compositeDisposable);
            }
            catch
            {
                // Dispose.
                using (compositeDisposableCopy)
                    // Throw.
                    throw;
            }
        }

        #endregion
    }
}
