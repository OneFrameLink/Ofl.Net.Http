using System;
using System.Net;
using System.Net.Http;

namespace Ofl.Net.Http
{
    public static class HttpClientHandlerExtensions
    {
        public static void SetCompression(this HttpClientHandler handler)
        {
            // Validate parameters.
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            // Set compression.
            handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
        }
    }
}
