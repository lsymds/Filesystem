using System;

namespace Baseline.Filesystem
{
    /// <summary>
    /// Response returned from the GetPublicUrl method that retrieves a publicly accessible URL for the requested
    /// file.
    /// </summary>
    public class GetFilePublicUrlResponse
    {
        /// <summary>
        /// Gets or sets the public URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets when the public URL expires, if applicable.
        /// </summary>
        public DateTime? Expiry { get; set; }
    }
}
