using System.Net.Http;

namespace Ofl.Net.Http
{
    public static class HttpMessageHandlerExtensions
    {
        public static HttpMessageHandler CreateDefaultMessageHandler()
        {
            // Create the handler.
            var handler = new HttpClientHandler();

            // Always handle compression.
            handler.SetCompression();

            // Return the handler.
            return handler;
        }
    }
}
