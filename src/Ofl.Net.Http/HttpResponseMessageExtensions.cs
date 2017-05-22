using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ofl.Net.Http
{
    public static class HttpResponseMessageExtensions
    {
        public static Task<T> ToObjectAsync<T>(this HttpResponseMessage httpResponseMessage,
            CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpResponseMessage == null) throw new ArgumentNullException(nameof(httpResponseMessage));

            // Call the overload,
            return httpResponseMessage.ToObjectAsync<T>(JsonSerializer.CreateDefault(), cancellationToken);
        }

        public static Task<T> ToObjectAsync<T>(this HttpResponseMessage httpResponseMessage,
            JsonSerializerSettings jsonSerializerSettings, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpResponseMessage == null) throw new ArgumentNullException(nameof(httpResponseMessage));
            if (jsonSerializerSettings == null) throw new ArgumentNullException(nameof(jsonSerializerSettings));

            // Call the overload,
            return httpResponseMessage.ToObjectAsync<T>(JsonSerializer.Create(jsonSerializerSettings), cancellationToken);
        }

        public static async Task<T> ToObjectAsync<T>(this HttpResponseMessage httpResponseMessage,
            JsonSerializer jsonSerializer, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (httpResponseMessage == null) throw new ArgumentNullException(nameof(httpResponseMessage));
            if (jsonSerializer == null) throw new ArgumentNullException(nameof(jsonSerializer));

            // Get the stream.
            using (Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false))
            // Text reader on top of stream.
            using (var streamReader = new StreamReader(stream))
            // Create a JsonReader.
            using (JsonReader jsonReader = new JsonTextReader(streamReader))
                // Deserialize and return.
                return jsonSerializer.Deserialize<T>(jsonReader);
        }
    }
}
