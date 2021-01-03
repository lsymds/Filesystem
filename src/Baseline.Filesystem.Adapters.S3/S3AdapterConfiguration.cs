using Amazon.S3;
using Baseline.Filesystem.Configuration;

namespace Baseline.Filesystem.Adapters.S3
{
    /// <summary>
    /// Configuration options for the S3 adapter.
    /// </summary>
    public class S3AdapterConfiguration : BaseAdapterConfiguration
    {
        /// <summary>
        /// Gets or sets the S3 client to use in the adapter.
        /// </summary>
        public IAmazonS3 S3Client { get; set; }
        
        /// <summary>
        /// Gets or sets the bucket name used in the S3 adapter's actions.
        /// </summary>
        public string BucketName { get; set; }
    }
}
