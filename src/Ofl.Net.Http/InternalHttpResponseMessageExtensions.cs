using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Ofl.Net.Http.Headers;
using Ofl.Text.RegularExpressions;

namespace Ofl.Net.Http
{
    internal static class InternalHttpResponseMessageExtensions
    {
        private static readonly Regex StatusLineRegex = new Regex(@"^HTTP\/(?<version>[0-9]\.[0-9]) (?<statusCode>[0-9]{3}) (?<reasonPhrase>.*)$",
            RegexOptions.Compiled | RegexOptions.Singleline);

        internal static async Task SetStatusLineAsync(this HttpResponseMessage response, IAsyncEnumerator<byte> enumerator, 
            CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (response == null) throw new ArgumentNullException(nameof(response));
            if (enumerator == null) throw new ArgumentNullException(nameof(enumerator));

            // Match the line.
            Match match = await enumerator.MatchLineAsync(StatusLineRegex, cancellationToken).ConfigureAwait(false);

            // Get the groups and set.
            response.Version = new Version(match.GetGroupValue("version"));
            response.StatusCode = (HttpStatusCode) Int32.Parse(match.GetGroupValue("statusCode"));
            response.ReasonPhrase = match.GetGroupValue("reasonPhrase");
        }

        private static readonly Regex HeaderFieldRegex = new Regex(@"^(?<fieldName>.+?):\s*(?<fieldValue>.*?)\s*$",
            RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex FoldedHeaderFieldRegex = new Regex(@"^[\t ]+(?<value>.*)$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        internal static async Task SetHeaderFieldsAsync(this HttpResponseMessage response,
            IAsyncEnumerator<byte> enumerator,
            CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (response == null) throw new ArgumentNullException(nameof(response));
            if (enumerator == null) throw new ArgumentNullException(nameof(enumerator));

            // The line.
            string line;

            // The field and value.
            string field = null;
            string value = null;

            // While the line is not null or empty.
            while (!string.IsNullOrEmpty(line = await enumerator.ReadLineAsync(cancellationToken).ConfigureAwait(false)))
            {
                // The match.
                Match match;

                // Does this match a folded value?  If so, add to the previous value.
                if ((match = FoldedHeaderFieldRegex.Match(line)).Success)
                    // Add to the previous value.
                    value += " " + match.GetGroupValue("value");
                else if ((match = HeaderFieldRegex.Match(line)).Success)
                {
                    // If the field and value are set, then set them now.
                    if (field != null && value != null) response.Headers.TryAddWithoutValidation(field, value);

                    // Set the field and the value.
                    field = match.GetGroupValue("fieldName");
                    value = match.GetGroupValue("fieldValue");
                }
            }

            // If the field and value are set, then set them now.
            if (field != null && value != null) response.Headers.TryAddWithoutValidation(field, value);
        }

        internal static async Task SetContentAsync(this HttpResponseMessage response,
            IAsyncEnumerator<byte> enumerator,
            CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (response == null) throw new ArgumentNullException(nameof(response));
            if (enumerator == null) throw new ArgumentNullException(nameof(enumerator));
            
            // Check the transmission.
            if (response.Headers.TransferEncodingChunked ?? false)
                // Set the content.
                response.Content = await enumerator.ReadHttpContentFromChunkedTransferEncoding(response, cancellationToken).
                    ConfigureAwait(false);
            else
                // Read the remainder of the content.
                response.Content = await enumerator.ReadHttpContent(response, cancellationToken).
                    ConfigureAwait(false);
        }

        private static readonly Regex ChunkSizeRegex = new Regex(@"^(?<chunkSize>[0-9a-fA-F]+)(?<chunkExtension>;.*)?$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        internal static async Task<HttpContent> ReadHttpContentFromChunkedTransferEncoding(this IAsyncEnumerator<byte> enumerator,
            HttpResponseMessage response, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (enumerator == null) throw new ArgumentNullException(nameof(enumerator));
            if (response == null) throw new ArgumentNullException(nameof(response));

            // The memory stream.
            var ms = new MemoryStream();

            // Continue forever.
            while (true)
            {
                // Read the array buffer as a string.
                string chunkSize = await enumerator.ReadLineAsync(cancellationToken).ConfigureAwait(false);

                // Match.
                Match match = ChunkSizeRegex.Match(chunkSize);

                // Convert to a length.
                int length = int.Parse(match.GetGroupValue("chunkSize"), NumberStyles.HexNumber);

                // If 0, get out.
                if (length == 0 && !match.Groups["chunkExtension"].Success)
                    // Get out.
                    break;

                // While there are bytes to read.
                while (length-- > 0 && await enumerator.MoveNext(cancellationToken).ConfigureAwait(false))
                    ms.WriteByte(enumerator.Current);

                // Read the bytes.
                var line = await enumerator.ReadLineBytesAsync(cancellationToken).ConfigureAwait(false);

                // If not empty, throw.
                if (line.Count > 0) throw new InvalidOperationException("An unexpected byte sequence was encountered after processing chunked encoding data.");
            }

            // Set the header fields again.  This is for extra headers that
            // may be passed down.
            await response.SetHeaderFieldsAsync(enumerator, cancellationToken).ConfigureAwait(false);

            // Reset the memory stream.
            ms.Seek(0, SeekOrigin.Begin);

            // Return the content.
            return new StreamContent(ms);
        }

        internal static async Task<HttpContent> ReadHttpContent(this IAsyncEnumerator<byte> enumerator,
            HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (enumerator == null) throw new ArgumentNullException(nameof(enumerator));
            if (response == null) throw new ArgumentNullException(nameof(response));

            // The content length string.
            string contentLengthString;

            // The content length.
            int contentLength = response.Headers.TryGetValue("Content-Length", out contentLengthString) ?
                int.Parse(contentLengthString) : -1;

            // The memory stream.
            var ms = contentLength > 0 ? new MemoryStream(contentLength) : new MemoryStream();

            // Cycle through the bytes, pushing to the memory stream.
            while ((contentLength < 0 || contentLength-- > 0) && await enumerator.MoveNext(cancellationToken).ConfigureAwait(false))
                ms.WriteByte(enumerator.Current);

            // Seek to the beginning of the stream.
            ms.Seek(0, SeekOrigin.Begin);

            // Return the content.
            return new StreamContent(ms);
        }
    }
}
