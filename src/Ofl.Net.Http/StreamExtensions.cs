using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ofl.IO;

namespace Ofl.Net.Http
{
    public static class StreamExtensions
    {
        public static async Task<HttpResponseMessage> ToHttpResponseMessageAsync(this Stream stream, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            // The response message.
            var response = new HttpResponseMessage();

            // Get the enumerator.  Dispose when done.
            using (IAsyncEnumerator<byte> enumerator = stream.ToAsyncEnumerable().GetEnumerator())
            {
                // Get the status line.
                await response.SetStatusLineAsync(enumerator, cancellationToken).ConfigureAwait(false);

                // Read the headers.
                await response.SetHeaderFieldsAsync(enumerator, cancellationToken).ConfigureAwait(false);

                // Read the content.
                await response.SetContentAsync(enumerator, cancellationToken).ConfigureAwait(false);
            }

            // Return the response.
            return response;
        }
    }
}
