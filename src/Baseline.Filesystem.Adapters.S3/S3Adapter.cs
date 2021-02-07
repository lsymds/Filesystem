using Amazon.S3;

namespace Baseline.Filesystem.Adapters.S3
{
    /// <summary>
    /// Provides the shared, directory/file agnostic functions of the <see cref="IAdapter"/> implementation for
    /// Amazon's Simple Storage Service. 
    /// </summary>
    public partial class S3Adapter : IAdapter
    {
        private readonly S3AdapterConfiguration _adapterConfiguration;
        private readonly IAmazonS3 _s3Client;

        /// <summary>
        /// Initialises a new instance of the <see cref="S3Adapter"/>, configuring it with options passed in by the
        /// consuming application.
        /// </summary>
        /// <param name="adapterConfiguration">The configuration used to configure this adapter.</param>
        public S3Adapter(S3AdapterConfiguration adapterConfiguration)
        {
            _adapterConfiguration = adapterConfiguration;
            _s3Client = _adapterConfiguration.S3Client;
        }
    }
}
