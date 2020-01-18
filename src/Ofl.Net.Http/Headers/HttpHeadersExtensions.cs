using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;

namespace Ofl.Net.Http.Headers
{
    public static class HttpHeadersExtensions
    {
        public static bool TryGetValue(
            this HttpHeaders headers, 
            string name, 
            [NotNullWhen(true)]
            out string? value
        )
        {
            // Validate parameters.
            if (headers == null) throw new ArgumentNullException(nameof(headers));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            // Default value.
            value = null;

            // Try and get the headers, if there are none, return false.
            if (!headers.TryGetValues(name, out IEnumerable<string> values)) return false;

            // Get the single value and return.
            value = values.Single();

            // It was found.
            return true;
        }
    }
}
