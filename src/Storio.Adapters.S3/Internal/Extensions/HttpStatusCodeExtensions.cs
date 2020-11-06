using System.Linq;
using System.Net;

namespace Storio.Adapters.S3.Internal.Extensions
{
    /// <summary>
    /// Extension methods related to the <see cref="HttpStatusCode"/> type.
    /// </summary>
    internal static class HttpStatusCodeExtensions
    {
        /// <summary>
        /// Array containing a collection of successful HTTP status codes.
        /// </summary>
        private static readonly HttpStatusCode[] SuccessfulStatusCodes = new[]
        {
            HttpStatusCode.Accepted,
            HttpStatusCode.Created,
            HttpStatusCode.NoContent,
            HttpStatusCode.OK,
        };
        
        /// <summary>
        /// Checks whether the status code is a successful response from Amazon's Simple Storage Service.
        /// </summary>
        /// <param name="statusCode">The status code to check.</param>
        /// <returns>Whether the status code is successful or not.</returns>
        public static bool IsSuccessStatusCode(this HttpStatusCode statusCode)
        {
            return SuccessfulStatusCodes.Any(x => x == statusCode);
        }
    }
}
