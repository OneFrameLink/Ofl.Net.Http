using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Ofl.Net.Http
{
    //////////////////////////////////////////////////
    ///
    /// <author>Nicholas Paldino</author>
    /// <created>2014-04-12</created>
    /// <summary>Extensions for the <see cref="HttpRequestHeaders"/>
    /// class.</summary>
    ///
    //////////////////////////////////////////////////
    public static class HttpRequestMessageExtensions
    {
        //////////////////////////////////////////////////
        ///
        /// <author>Nicholas Paldino</author>
        /// <created>2014-04-12</created>
        /// <summary>Sets a <see cref="HttpRequestMessage"/> to use Basic
        /// HTTP authentication.</summary>
        /// <param name="message">The <see cref="HttpRequestMessage"/>
        /// to set the authentication on.</param>
        /// <param name="username">The username to use.</param>
        /// <param name="password">The password.</param>
        /// 
        //////////////////////////////////////////////////
        public static void SetBasicHttpAuthentication(this HttpRequestMessage message,
            string username, string password)
        {
            // Validate the parameters.
            if (message == null) throw new ArgumentNullException(nameof(message));

            // Do not validate username and password, they might be intentionally null.

            // The scheme.
            const string scheme = "Basic";

            // The parameter template.
            const string parameterTemplate = "{0}:{1}";

            // Format the parameter.
            string parameter = string.Format(CultureInfo.InvariantCulture, parameterTemplate, username, password);

            // Get the bytes, convert to base 64.
            parameter = Convert.ToBase64String(Encoding.ASCII.GetBytes(parameter));

            // Add the header values.
            message.Headers.Authorization = new AuthenticationHeaderValue(scheme, parameter);
        }
    }
}
